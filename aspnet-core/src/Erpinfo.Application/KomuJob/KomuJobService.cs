using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.UI;
using Erpinfo.Authorization.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erpinfo.Extension;
using Microsoft.AspNetCore.Http;
using Erpinfo.Uitls;
using Erpinfo.KomuJob.Dto;
using Erpinfo.Entities;
using Erpinfo.Enums;
using Erpinfo.Configuration;
using Abp.BackgroundJobs;
using Erpinfo.NotificationKomuBackgroundJob;
using Erpinfo.Authorization.Roles;
using Abp.Authorization.Users;
using Erpinfo.Authorization;
using Erpinfo.Constant;

namespace Erpinfo.KomuJob
{
    public class KomuJobService : ErpinfoAppServiceBase, IKomuJobService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager _roleManager;
        public KomuJobService(IHttpContextAccessor httpContextAccessor, 
            RoleManager roleManager) : base()
        {
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
        }

        [AbpAllowAnonymous]
        [HttpPost]
        public async Task<object> CreateJob(CreateJobDTO createJobDto)
        {
            if (!CheckSecurityCode())
                throw new UserFriendlyException("SecretCode does not match!");
            var dateNow = DateTime.Now;
            if (createJobDto == null) throw new UserFriendlyException("Please provide enough information!");
            if (string.IsNullOrEmpty(createJobDto.CreatorEmail)) throw new UserFriendlyException("Please provide CreatorEmail information!");
            if (string.IsNullOrEmpty(createJobDto.RecipientEmail)) throw new UserFriendlyException("Please provide RecipientEmail information!");
            if (string.IsNullOrEmpty(createJobDto.JobName)) throw new UserFriendlyException("Please provide JobName information!");
            var qUsers = WorkScope.GetAll<User>().Include(x => x.Roles);
            var creatorUser = await qUsers.FirstOrDefaultAsync(u => u.EmailAddress == createJobDto.CreatorEmail);
            var recipientUser = await qUsers.FirstOrDefaultAsync(u => u.EmailAddress == createJobDto.RecipientEmail);
            if (creatorUser == null) throw new UserFriendlyException("CreatorEmail does not exist in the system!");
            if (creatorUser.Roles == null || creatorUser.Roles.Count == 0) throw new UserFriendlyException("Permission Denied!");
            var permissions = await GetPermissions(creatorUser.Roles);
            if (!permissions.Any(x => x == PermissionNames.Job.Create))
                throw new UserFriendlyException("Permission Denied!");
            var job = new Job() { 
                Name = createJobDto.JobName,
                CreatorUserId = creatorUser.Id,
                Deadline = createJobDto.Deadline,
                CreationTime = DateTimeUtils.GetNow(),
                Status = JobStatus.Todo,
                IsDeleted = false
            };
            job.Id = await WorkScope.InsertAndGetIdAsync<Job>(job);
            var userJob = new UserJob()
            {
                JobId = job.Id,
                UserId = recipientUser.Id,
                CreatorUserId = creatorUser.Id,
                CreationTime = DateTimeUtils.GetNow(),
                NumberReminded = 0,
                IsDeleted = false
            };
            long userJobId = await WorkScope.InsertAndGetIdAsync<UserJob>(userJob);

            return new
            {
                JobId = job.Id,
                CreationTime = job.CreationTime,
                CreatorEmail = creatorUser != null ? creatorUser.EmailAddress : string.Empty,
                CreatorName = creatorUser != null ? creatorUser.Surname + " " + creatorUser.Name : string.Empty,
                CreatorUsername = creatorUser != null ? (GetUserName(creatorUser.EmailAddress) ?? creatorUser.UserName) : string.Empty,
                Status = Enum.GetName(typeof(JobStatus), job.Status),
                JobName = job.Name,
                Message = "Success"
            };
        }

        [AbpAllowAnonymous]
        [HttpPost]
        public async Task<object> UpdateJobStatus(UpdateJobStatusDto updateJobStatusDto)
        {
            if (!CheckSecurityCode())
                throw new UserFriendlyException("SecretCode does not match!");
            if (updateJobStatusDto == null) throw new UserFriendlyException("Please provide enough information!");
            if (string.IsNullOrEmpty(updateJobStatusDto.ModifierEmail)) throw new UserFriendlyException("Please provide ModifierEmail information!");
            if (!updateJobStatusDto.JobId.HasValue) throw new UserFriendlyException("Please provide JobId information!");
            var qUsers = WorkScope.GetAll<User>().Include(x => x.Roles);
            var modifierUser = await qUsers.FirstOrDefaultAsync(x => x.EmailAddress == updateJobStatusDto.ModifierEmail);
            if (modifierUser == null) throw new UserFriendlyException("ModifierEmail does not exist in the system!");
            if (modifierUser.Roles == null || modifierUser.Roles.Count == 0) throw new UserFriendlyException("Permission Denied!");
            var permissions = await GetPermissions(modifierUser.Roles);
            if (!permissions.Any(x => x == PermissionNames.Job.Edit || x == PermissionNames.Job.ChangeStatus))
                throw new UserFriendlyException("Permission Denied!");
            var job = await WorkScope.GetAll<Job>().FirstOrDefaultAsync(s => s.Id == updateJobStatusDto.JobId);
            var createdUser = await qUsers.FirstOrDefaultAsync(x => x.Id == job.CreatorUserId);
            if (job.Status != JobStatus.Done)
            {
                job.LastModificationTime = DateTimeUtils.GetNow();
                job.LastModifierUserId = modifierUser.Id;
                job.Status = JobStatus.Done;
                if (job == null) return null;
                job = await WorkScope.UpdateAsync(job);
                return new
                {
                    JobId = job.Id,
                    CreationTime = job.CreationTime,
                    CreatorEmail = createdUser != null ? createdUser.EmailAddress : string.Empty,
                    CreatorName = createdUser != null ? createdUser.Surname + " " + createdUser.Name : string.Empty,
                    CreatorUsername = createdUser != null ? (GetUserName(createdUser.EmailAddress) ?? createdUser.UserName) : string.Empty,
                    ModifierName = modifierUser.Surname + " " + modifierUser.Name,
                    ModifierUsername = GetUserName(modifierUser.EmailAddress) ?? modifierUser.UserName,
                    ModifierEmail = modifierUser.EmailAddress,
                    ModificationDate = job.LastModificationTime,
                    Status = Enum.GetName(typeof(JobStatus), job.Status),
                    JobName = job.Name,
                    Message = "Success"
                };
            }
            else
            {
                return new { Message = "The job is already in the status of 'Done'!" };
            }
        }

        [AbpAllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<JobDTO>> GetJobs(string email)
        {
            if (!CheckSecurityCode())
                    throw new UserFriendlyException("SecretCode does not match!");
            if (string.IsNullOrEmpty(email)) return null;
            var qusers = WorkScope.GetAll<User>().Include(x => x.Roles);
            var user = await qusers.FirstOrDefaultAsync(u => u.EmailAddress == email);
            if (user == null
                || user.Roles == null
                || user.Roles.Count == 0) 
                return null;
            var qUserJobs = WorkScope.GetAll<UserJob>().Where(s => s.UserId == user.Id && s.Job.Status != JobStatus.Done);
            var query = from uj in qUserJobs
                       join u in qusers on uj.CreatorUserId equals u.Id
                       let modifier = qusers.FirstOrDefault(x => x.Id == uj.Job.LastModifierUserId)
                       select new
                       {
                           JobId = uj.JobId,
                           CreationTime = uj.CreationTime,
                           CreatorEmail = u.EmailAddress,
                           CreatorName = u.Surname + " " + u.Name,
                           CreatorUsername = u.UserName,
                           ModifierName = modifier != null ? modifier.Surname + " " + modifier.Name : string.Empty,
                           ModifierEmail = modifier != null ? modifier.EmailAddress : string.Empty,
                           ModifierUsername = modifier != null ? modifier.UserName : string.Empty,
                           ModificationDate = uj.Job.LastModificationTime,
                           Status = uj.Job.Status,
                           JobName = uj.Job.Name
                       };
            var jobs = await query.OrderByDescending(x => x.CreationTime).ToListAsync();
            return jobs.Select(x => new JobDTO()
            {
                JobId = x.JobId,
                CreationTime = x.CreationTime,
                CreatorEmail = x.CreatorEmail,
                CreatorName = x.CreatorName,
                CreatorUsername = GetUserName(x.CreatorEmail) ?? x.CreatorUsername,
                ModifierName = x.ModifierName,
                ModifierUsername = GetUserName(x.ModifierEmail) ?? x.ModifierUsername,
                ModifierEmail = x.ModifierEmail,
                ModificationDate = x.ModificationDate,
                Status = Enum.GetName(typeof(JobStatus), x.Status),
                JobName = x.JobName,
            });
        }

        #region API HELPER
        private bool CheckSecurityCode()
        {
            var securityCode = EntityConstant.SecurityCode;
            var header = _httpContextAccessor.HttpContext.Request.Headers;
            var securityCodeHeader = header["X-Secret-Key"];
            if (securityCode == securityCodeHeader)
                return true;
            return false;
        }

        private async Task<List<string>> GetPermissions(ICollection<UserRole> roles)
        {
            var permissions = new List<string>();
            foreach (var role in roles)
            {
                var grantedPermissions = await _roleManager.GetGrantedPermissionsAsync(role.RoleId);
                permissions.AddRange(grantedPermissions.Select(p => p.Name).ToList());
            }
            return permissions;
        }

        private string GetUserName(string emailAddress)
        {
            if (!string.IsNullOrEmpty(emailAddress))
            {
                var gmailFormat = "@ncc.asia";
                var userName = emailAddress.Contains(gmailFormat) ? emailAddress.Replace(gmailFormat, "") : null;
                return userName;
            }
            return null;

        }
        #endregion
    }
}
