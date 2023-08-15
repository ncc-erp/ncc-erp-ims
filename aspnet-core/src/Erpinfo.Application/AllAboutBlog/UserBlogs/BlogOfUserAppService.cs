using Erpinfo.AllAboutBlog.UserBlogs.Dto;
using Erpinfo.Entities;
using Erpinfo.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Erpinfo.DomainServices;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Erpinfo.Authorization.Users;
using Abp.Collections.Extensions;
using Erpinfo.AllAbouBlog.BlogFeed.Dto;
using Erpinfo.Constant;
using Abp.Extensions;

namespace Erpinfo.AllAboutBlog.UserBlogs
{
    [Abp.Authorization.AbpAuthorize]
    public class BlogOfUserAppService : ErpinfoAppServiceBase
    {
        private readonly UploadImageService imageService;
        public BlogOfUserAppService(UploadImageService imageService)
        {
            this.imageService = imageService;
        }

        public async Task UpdateInfo([FromForm] UpdateUserBlog input)
        {
            if (input.UserId != AbpSession.UserId)
            {
                throw new UserFriendlyException("Bạn không phải chủ blog này");
            }
            if (!await WorkScope.GetAll<UserBlog>().AnyAsync(s => s.CreatorUserId == AbpSession.UserId))
            {
                var mapped = ObjectMapper.Map<UserBlog>(input);
                mapped.BackgroundImages = await imageService.UploadImage(input.BackgroundImages);
                var linkAnh = await imageService.UploadImage(input.Avatar);
                mapped.AvatarUrl = linkAnh;
                await WorkScope.InsertAsync(mapped);
                var User = await WorkScope.GetAsync<User>(input.UserId);
                User.Avatar = $"{EntityConstant.RootUrl}{linkAnh}";
                await WorkScope.UpdateAsync(User);
            }
            else
            {
                var oldData = await WorkScope.GetAll<UserBlog>().Where(s => s.CreatorUserId == AbpSession.UserId).FirstOrDefaultAsync();
                ObjectMapper.Map<UpdateUserBlog, UserBlog>(input, oldData);
                if (input.Avatar != null)
                {
                    var linkAnh = await imageService.UploadImage(input.Avatar);
                    oldData.AvatarUrl = linkAnh;
                    var User = await WorkScope.GetAsync<User>(input.UserId);
                    //  User.Avatar = $"{EntityConstant.RootUrl}{linkAnh}";
                    User.Avatar = linkAnh;
                    await WorkScope.UpdateAsync(User);
                }
                if (input.BackgroundImages != null)
                {
                    oldData.BackgroundImages = await imageService.UploadImage(input.BackgroundImages);

                }
                await WorkScope.UpdateAsync(oldData);
            }
        }
        public async Task<DetailBlog> DetailBlog(long UserId)
        {

            var Comment = WorkScope.GetAll<MainComment>().Include(s => s.SubComments).Where(s => s.EntityName == EntityConstant.BlogEntity).AsEnumerable().GroupBy(s => s.EntityId)
                   .Select(s => new CommentCount
                   {
                       Id = s.First().Id,
                       EntityName = "Blog",
                       EntityId = s.Key,
                       Commentcount = s.Select(s => s.Id).LongCount(),
                       SubCommentCount = s.Select(s => s.SubComments.LongCount()).FirstOrDefault(),
                   }).ToList();

            var query1 = (from s in WorkScope.GetRepo<BlogPost>().GetAllIncluding(s => s.User, k => k.Reaction)
                         .Where(s => s.CreatorUserId == UserId).AsEnumerable()
                          join u in Comment
                          on s.Id equals u.EntityId into temped
                          from i in temped.DefaultIfEmpty()
                          select new BlogFeedDto
                          {

                              TotalView = s.CountView,
                              TotalComment = i == null ? 0 : (i.SubCommentCount + i.Commentcount),
                              TotalReaction = s.Reaction != null ? s.Reaction.LongCount() : 0,
                          }).ToList();
            var query = await WorkScope.GetRepo<UserBlog>().GetAllIncluding(s => s.User).Where(s => s.CreatorUserId == UserId)

                .Select(s => new DetailBlog
                {
                    AvatarUrl = s.AvatarUrl.IsNullOrWhiteSpace() ? s.User.Avatar : s.AvatarUrl,
                    BackgroundImages = s.BackgroundImages,
                    Description = s.Description,
                    FullName = $"{s.User.Surname} {s.User.Name}",
                    MusicBackground = s.MusicBackground,
                    UserId = s.CreatorUserId ?? 0,
                    NickName = s.NickName,
                }).FirstOrDefaultAsync();
            if (query != null)
            {
                return new DetailBlog
                {
                    TotalComment = query1.Sum(s => s.TotalComment),
                    AvatarUrl = query.AvatarUrl,
                    BackgroundImages = query.BackgroundImages,
                    Description = query.Description,
                    FullName = query.FullName,
                    MusicBackground = query.MusicBackground,
                    TotalReaction = query1.Sum(s => s.TotalReaction),
                    TotalView = query1.Sum(s => s.TotalView),
                    UserId = query.UserId,
                    NickName = query.NickName
                };
            }
            else
            {
                return null;
            }
        }
        public async Task<PagedResultDto<ReturnOfFind>> FindBlogByNameOrUserId(FindByUserNameOrUserIDDto input)
        {
            return await Task.Run(() =>
            {
                long UserId = 0;
                try
                {
                    UserId = long.Parse(input.NameOrUserID);
                }
                catch (Exception ex)
                {

                }
                var Query = from s in WorkScope.GetAll<User>()
                    .WhereIf(UserId != 0, s => s.Id == UserId)
                    .WhereIf(UserId == 0, s => $"{s.Surname} {s.Name}".Contains(input.NameOrUserID, StringComparison.OrdinalIgnoreCase))
                            join l in WorkScope.GetAll<UserBlog>()
                                    on s.CreatorUserId equals l.CreatorUserId into temped1
                            from ll in temped1.DefaultIfEmpty()
                            select new ReturnOfFind
                            {
                                Avatar = ll == null ? s.Avatar : (ll.AvatarUrl.IsNullOrWhiteSpace() ? s.Avatar : ll.AvatarUrl),
                                FullName = $"{s.Surname} {s.Name}",
                                UserId = s.Id
                            };
                var TotalCount = Query.Count();
                var result = Query.Skip((input.PageNumber - 1) * input.PageSize).Take(input.PageSize).ToList();
                return new PagedResultDto<ReturnOfFind>
                {
                    Items = result,
                    TotalCount = TotalCount
                };
            });
        }
    }
}
