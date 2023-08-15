using Erpinfo.ContestFace.Dto;
using Erpinfo.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Microsoft.AspNetCore.Authorization;
using static Erpinfo.Authorization.Roles.StaticRoleNames;
using Erpinfo.Authorization;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Extensions;

namespace Erpinfo.ContestFace
{
    [Authorize]
    public class ContestFaceAppService : ErpinfoAppServiceBase
    {
        public async Task CreateContest(ContestDto input)
        {
            // tao moi contest
            if (input.ContestId == null)
            {
                if(await WorkScope.GetAll<ContestImage>().Where(s=>(s.StartDate>= input.StartDate && input.StartDate<=s.EndDate)|| (s.StartDate >= input.EndDate && input.EndDate <= s.EndDate)).AnyAsync())
                {
                    throw new UserFriendlyException("Đang có xung đột giữa các cuộc thi, vui lòng xem lại");
                }
                var newContest = ObjectMapper.Map<ContestImage>(input);
                await WorkScope.InsertAsync(newContest);
            }
            else
            {
                var OldData = await WorkScope.GetAll<ContestImage>().Where(s => s.Id == input.ContestId).FirstOrDefaultAsync();
                if (await WorkScope.GetAll<ContestImage>().Where(s=>s.Id != OldData.Id).Where(s => (s.StartDate >= input.StartDate && input.StartDate <= s.EndDate) || (s.StartDate >= input.EndDate && input.EndDate <= s.EndDate)).AnyAsync())
                {
                    throw new UserFriendlyException("Đang có xung đột giữa các cuộc thi, vui lòng xem lại");
                }
                ObjectMapper.Map<ContestDto, ContestImage>(input, OldData);
                await WorkScope.UpdateAsync(OldData);
            }
        }
        public async Task DeleteContest(long ContestId)
        {
            var OldData = await WorkScope.GetAll<ContestImage>().Where(s => s.Id == ContestId).FirstOrDefaultAsync();
            if (OldData.StatusOfEventPublish == StatusOfEventPublish.Doing || OldData.StatusOfEventPublish == StatusOfEventPublish.Ended)
            {
                throw new UserFriendlyException("Không thể xóa cuộc thi đang diễn ra hoặc đã kết thúc");
            }
            else
            {
                await WorkScope.DeleteAsync(OldData);
            }
        }
        public async Task<List<ContestDto>> GetAllContest()
        {
            return await WorkScope.GetAll<ContestImage>().OrderByDescending(s => s.StartDate)
                .Select(s=>new ContestDto 
                { StartDate=s.StartDate,
                  StatusOfEventPublish=s.StatusOfEventPublish,
                  ContestId=s.Id,
                  ContestName=s.ContestName,
                  Description=s.Description,
                  EndDate=s.EndDate,
                  MaxImagesPerDay=s.MaxImagesPerDay
                }).ToListAsync();
        }
        public async Task PublishImage(string imageUrl)
        {
            var dataEvent = await WorkScope.GetAll<ContestImage>().Where(s => s.StatusOfEventPublish == StatusOfEventPublish.Doing).FirstOrDefaultAsync();
            if(dataEvent== null)
            {
                throw new UserFriendlyException("Hiện không có cuộc thi nào đang diễn ra");
            }
            if (await WorkScope.GetAll<ImagePublish>().Where(s=>s.CreatorUserId==AbpSession.UserId && s.ContestId == dataEvent.Id).CountAsync() > dataEvent.MaxImagesPerDay)
            {
                throw new UserFriendlyException("Bạn đã đăng vượt quá số ảnh quy định, hãy unpublish ảnh cũ để publish ảnh mới");
            }
            var newData = new ImagePublish
            {
                Status = true,
                ContestId = dataEvent.Id,
                ImageURI = imageUrl,
            };
            await WorkScope.InsertAsync(newData);
        }
        public async Task UnPublishImage(long Id)
        {
            var oldData = await WorkScope.GetAsync<ImagePublish>(Id);
            if (oldData.CreatorUserId == AbpSession.UserId || (oldData.CreatorUserId != AbpSession.UserId && UserHasRole((AbpSession.UserId ?? 0), Host.Admin)))
            {
                oldData.Status = (!oldData.Status);
            }
            else
            {
                throw new UserFriendlyException("Bạn không có quyền thực hiện hành động này");
            }
        }
        public async Task<List<ImagePublish>> GetAllPublishImageOfUser(long ContestId)
        {
            return await WorkScope.GetAll<ImagePublish>().Where(s => s.ContestId == ContestId && s.CreatorUserId == (AbpSession.UserId ?? 0)).ToListAsync();
        }
        public async Task<PagedResultDto<GetAllImageDto>> GetAllImages(StatusOfEventPublish? statusOfEventPublish, string ContestName, int PageSize, int PageNumber)
        {
            var CurrentContest = WorkScope.GetAll<ContestImage>();
            var IdCurrentContest = new ContestImage();
             if (!await PermissionChecker.IsGrantedAsync(PermissionNames.FaceId.ManagerFaceId))
            {
                IdCurrentContest = await  CurrentContest.Where(s => s.StatusOfEventPublish == StatusOfEventPublish.Doing).FirstOrDefaultAsync();
            }
            else
            {
                IdCurrentContest =  CurrentContest.WhereIf(statusOfEventPublish != null, s => s.StatusOfEventPublish == statusOfEventPublish)
                    .WhereIf(!ContestName.IsNullOrWhiteSpace(), s => s.ContestName.Contains(ContestName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    
            }            
            var ImageAndLike = (from s in WorkScope.GetRepo<ImagePublish>().GetAllIncluding(s => s.User).Where(s => s.ContestId == IdCurrentContest.Id)
                                .Select(s => new
                                {
                                    Id = s.Id,
                                    Name = $"{s.User.FullName}{s.User.Name}",
                                    UserId = s.CreatorUserId ?? 0,
                                    ImageUrl = s.ImageURI
                                }).AsEnumerable()
                                join i in WorkScope.GetAll<UserLikePublishImage>()
                                on s.Id equals i.ImageId into temped
                                from k in temped.DefaultIfEmpty()
                                group k by s into temped2
                                select new GetAllImageDto
                                {
                                    
                                    FullName = temped2.Key.Name,
                                    ImageUrl = temped2.Key.ImageUrl,
                                    LikeCounted = temped2.LongCount(t => t?.Id != null),
                                    UserId = temped2.Key.UserId

                                }).AsEnumerable();




            return new PagedResultDto<GetAllImageDto>
            {
                Items = ImageAndLike.Take(PageSize).Skip(PageSize * (PageNumber - 1)).ToList(),
            TotalCount = ImageAndLike.Count()
            };

        }
        public async Task<List<GetAllImageDto>> TopLikeImages(int top)
        {
            var CurrentContest = await WorkScope.GetAll<ContestImage>().Where(s => s.StatusOfEventPublish == StatusOfEventPublish.Doing).FirstOrDefaultAsync();
            return ((from s in WorkScope.GetRepo<ImagePublish>().GetAllIncluding(s => s.User).Where(s => s.ContestId == CurrentContest.Id)
                                .Select(s => new
                                {
                                    Id = s.Id,
                                    Name = $"{s.User.FullName}{s.User.Name}",
                                    UserId = s.CreatorUserId ?? 0,
                                    ImageUrl = s.ImageURI
                                }).AsEnumerable()
                     join i in WorkScope.GetAll<UserLikePublishImage>()
                     on s.Id equals i.ImageId into temped
                     from k in temped.DefaultIfEmpty()
                     group k by s into temped2
                     select new GetAllImageDto
                     {

                         FullName = temped2.Key.Name,
                         ImageUrl = temped2.Key.ImageUrl,
                         LikeCounted = temped2.LongCount(t => t?.Id != null),
                         UserId = temped2.Key.UserId

                     }).OrderByDescending(s => s.LikeCounted).Take(top).ToList());

        }
        public async Task LikeImage(long ImageId)
        {
            var old = await WorkScope.GetAll<UserLikePublishImage>()
                .FirstOrDefaultAsync(s => s.ImageId == ImageId && (s.CreatorUserId ?? s.LastModifierUserId) == AbpSession.UserId.Value);
            if (old != null)
            {
                old.IsLiked = old.IsLiked == true ? false : true;
                await WorkScope.UpdateAsync<UserLikePublishImage>(old);
            }
            else
            {
                var userLike = await WorkScope.InsertAsync(new UserLikePublishImage
                {
                    ImageId = ImageId,
                    IsLiked = true,
                    CreatorUserId = AbpSession.UserId.Value
                });
                /*var Images = await WorkScope.GetAsync<MainComment>(id);
                var userLikeCm = await WorkScope.GetAsync<User>(userLike.CreatorUserId.Value);
                var notification = new SystemNotification
                {
                    IsRead = false,
                    Type = NotificationType.ReactComment,
                    Messsage = userLikeCm.FullName + " đã yêu thích bình luận của bạn",
                    EntityId = comment.EntityId,
                    EntityName = comment.EntityName,
                    UserId = comment.CreatorUserId.Value,
                    CommentId = id,
                    AuthorAvatar = userLikeCm.Avatar
                };
                await WorkScope.InsertAsync(notification);*/
            }
        }
    }
}
