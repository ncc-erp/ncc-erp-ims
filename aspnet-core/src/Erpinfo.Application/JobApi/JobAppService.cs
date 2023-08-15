using Erpinfo.Entities;
using Erpinfo.JobApi.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using Abp.UI;
using Erpinfo.Authorization.Users;
using Erpinfo.Enums;
using Erpinfo.Authorization;
using Erpinfo.Configuration;
using Erpinfo.Constant;
using Abp.Linq.Extensions;
using Abp.Collections.Extensions;
using System.IO;
using OfficeOpenXml;
using Erpinfo.Services.KomuService.KomuDto;
using Newtonsoft.Json;
using Erpinfo.Services.KomuService;
using Abp.BackgroundJobs;
using Erpinfo.NotificationKomuBackgroundJob;
using Abp.Modules;

namespace Erpinfo.JobApi
{
    [AbpAuthorize]
    public class JobAppService : ErpinfoAppServiceBase
    {
        private readonly KomuService komuService;
        private readonly IBackgroundJobManager _backgroundJobManager;
        public JobAppService(KomuService komuService, IBackgroundJobManager backgroundJobManager)
        {
            this.komuService = komuService;
            _backgroundJobManager = backgroundJobManager;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Job.Create, PermissionNames.Job.Edit)]

        public async Task<CreateJobDto> Save(CreateJobDto input)
        {
            var dateNow = DateTime.Now;
            double jobDelay1 = 0;
            double jobDelay2 = 0;
            double addSecond = 0;
            if (input.Deadline.HasValue && input.Remind.HasValue)
            {
                jobDelay1 = input.Deadline.Value.AddMinutes(-input.Remind.Value).Subtract(dateNow).TotalSeconds;
                jobDelay2 = input.Deadline.Value.AddMilliseconds(10).Subtract(dateNow).TotalSeconds;
            }
            var jobDelay3 = dateNow.AddMonths(1).Subtract(dateNow).TotalSeconds;
            var jobDelay4 = dateNow.AddMonths(2).Subtract(dateNow).TotalSeconds;
            if (input.Id <= 0)
            {
                var job = ObjectMapper.Map<Job>(input);
                input.Id = await WorkScope.InsertAndGetIdAsync<Job>(job);
                foreach (var u in input.UserId)
                {
                    var hasOrder = await WorkScope.GetAll<UserJob>().Where(s => s.UserId == u).AnyAsync(s => s.Order != null);
                    int orderMax = 0;
                    if (hasOrder)
                    {
                        orderMax = (int)await WorkScope.GetAll<UserJob>().Where(s => s.UserId == u).Select(s => s.Order.Value).MaxAsync();
                    }
                    orderMax++;
                    var uj = new UserJob
                    {
                        UserId = u,
                        JobId = input.Id,
                        Order = orderMax,
                        Remind = input.Remind
                    };
                    long userJobId = await WorkScope.InsertAndGetIdAsync<UserJob>(uj);
                    //add into user canlendar
                    if (input.Deadline != null)
                    {
                        await WorkScope.InsertAndGetIdAsync<UserCalendar>(new UserCalendar
                        {
                            UserId = u,
                            Date = input.Deadline.Value,
                            Entity = "UserJob",
                            EntityId = userJobId,
                            Note = input.Name
                        });
                        if (input.Status == JobStatus.Todo)
                        {
                            if (input.Remind.HasValue && input.Deadline.Value.AddMinutes(-input.Remind.Value) > dateNow)
                            {
                                await _backgroundJobManager.EnqueueAsync<NotificationBackgroundJob, NotificationBackgroundJobArgs>(new NotificationBackgroundJobArgs
                                {
                                    JobId = input.Id,
                                    UserJobId = userJobId,
                                    Message = $"Bạn có công việc sắp đến deadline"
                                }, BackgroundJobPriority.High, TimeSpan.FromSeconds(jobDelay1 + addSecond));
                                await _backgroundJobManager.EnqueueAsync<NotificationBackgroundJob, NotificationBackgroundJobArgs>(new NotificationBackgroundJobArgs
                                {
                                    JobId = input.Id,
                                    UserJobId = userJobId,
                                    Message = $"Bạn có công việc đã quá deadline"
                                }, BackgroundJobPriority.High, TimeSpan.FromSeconds(jobDelay2 + addSecond));
                                addSecond += 2;
                            }
                        }

                    }
                    else
                    {
                        if (input.Status == JobStatus.Todo)
                        {
                            await _backgroundJobManager.EnqueueAsync<NotificationBackgroundJob, NotificationBackgroundJobArgs>(new NotificationBackgroundJobArgs
                            {
                                JobId = input.Id,
                                UserJobId = userJobId,
                                Message = $"Bạn có công việc sau 1 tháng mà chưa hoàn thành"
                            }, BackgroundJobPriority.High, TimeSpan.FromSeconds(jobDelay3 + addSecond));
                            await _backgroundJobManager.EnqueueAsync<NotificationBackgroundJob, NotificationBackgroundJobArgs>(new NotificationBackgroundJobArgs
                            {
                                JobId = input.Id,
                                UserJobId = userJobId,
                                Message = $"Bạn có công việc sau 2 tháng mà chưa hoàn thành"
                            }, BackgroundJobPriority.High, TimeSpan.FromSeconds(jobDelay4 + addSecond));
                            addSecond += 2;
                        }
                    }
                }
            }
            else
            {
                //background job 
                var jobIdString = $"\"JobId\":{input.Id}";
                var listBgr = await WorkScope.GetAll<BackgroundJobInfo>().Where(s => s.JobArgs.Contains(jobIdString)).ToListAsync();
                foreach (var l in listBgr)
                {
                    await WorkScope.DeleteAsync(l);
                }
                var job = await WorkScope.GetAsync<Job>(input.Id);
                var deadlineOld = job.Deadline;
                ObjectMapper.Map<CreateJobDto, Job>(input, job);
                await WorkScope.UpdateAsync(job);
                //update userJob
                var oldUser = await WorkScope.GetAll<UserJob>().Where(s => s.JobId == input.Id).Select(s => s.UserId).ToListAsync();
                var newUser = input.UserId;
                var deleteUserJob = oldUser.Except(newUser);
                var insertUserJob = newUser.Except(oldUser);
                var listNoChange = await WorkScope.GetAll<UserJob>().Where(s => s.JobId == input.Id).Where(s => oldUser.Intersect(newUser).Contains(s.UserId)).ToListAsync();
                foreach (var l in listNoChange)
                {
                    l.Remind = input.Remind;
                }
                await WorkScope.UpdateRangeAsync(listNoChange);
                if (input.Status == JobStatus.Todo)
                {
                    if (input.Deadline != null)
                    {
                        foreach (var l in listNoChange)
                        {
                            if (input.Remind.HasValue && input.Deadline.Value.AddMinutes(-input.Remind.Value) > dateNow)
                            {
                                await _backgroundJobManager.EnqueueAsync<NotificationBackgroundJob, NotificationBackgroundJobArgs>(new NotificationBackgroundJobArgs
                                {
                                    JobId = input.Id,
                                    UserJobId = l.Id,
                                    Message = $"Bạn có công việc sắp đến deadline"
                                }, BackgroundJobPriority.High, TimeSpan.FromSeconds(jobDelay1 + addSecond));
                                await _backgroundJobManager.EnqueueAsync<NotificationBackgroundJob, NotificationBackgroundJobArgs>(new NotificationBackgroundJobArgs
                                {
                                    JobId = input.Id,
                                    UserJobId = l.Id,
                                    Message = $"Bạn có công việc đã quá deadline"
                                }, BackgroundJobPriority.High, TimeSpan.FromSeconds(jobDelay2 + addSecond));
                                addSecond += 2;
                            }
                        }
                    }
                    else
                    {
                        foreach (var l in listNoChange)
                        {
                            await _backgroundJobManager.EnqueueAsync<NotificationBackgroundJob, NotificationBackgroundJobArgs>(new NotificationBackgroundJobArgs
                            {
                                JobId = input.Id,
                                UserJobId = l.Id,
                                Message = $"Bạn có công việc sau 1 tháng mà chưa hoàn thành"
                            }, BackgroundJobPriority.High, TimeSpan.FromSeconds(jobDelay3 + addSecond));
                            await _backgroundJobManager.EnqueueAsync<NotificationBackgroundJob, NotificationBackgroundJobArgs>(new NotificationBackgroundJobArgs
                            {
                                JobId = input.Id,
                                UserJobId = l.Id,
                                Message = $"Bạn có công việc sau 2 tháng mà chưa hoàn thành"
                            }, BackgroundJobPriority.High, TimeSpan.FromSeconds(jobDelay4 + addSecond));
                            addSecond += 2;
                        }
                    }
                }

                if (deadlineOld == null)
                {
                    if (input.Deadline.HasValue)
                    {
                        foreach (var u in listNoChange)
                        {
                            await WorkScope.InsertAndGetIdAsync<UserCalendar>(new UserCalendar
                            {
                                UserId = u.UserId,
                                Date = input.Deadline.Value,
                                Entity = "UserJob",
                                EntityId = u.Id,
                                Note = input.Name
                            });
                        }
                    }
                }
                else
                {
                    if (!input.Deadline.HasValue)
                    {
                        foreach (var u in listNoChange)
                        {
                            var userCalendar = await WorkScope.GetAll<UserCalendar>().Where(s => s.EntityId == u.Id && s.Entity == EntityConstant.UserJobEntity).FirstOrDefaultAsync();
                            await WorkScope.DeleteAsync<UserCalendar>(userCalendar.Id);
                        }
                    }
                    else
                    {
                        if (input.Deadline != deadlineOld)
                        {
                            foreach (var u in listNoChange)
                            {
                                var userCalendar = await WorkScope.GetAll<UserCalendar>().Where(s => s.EntityId == u.Id && s.Entity == EntityConstant.UserJobEntity).FirstOrDefaultAsync();
                                userCalendar.Date = input.Deadline.Value;
                                await WorkScope.UpdateAsync<UserCalendar>(userCalendar);
                            }
                        }
                    }
                }
                foreach (var u in insertUserJob)
                {
                    var hasOrder = await WorkScope.GetAll<UserJob>().Where(s => s.UserId == u).AnyAsync(s => s.Order != null);
                    int orderMax = 0;
                    if (hasOrder)
                    {
                        orderMax = (int)await WorkScope.GetAll<UserJob>().Where(s => s.UserId == u).Select(s => s.Order.Value).MaxAsync();
                    }
                    orderMax++;
                    var uj = new UserJob
                    {
                        UserId = u,
                        JobId = input.Id,
                        Order = orderMax,
                        Remind = input.Remind
                    };
                    long userJobId = await WorkScope.InsertAndGetIdAsync<UserJob>(uj);
                    //add into user canlendar
                    if (input.Deadline != null)
                    {
                        await WorkScope.InsertAndGetIdAsync<UserCalendar>(new UserCalendar
                        {
                            UserId = u,
                            Date = input.Deadline.Value,
                            Entity = "UserJob",
                            EntityId = userJobId,
                            Note = input.Name
                        });
                        if (input.Status == JobStatus.Todo)
                        {
                            if (input.Remind.HasValue && input.Deadline.Value.AddMinutes(-input.Remind.Value) > dateNow)
                            {
                                await _backgroundJobManager.EnqueueAsync<NotificationBackgroundJob, NotificationBackgroundJobArgs>(new NotificationBackgroundJobArgs
                                {
                                    JobId = input.Id,
                                    UserJobId = userJobId,
                                    Message = $"Bạn có công việc sắp đến deadline"
                                }, BackgroundJobPriority.High, TimeSpan.FromSeconds(jobDelay1 + addSecond));
                                await _backgroundJobManager.EnqueueAsync<NotificationBackgroundJob, NotificationBackgroundJobArgs>(new NotificationBackgroundJobArgs
                                {
                                    JobId = input.Id,
                                    UserJobId = userJobId,
                                    Message = $"Bạn có công việc đã quá deadline"
                                }, BackgroundJobPriority.High, TimeSpan.FromSeconds(jobDelay2 + addSecond));
                                addSecond += 2;
                            }
                        }
                        else
                        {
                            if (input.Status == JobStatus.Todo)
                            {
                                await _backgroundJobManager.EnqueueAsync<NotificationBackgroundJob, NotificationBackgroundJobArgs>(new NotificationBackgroundJobArgs
                                {
                                    JobId = input.Id,
                                    UserJobId = userJobId,
                                    Message = $"Bạn có công việc sau 1 tháng mà chưa hoàn thành"
                                }, BackgroundJobPriority.High, TimeSpan.FromSeconds(jobDelay3 + addSecond));
                                await _backgroundJobManager.EnqueueAsync<NotificationBackgroundJob, NotificationBackgroundJobArgs>(new NotificationBackgroundJobArgs
                                {
                                    JobId = input.Id,
                                    UserJobId = userJobId,
                                    Message = $"Bạn có công việc sau 2 tháng mà chưa hoàn thành"
                                }, BackgroundJobPriority.High, TimeSpan.FromSeconds(jobDelay4 + addSecond));
                                addSecond += 2;
                            }
                        }
                    }
                }
                var listUserJobDelete = await WorkScope.GetAll<UserJob>().Where(s => s.JobId == input.Id && deleteUserJob.Contains(s.UserId))
                    .Select(s => s.Id).ToListAsync();
                foreach (var l in listUserJobDelete)
                {
                    await WorkScope.DeleteAsync<UserJob>(l);
                }
                //delete userCalendar
                var listUserCalendarDelete = await WorkScope.GetAll<UserCalendar>().Where(s => listUserJobDelete.Contains(s.EntityId) && s.Entity == EntityConstant.UserJobEntity)
                    .Select(s => s.Id).ToListAsync();
                foreach (var l in listUserCalendarDelete)
                {
                    await WorkScope.DeleteAsync<UserCalendar>(l);
                }
            }
            return input;
        }
        [HttpDelete]
        [AbpAuthorize(PermissionNames.Job.Delete)]
        public async Task Delete(long id)
        {
            var job = await WorkScope.GetAsync<Job>(id);
            if (job == null)
            {
                throw new UserFriendlyException(string.Format("Job id = {0} not found", id));
            }
            //delete backgroundjob
            var jobIdString = $"\"JobId\":{id}";
            var listBgr = await WorkScope.GetAll<BackgroundJobInfo>().Where(s => s.JobArgs.Contains(jobIdString)).ToListAsync();
            foreach (var l in listBgr)
            {
                await WorkScope.DeleteAsync(l);
            }
            var listUserJob = await WorkScope.GetAll<UserJob>().Where(s => s.JobId == id).Select(s => s.Id).ToListAsync();
            foreach (var u in listUserJob)
            {
                await WorkScope.DeleteAsync<UserJob>(u);
                //delete userCalendar
                var userCalendar = await WorkScope.GetAll<UserCalendar>().Where(s => s.EntityId == u && s.Entity == EntityConstant.UserJobEntity)
                    .Select(s => s.Id).FirstOrDefaultAsync();
                await WorkScope.DeleteAsync<UserCalendar>(userCalendar);
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            await WorkScope.DeleteAsync<Job>(id);
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.MyJob.View)]

        public async Task<List<MyJobDto>> GetAllMyJob(DateTime? startDate, DateTime? endDate, JobStatus? status, long? reporterId)
        {
            var userId = AbpSession.UserId.Value;
            var now = DateTime.Now;
            var myJobs = await WorkScope.GetAll<UserJob>().Where(s => s.UserId == userId)
                .Where(s => !startDate.HasValue || s.Job.Deadline >= startDate)
                .Where(s => !endDate.HasValue || s.Job.Deadline <= endDate)
                .Where(s => !status.HasValue || s.Job.Status == status)
                .Where(s => !reporterId.HasValue || s.Job.CreatorUserId == reporterId)
                .OrderByDescending(s => s.Order)
                .Select(s => new MyJobDto
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    JobId = s.JobId,
                    Order = s.Order,
                    Remind = s.Remind,
                    Comment = s.Job.Comment,
                    Deadline = s.Job.Deadline,
                    Description = s.Job.Description,
                    Name = s.Job.Name,
                    Status = s.Job.Status,
                    InTimelimit = !(s.Job.Deadline < now && s.Job.Status != JobStatus.Done),
                    CreationTime = s.Job.CreationTime,
                    ReporterId = s.Job.CreatorUserId.Value
                }).ToListAsync();
            foreach (var m in myJobs)
            {
                var reporter = WorkScope.GetAll<User>().Where(k => k.Id == m.ReporterId).Select(k => k.FullName).FirstOrDefault();
                m.Reporter = reporter;
            }
            return myJobs;
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.MyJob.ViewDetail)]

        public async Task<GetDetailMyJobDto> GetMyJob(long id)
        {
            var now = DateTime.Now;
            var myJob = await WorkScope.GetAll<UserJob>().Where(s => s.Id == id).FirstOrDefaultAsync();
            var listUser = await WorkScope.GetAll<UserJob>().Where(s => s.JobId == myJob.JobId).Select(s => new GetUserJobDto
            {
                FullName = s.User.FullName,
                UserId = s.UserId
            }).ToListAsync();
            var job = await WorkScope.GetAll<UserJob>()
                .Where(s => s.Id == id)
                .Select(s => new GetDetailMyJobDto
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    JobId = s.JobId,
                    Order = s.Order,
                    Remind = s.Remind,
                    Comment = s.Job.Comment,
                    Deadline = s.Job.Deadline,
                    Description = s.Job.Description,
                    Name = s.Job.Name,
                    Status = s.Job.Status,
                    InTimelimit = !(s.Job.Deadline < now && s.Job.Status != JobStatus.Done),
                    CreationTime = s.Job.CreationTime,
                    ReporterId = s.Job.CreatorUserId.Value,
                    Receiver = listUser
                }).FirstOrDefaultAsync();
            var reporter = WorkScope.GetAll<User>().Where(k => k.Id == job.ReporterId).Select(k => k.FullName).FirstOrDefault();
            job.Reporter = reporter;
            return job;
        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.Job.View)]

        public async Task<List<GetJobDto>> GetAllCreatedJob(DateTime? startDate, DateTime? endDate, JobStatus? status, long? receiverId)
        {
            var creatorUserId = AbpSession.UserId.Value;
            var now = DateTime.Now;
            return WorkScope.GetRepo<UserJob>().GetAllIncluding(s => s.Job, k => k.User)
                .Where(s => !startDate.HasValue || s.Job.Deadline >= startDate)
                .Where(s => !endDate.HasValue || s.Job.Deadline <= endDate)
                .Where(s => !status.HasValue || s.Job.Status == status)
                .Where(s => !receiverId.HasValue || s.UserId == receiverId)
                .Where(s => s.Job.CreatorUserId == creatorUserId).AsEnumerable()
                .OrderByDescending(s => s.Job.CreationTime)
                .Select(s => new
                {
                    s.Job.Id,
                    s.Job?.Comment,
                    s.Job?.Deadline,
                    s.Job?.Description,
                    s.Job?.Name,
                    s.Job?.Status,
                    s.Remind,
                    UserId = s.User.Id,
                    s.Job?.CreationTime,
                    s.User?.FullName
                }).GroupBy(s => new { s.Id, s.Comment, s.Deadline, s.Description, s.Name, s.Status, s.CreationTime, s.Remind }).Select(s => new GetJobDto
                {
                    Id = s.Key.Id,
                    Status = (Enums.JobStatus)s.Key.Status,
                    Name = s.Key.Name,
                    Description = s.Key.Description,
                    CreationTime = s.Key.CreationTime,
                    Deadline = s.Key.Deadline,
                    InTimelimit = !(s.Key.Deadline < now && s.Key.Status != JobStatus.Done),
                    Comment = s.Key.Comment,
                    Remind = s.Key.Remind,
                    Users = s.Select(k => new GetUserJobDto
                    {
                        UserId = k.UserId,
                        FullName = k.FullName
                    })
                }).ToList();
        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.Job.ViewDetail)]

        public async Task<GetJobDto> GetCreatedJob(long id)
        {
            var now = DateTime.Now;
            return WorkScope.GetRepo<UserJob>().GetAllIncluding(s => s.Job, k => k.User)
                .Where(s => s.Job.Id == id).AsEnumerable()
                .Select(s => new
                {
                    s.Job.Id,
                    s.Job?.Comment,
                    s.Job?.Deadline,
                    s.Job?.Description,
                    s.Job?.Name,
                    s.Job?.Status,
                    s.Remind,
                    UserId = s.User.Id,
                    s.Job?.CreationTime,
                    s.User?.FullName
                }).GroupBy(s => new { s.Id, s.Comment, s.Deadline, s.Description, s.Name, s.Status, s.CreationTime, s.Remind }).Select(s => new GetJobDto
                {
                    Id = s.Key.Id,
                    Status = (Enums.JobStatus)s.Key.Status,
                    Name = s.Key.Name,
                    Description = s.Key.Description,
                    CreationTime = s.Key.CreationTime,
                    Deadline = s.Key.Deadline,
                    InTimelimit = !(s.Key.Deadline < now && s.Key.Status != JobStatus.Done),
                    Comment = s.Key.Comment,
                    Remind = s.Key.Remind,
                    Users = s.Select(k => new GetUserJobDto
                    {
                        UserId = k.UserId,
                        FullName = k.FullName
                    })
                }).FirstOrDefault();
        }
        public async Task<List<GetUserJobDto>> GetReceiverJob()
        {
            var creatorUserId = AbpSession.UserId.Value;
            return await WorkScope.GetAll<UserJob>().Where(s => s.Job.CreatorUserId == creatorUserId).Select(s => new GetUserJobDto
            {
                UserId = s.UserId,
                FullName = s.User.FullName
            }).Distinct().ToListAsync();
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.MyJob.ChangeStatus, PermissionNames.Job.ChangeStatus)]

        public async Task ChangeStatusJob(JobStatusDto input)
        {
            var userId = AbpSession.UserId;
            var isCreatorOrAssignee = await WorkScope.GetAll<UserJob>().Where(s => s.JobId == input.Id).AnyAsync(s => s.UserId == userId || s.Job.CreatorUserId == userId);
            if (!isCreatorOrAssignee)
            {
                throw new UserFriendlyException(string.Format("Job id = {0} not found", input.Id));
            }
            var job = await WorkScope.GetAsync<Job>(input.Id);
            job.Status = input.Status;
            await WorkScope.UpdateAsync<Job>(job);
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.MyJob.ChangeOrder)]

        public async Task ChangeOrderJob(SwapUserJobDto input)
        {
            var userId = AbpSession.UserId.Value;
            var userJobs = await WorkScope.GetAll<UserJob>().Where(s => s.UserId == userId && s.Job.Status != Enums.JobStatus.Done)
                .Select(s => new
                {
                    Id = s.Id,
                    Order = s.Order
                }).OrderByDescending(s => s.Order).ToListAsync();
            var listOrder = new List<OrderMapDto>();
            int index = 0;
            foreach (var u in userJobs)
            {
                listOrder.Add(new OrderMapDto { Index = index, Order = u.Order.Value, UserJobId = u.Id });
                index++;
            }
            var from = listOrder.Where(s => s.UserJobId == input.FromId).Select(s => s.Index).FirstOrDefault();
            var to = listOrder.Where(s => s.UserJobId == input.ToId).Select(s => s.Index).FirstOrDefault();
            if (from < to)
            {
                var orderTo = listOrder[to];
                for (int i = from; i < to; i++)
                {
                    var orderA = listOrder[i];
                    var orderB = listOrder[i + 1];
                    var userJobB = await WorkScope.GetAsync<UserJob>(orderB.UserJobId);
                    userJobB.Order = orderA.Order;
                    await WorkScope.UpdateAsync<UserJob>(userJobB);
                    CurrentUnitOfWork.SaveChanges();
                }
                var userJobFrom = await WorkScope.GetAsync<UserJob>(listOrder[from].UserJobId);
                userJobFrom.Order = orderTo.Order;
                await WorkScope.UpdateAsync<UserJob>(userJobFrom);
            }
            else
            {
                var orderTo = listOrder[to];
                for (int i = from; i > to; i--)
                {
                    var orderA = listOrder[i];
                    var orderB = listOrder[i - 1];
                    var userJobB = await WorkScope.GetAsync<UserJob>(orderB.UserJobId);
                    userJobB.Order = orderA.Order;
                    await WorkScope.UpdateAsync<UserJob>(userJobB);
                    CurrentUnitOfWork.SaveChanges();
                }
                var userJobFrom = await WorkScope.GetAsync<UserJob>(listOrder[from].UserJobId);
                userJobFrom.Order = orderTo.Order;
                await WorkScope.UpdateAsync<UserJob>(userJobFrom);
            }
        }
        //Nhac viec : Nhac vao KOMU, hien thong bao IMS, nhac vao GMAIL
        [HttpPost]
        [AbpAuthorize(PermissionNames.MyJob.Remind)]
        public async Task Remind(RemindJobDto input)
        {
            var userId = AbpSession.UserId;
            var myJob = await WorkScope.GetAll<UserJob>().Where(s => s.UserId == userId && s.Id == input.Id).FirstOrDefaultAsync();
            if (myJob == null)
            {
                throw new UserFriendlyException(string.Format("User Job Id = {0} isn't exist", input.Id));
            }
            var listUserJob = await WorkScope.GetAll<UserJob>().Where(s => s.JobId == myJob.JobId).ToListAsync();
            foreach (var l in listUserJob)
            {
                myJob.Remind = input.Minutes;
            }
            await WorkScope.UpdateRangeAsync(listUserJob);

        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.MyJob.ViewJobInDashboard)]
        public async Task<List<MyJobDto>> ViewJobInDashboard()
        {
            var hasView = bool.Parse(await SettingManager.GetSettingValueAsync(AppSettingNames.HasViewJobInDashboard));
            if (!hasView)
            {
                return null;
            }
            var userId = AbpSession.UserId.Value;
            var now = DateTime.Now;
            var myJobs = await WorkScope.GetAll<UserJob>().Where(s => s.UserId == userId)
                .Where(s => !s.Remind.HasValue || s.Job.Deadline.Value.AddMinutes(-s.Remind.Value) < now)
                .Where(s => s.Job.Deadline < now)
                .Where(s => s.Job.Status == JobStatus.Todo)
                .OrderByDescending(s => s.Order)
                .Select(s => new MyJobDto
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    JobId = s.JobId,
                    Order = s.Order,
                    Remind = s.Remind,
                    Comment = s.Job.Comment,
                    Deadline = s.Job.Deadline,
                    Description = s.Job.Description,
                    Name = s.Job.Name,
                    Status = s.Job.Status,
                    InTimelimit = false,
                    CreationTime = s.Job.CreationTime,
                    ReporterId = s.Job.CreatorUserId.Value
                }).ToListAsync();
            foreach (var m in myJobs)
            {
                var reporter = WorkScope.GetAll<User>().Where(k => k.Id == m.ReporterId).Select(k => k.FullName).FirstOrDefault();
                m.Reporter = reporter;
            }
            return myJobs;
        }
        [HttpPost]
        public async Task<Object> ImportKomuUserNameFromFile([FromForm] FileInputDto input)
        {
            var successList = new List<string>();
            var failedList = new List<string>();
            if (input != null)
            {
                if (Path.GetExtension(input.File.FileName).Equals(".xlsx"))
                {
                    using (var stream = new MemoryStream())
                    {
                        await input.File.CopyToAsync(stream);

                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                            var rowCount = worksheet.Dimension.Rows;
                            for (int row = 2; row <= rowCount; row++)
                            {
                                if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 2].Value != null)
                                {
                                    var emailInput = worksheet.Cells[row, 1].Value.ToString().Trim();
                                    var user = await WorkScope.GetAll<User>().Where(s => s.EmailAddress == emailInput).FirstOrDefaultAsync();
                                    if (user != null)
                                    {
                                        user.KomuUserName = worksheet.Cells[row, 2].Value.ToString().Trim();
                                        await WorkScope.UpdateAsync(user);
                                        successList.Add(emailInput);
                                    }
                                    else
                                    {
                                        failedList.Add(emailInput);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new UserFriendlyException(String.Format("No file upload!"));
                }
            }
            return new { successList, failedList };
        }
    }
}
