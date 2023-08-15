using Abp.Dependency;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Erpinfo.Entities;
using Erpinfo.Services.KomuService;
using Newtonsoft.Json;
using Erpinfo.Services.KomuService.KomuDto;
using Erpinfo.Configuration;
using Erpinfo.Constant;
using Erpinfo.Authorization.Users;
using Abp.Configuration;
using Erpinfo.Enums;
using Abp.Domain.Uow;
using Erpinfo.Uitls;
using Erpinfo.Helper;

namespace Erpinfo.BackGroundWorker
{
    public class Sedulche : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        public Sedulche(AbpTimer timer) : base(timer)
        {
            timer.Period = 3600000;
            //timer.Period = 30000;
        }
        protected override void DoWork()
        {
            new DoWork().Excute();
        }
    }
    public class DoWork : ErpinfoAppServiceBase
    {
        private static bool isExcuted;
        private readonly KomuService komuService;
        private ISettingManager settingManager;
        public DoWork()
        {
            this.settingManager = IocManager.Instance.Resolve<SettingManager>();
            this.komuService = IocManager.Instance.Resolve<KomuService>();
        }

        public void Excute()
        {
            // ExcuteContest().Wait();
            //NotificationRemindWork().Wait();
        }
        //protected async Task ExcuteContest()
        //{
        //    var currentDoing = dbContext.ContestImages.Where(s => (s.StartDate.Date == DateTime.Now.Date && s.StartDate.Hour == DateTime.Now.Hour)
        //       || (s.EndDate.Date == DateTime.Now.Date && s.EndDate.Hour == DateTime.Now.Hour)
        //    );
        //    foreach (var i in currentDoing)
        //    {
        //        if (i.StatusOfEventPublish == Entities.StatusOfEventPublish.Pending)
        //        {
        //            i.StatusOfEventPublish = Entities.StatusOfEventPublish.Doing;
        //        }
        //        if (i.StatusOfEventPublish == Entities.StatusOfEventPublish.Doing)
        //        {
        //            i.StatusOfEventPublish = Entities.StatusOfEventPublish.Ended;
        //        }
        //    }
        //    await dbContext.SaveChangesAsync();
        //}
        [UnitOfWork]
        protected async Task NotificationRemindWork()
        {
            var dateNow = DateTimeUtils.GetNow();
            var listJobRemind = new List<UserJob>();
            //Nhac cong viec chuan bi den deadline
            var listJobBeforeDeadline = await WorkScope.GetAll<UserJob>().Where(s => s.NumberReminded == 0 && s.Job.Status != JobStatus.Done)
                .Where(s => s.Job.Deadline.Value.AddMinutes(-s.Remind.Value) < dateNow).ToListAsync();
            listJobRemind.AddRange(listJobBeforeDeadline);
            //Nhac cong viec vua qua deadline
            var listJobAfterDeadline = await WorkScope.GetAll<UserJob>().Where(s => s.NumberReminded == 1 && s.Job.Status != JobStatus.Done).Where(s => s.Job.Deadline.Value < dateNow).ToListAsync();
            listJobRemind.AddRange(listJobAfterDeadline);
            // Nhac cong viec khong cap nhat trang thai sau 1 month
            var listJobAfteraMonth = await WorkScope.GetAll<UserJob>().Where(s => !s.Job.Deadline.HasValue).Where(s => s.NumberReminded == 0)
                .Where(s => s.Job.CreationTime.AddMonths(1) < dateNow).Where(s => s.Job.Status == JobStatus.Todo).ToListAsync();
            listJobRemind.AddRange(listJobAfteraMonth);
            // Nhac cong viec khong cap nhat trang thai sau 2 month
            var listJobAfterTwoMonth = await WorkScope.GetAll<UserJob>().Where(s => !s.Job.Deadline.HasValue)
                .Where(s => s.Job.CreationTime.AddMonths(2) < dateNow).Where(s => s.Job.Status == JobStatus.Todo).ToListAsync();
            listJobRemind.AddRange(listJobAfterTwoMonth);
            foreach (var l in listJobRemind)
            {
                var user = await WorkScope.GetAsync<User>(l.UserId);
                var userName = UserHelper.GetUserName(user.EmailAddress);
                if (user != null && !user.KomuUserId.HasValue)
                {
                    user.KomuUserId = await komuService.GetKomuUserId(new KomuUserDto { Username = userName ?? user.UserName }, ChannelTypeConstant.KOMU_USER);
                    await WorkScope.UpdateAsync(user);
                }
                var isBeforeDeadline = listJobBeforeDeadline.Any(s => s.Id == l.Id);
                var isAfterDeadline = listJobAfterDeadline.Any(s => s.Id == l.Id);
                var isAfteraMonth = listJobAfteraMonth.Any(s => s.Id == l.Id);
                var isAfterTwoMonth = listJobAfterTwoMonth.Any(s => s.Id == l.Id);
                var message = new StringBuilder();
                if (isBeforeDeadline)
                {
                    message.AppendLine($"Chào {(user.KomuUserId.HasValue ? "<@" + user.KomuUserId.ToString() + ">" : "**" + (userName ?? user.UserName) + "**")}. Bạn có công việc sắp đến deadline.");
                    l.NumberReminded = 1;

                }
                else if (isAfterDeadline)
                {
                    message.AppendLine($"Chào {(user.KomuUserId.HasValue ? "<@" + user.KomuUserId.ToString() + ">" : "**" + (userName ?? user.UserName) + "**")}. Bạn có công việc quá deadline.");
                    l.NumberReminded = 2;

                }
                else if (isAfteraMonth)
                {
                    message.AppendLine($"Chào {(user.KomuUserId.HasValue ? "<@" + user.KomuUserId.ToString() + ">" : "**" + (userName ?? user.UserName) + "**")}. Bạn có công việc sau 1 tháng mà chưa hoàn thành.");
                    l.NumberReminded = 1;
                }
                else
                {
                    message.AppendLine($"Chào {(user.KomuUserId.HasValue ? "<@" + user.KomuUserId.ToString() + ">" : "**" + (userName ?? user.UserName) + "**")}. Bạn có công việc sau 2 tháng mà chưa hoàn thành.");
                    l.NumberReminded = 2;

                }
                var titlelink = $"{EntityConstant.Frontend}#/task/myTask/{l.Id}";
                message.AppendLine(titlelink);
                await komuService.NotifyToChannel(new KomuMessage
                {
                    UserName = userName ?? user.UserName,
                    Message = message.ToString(),
                    CreateDate = DateTimeUtils.GetNow(),
                }, ChannelTypeConstant.USER_ONLY);

            }
            await WorkScope.UpdateRangeAsync(listJobRemind);
        }

    }
}
