using Abp.BackgroundJobs;
using Abp.Dependency;
using Erpinfo.Configuration;
using Erpinfo.Constant;
using Erpinfo.Entities;
using Erpinfo.IoC;
using Erpinfo.Services.KomuService;
using Erpinfo.Services.KomuService.KomuDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Abp.Domain.Uow;
using Erpinfo.Helper;
using Erpinfo.Uitls;

namespace Erpinfo.NotificationKomuBackgroundJob
{
    public class NotificationBackgroundJob : BackgroundJob<NotificationBackgroundJobArgs>, ITransientDependency
    {
        private readonly KomuService komuService;
        private IWorkScope workScope;
        public NotificationBackgroundJob(KomuService komuService)
        {
            this.komuService = komuService;
            workScope = IocManager.Instance.Resolve<IWorkScope>();
        }
        [UnitOfWork]
        public override async void Execute(NotificationBackgroundJobArgs args)
        {
            try
            {
                var userJob = workScope.GetAll<UserJob>().Where(s => s.Id == args.UserJobId).FirstOrDefault();
                if (userJob != null)
                {
                    if (userJob.Job?.Status == Enums.JobStatus.Todo && userJob.User != null)
                    {
                        var username = UserHelper.GetUserName(userJob.User?.EmailAddress);
                        if (!userJob.User.KomuUserId.HasValue)
                        {
                            userJob.User.KomuUserId = await komuService.GetKomuUserId(new KomuUserDto { Username = username ?? userJob.User.UserName }, ChannelTypeConstant.KOMU_USER);
                            await workScope.UpdateAsync(userJob.User);
                        }
                        var message = new StringBuilder();
                        var notification = $"Chào {(userJob.User.KomuUserId.HasValue ? "<@" + userJob.User.KomuUserId + ">" : "**" + (username ?? userJob.User.UserName) + "**")}. " + args.Message + ": " + userJob.Job.Name;
                        var titlelink = $"{EntityConstant.Frontend}#/task/myTask/{args.UserJobId}";
                        message.AppendLine(notification);
                        message.AppendLine(titlelink);
                        await komuService.NotifyToChannel(new KomuMessage
                        {
                            UserName = username ?? userJob.User.UserName,
                            Message = message.ToString(),
                            CreateDate = DateTimeUtils.GetNow(),
                        }, ChannelTypeConstant.USER_ONLY);
                    }
                }
                // check cong viec no deadline da thay doi trang thai hay chua

            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }
    }
}
