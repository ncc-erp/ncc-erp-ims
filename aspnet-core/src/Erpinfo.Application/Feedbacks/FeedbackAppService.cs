using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.UI;
using Erpinfo.Authorization;
using Erpinfo.Entities;
using Erpinfo.Enums;
using Erpinfo.Feedbacks.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erpinfo.Feedbacks
{
    public class FeedbackAppService : ErpinfoAppServiceBase
    {
        [HttpGet]
        [AbpAuthorize(PermissionNames.Feedback.GetAllFeedbackAdmin)]
        public async Task<List<GetFeedbackDto>> GetAllFeedbackAdmin(List<FeedbackType> type)
        {

            return await WorkScope.GetAll<Feedback>().Where(x => type.Contains(x.Type))
                        .OrderByDescending(x => x.CreationTime)
                        .Select(x => new GetFeedbackDto
                        {
                            Id = x.Id,
                            FeedBack = x.FeedBack,
                            Status = x.Status,
                            Type = x.Type
                        }).ToListAsync();
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.Feedback.GetAllFeedbackUser)]
        public async Task<List<GetFeedbackDto>> GetAllFeedbackUser(FeedbackType type)
        {
            return await WorkScope.GetAll<Feedback>().Where(x => x.Status == FeedbackStatus.Approved || x.Status == FeedbackStatus.Used)
                        .Where(x => x.Type == type)
                        .OrderByDescending(x => x.CreationTime)
                        .Select(x => new GetFeedbackDto
                        {
                            Id = x.Id,
                            FeedBack = x.FeedBack,
                            Status = x.Status,
                            Type = x.Type
                        }).ToListAsync();
        }

        [HttpPost]
        public async Task<FeedbackDto> SaveFeedback(FeedbackDto Input)
        {
            var span = DateTime.Now.Subtract(new DateTime(2021, 4, 1, 0, 0, 0, DateTimeKind.Utc));

            Input.ReferenceCode = (long)span.TotalSeconds;
            Input.Code = span.TotalSeconds;
            Input.Status = FeedbackStatus.Default;

            var feedback = ObjectMapper.Map<Feedback>(Input);
            Input.Id = await WorkScope.InsertAndGetIdAsync(feedback);

            return Input;
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Feedback.Approve)]
        public async Task Approve(long feedbackId)
        {
            var feedback = await WorkScope.GetAsync<Feedback>(feedbackId);

            if(feedback != null)
            {
                feedback.Status = FeedbackStatus.Approved;
                await WorkScope.GetRepo<Feedback, long>().UpdateAsync(feedback);
            } else
            {
                throw new UserFriendlyException(string.Format("Feedback is not exist"));
            }
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Feedback.Reject)]
        public async Task Reject(long feedbackId)
        {
            var feedback = await WorkScope.GetAsync<Feedback>(feedbackId);

            if (feedback != null)
            {
                feedback.Status = FeedbackStatus.Reject;
                await WorkScope.GetRepo<Feedback, long>().UpdateAsync(feedback);
            }
            else
            {
                throw new UserFriendlyException(string.Format("Feedback is not exist"));
            }
        }
    }
}
