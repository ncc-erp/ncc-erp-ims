using Erpinfo.AllAbouBlog.BlogFeed.Dto;
using Erpinfo.Entities;
using Erpinfo.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Erpinfo.Constant;
using Abp.UI;
using Erpinfo.DomainServices;
using Erpinfo.Paging;
using Abp.Collections.Extensions;
using Abp.Authorization;
using Abp.Extensions;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Erpinfo.Helper;
using System.Text.RegularExpressions;
using Erpinfo.AllAboutBlog.BlogFeed.Dto;
using Erpinfo.Enums;
using Erpinfo.Authorization.Users;

namespace Erpinfo.AllAbouBlog.BlogFeed
{
    [AbpAuthorize]
    public class BlogFeedAppService : ErpinfoAppServiceBase
    {
        private IUserService userService;

        public BlogFeedAppService(IUserService userService)
        {
            this.userService = userService;
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<PagedResultDto<BlogFeedDto>> GetAllBlogFeed(BlogFeedInputDto input)
        {
            return await Task.Run(() =>
           {
               if (input.IsInBlogFeed == KindOfView.Visiting && input.UserId == null)
               {
                   throw new UserFriendlyException("Bị thiếu userId");
               }
               var Comment = WorkScope.GetAll<MainComment>().Include(s => s.SubComments).Where(s => s.EntityName == EntityConstant.BlogEntity).AsEnumerable().GroupBy(s => s.EntityId)
                   .Select(s => new CommentCount
                   {
                       Id = s.First().Id,
                       EntityName = "Blog",
                       EntityId = s.Key,
                       Commentcount = s.Select(s => s.Id).LongCount(),
                       SubCommentCount = s.Select(s => s.SubComments.LongCount()).FirstOrDefault(),
                   });

               var query = (from s in WorkScope.GetRepo<BlogPost>().GetAllIncluding(s => s.User, k => k.Reaction)
                            .WhereIf(input.IsInBlogFeed == KindOfView.SeeYourSelf, s => s.CreatorUserId == AbpSession.UserId)
                            .WhereIf(input.IsInBlogFeed == KindOfView.Visiting, s => s.CreatorUserId == input.UserId)
                            .Where(s => s.Status == StatusOfBlog.Public)
                            .OrderByDescending(s => s.CreationTime).AsEnumerable()
                            join l in WorkScope.GetAll<UserBlog>()
                            on s.CreatorUserId equals l.CreatorUserId into temped1
                            from ll in temped1.DefaultIfEmpty()
                            join u in Comment
                            on s.Id equals u.EntityId into temped
                            from i in temped.DefaultIfEmpty()
                            select new BlogFeedDto
                            {
                                Status = s.Status,
                                Detail = s.Detail,
                                FullName = $"{s.User.Surname} {s.User.Name}",
                                PostId = s.Id,
                                Tilte = s.Title,
                                TimeCreated = s.CreationTime,
                                UrlAvatar = ll == null ? s.User.Avatar : (ll.AvatarUrl.IsNullOrWhiteSpace() ? s.User.Avatar : ll.AvatarUrl),
                                UserId = s.User.Id,
                                TotalView = s.CountView,
                                TotalComment = ((i?.SubCommentCount ?? 0) + (i?.Commentcount ?? 0)),
                                Reaction = s.Reaction.Where(s => s.UserID == (AbpSession.UserId ?? 0)).Select(s => s.Reaction).FirstOrDefault(),
                                TotalReaction = s.Reaction != null ? s.Reaction.LongCount() : 0,
                            }).AsQueryable();
               var Result = query.OrderByDescending(s => s.TimeCreated).Skip((input.PageNumber - 1) * input.PageSize).Take(input.PageSize).ToList();
               var ListPostId = Result.Select(s => s.PostId).ToList();
               /*var UpdateViewCount = WorkScope.GetAll<BlogPost>().Where(s => ListPostId.Contains(s.Id));
               foreach (var i in UpdateViewCount)
               {
                   i.CountView = i.CountView + 1;
               }
               WorkScope.UpdateRange(UpdateViewCount);*/
               var TotalCount = query.Count();
               return new PagedResultDto<BlogFeedDto>
               {
                   Items = Result,
                   TotalCount = TotalCount
               };
           });
        }
        public async Task CreateBlogPost(CreateBlogPostDto input)
        {
            if (input.Status == 0)
            {
                throw new UserFriendlyException("Chưa chọn trạng thái bài viết");
            }
            var mapped = ObjectMapper.Map<BlogPost>(input);
            var StripContent = Helpers.StripHTMLContent(input.Detail);
            var AllHashtag = AllRegex.HashtagRegex.Matches(StripContent);
            var ToListString = AllHashtag.Select(s => s.ToString().Replace(" ", "")).Distinct().ToList();
            var ListIdInserted = new List<long>();
            var ListNewHashTag = new List<string>();
            foreach (var i in ToListString)
            {
                if (!WorkScope.GetAll<HashTag>().Any(s => s.Hashtag == i))
                {
                    var a = new HashTag
                    {
                        Hashtag = i,
                    };
                    var id = await WorkScope.InsertAndGetIdAsync(a);
                    ListIdInserted.Add(id);
                    ListNewHashTag.Add(i);
                }
            };
            var ListHashtagCoSan = ToListString.Except(ListNewHashTag);
            var DanhSachIdHashTagCoSan = await WorkScope.GetAll<HashTag>().Where(s => ListHashtagCoSan.Contains(s.Hashtag)).Select(s => s.Id).ToListAsync();

            var IdPost = await WorkScope.InsertAndGetIdAsync(mapped);
            var ListHashtagPost = new List<PostHashtag>();
            foreach (var i in (DanhSachIdHashTagCoSan.Concat(ListIdInserted)))
            {
                var PostHashTag = new PostHashtag
                {
                    IdHashTag = i,
                    IdPost = IdPost
                };
                ListHashtagPost.Add(PostHashTag);
            }
            await WorkScope.InsertRangeAsync(ListHashtagPost);
        }
        public async Task EditBlogPost(EditBlogPostDto input)
        {
            if (input.Status == 0)
            {
                throw new UserFriendlyException("Chưa chọn trạng thái bài viết");
            }
            //loc input tu html ra cac string
            var StripContent = Helpers.StripHTMLContent(input.Detail);
            //loc tu string ben tren ra cac hashtag
            var AllInputHashtag = AllRegex.HashtagRegex.Matches(StripContent);
            //loc hashtag ra List string rồi loai bo khoang trang du thua
            var AllInputHashTagString = AllInputHashtag.Select(s => s.ToString().Replace(" ", "")).Distinct().ToList();
            var oldData = WorkScope.Get<BlogPost>(input.Id);

            if (oldData.CreatorUserId != AbpSession.UserId)
            {
                throw new UserFriendlyException("Bạn không phải là người tạo ra bài viết này");
            }
            var AllOldHashtag = AllRegex.HashtagRegex.Matches(oldData.Detail);
            var AllOutputHashTagString = AllOldHashtag.Select(s => s.ToString()).Distinct().ToList();
            //lôi ra hashtag cũ mới
            var HashTagDelete = AllOutputHashTagString.Except(AllInputHashTagString);
            var HashTagInput = AllInputHashTagString.Except(AllOutputHashTagString);
            //add vào list để sau add vào bảng trung gian, chỉ cần duyệt những hashtag được thêm vào
            var ListIdInserted = new List<long>();
            var ListNewHashTag = new List<string>();
            foreach (var i in HashTagInput)
            {
                if (!WorkScope.GetAll<HashTag>().Any(s => s.Hashtag == i))
                {
                    var a = new HashTag
                    {
                        Hashtag = i,
                    };
                    var id = await WorkScope.InsertAndGetIdAsync(a);
                    ListIdInserted.Add(id);
                    ListNewHashTag.Add(i);
                }
            };
            var ListHashtagCoSan = HashTagInput.Except(ListNewHashTag);
            var DanhSachIdHashTagCoSan = await WorkScope.GetAll<HashTag>().Where(s => ListHashtagCoSan.Contains(s.Hashtag)).Select(s => s.Id).ToListAsync();
            var ListHashtagPost = new List<PostHashtag>();
            foreach (var i in (DanhSachIdHashTagCoSan.Concat(ListIdInserted)))
            {
                var PostHashTag = new PostHashtag
                {
                    IdHashTag = i,
                    IdPost = oldData.Id
                };
                ListHashtagPost.Add(PostHashTag);
            }
            await WorkScope.InsertRangeAsync(ListHashtagPost);
            //end of new hashtag
            //begin delete hashtag
            var deletedHashtagId = WorkScope.GetAll<HashTag>().Where(s => HashTagDelete.Contains(s.Hashtag)).Select(s => s.Id);
            var deleteHashtagPost = WorkScope.GetAll<PostHashtag>().Where(s => s.IdPost == oldData.Id && deletedHashtagId.Contains(s.IdHashTag));
            foreach (var i in deleteHashtagPost)
            {
                await WorkScope.DeleteAsync(i);
            }
            ObjectMapper.Map<EditBlogPostDto, BlogPost>(input, oldData);

            await WorkScope.UpdateAsync(oldData);
        }
        public async Task DeleteBlogPost(long Id)
        {
            var DeleteData = await WorkScope.GetAsync<BlogPost>(Id);

            if (userService.UserHasRole(AbpSession.UserId ?? 0, "Admin") || AbpSession.UserId == DeleteData.CreatorUserId)
            {
                var deleteHashTagPost = WorkScope.GetAll<PostHashtag>().Where(s => s.IdPost == Id);
                foreach (var i in deleteHashTagPost)
                {
                    await WorkScope.DeleteAsync(i);
                }
                await WorkScope.DeleteAsync(DeleteData);
            }
            else
            {
                throw new UserFriendlyException("Bạn không có quyền xóa bài viết này");
            }
        }
        public async Task<DetailAPostOutPut> DetailAPost(long PostId)
        {
            var Post = await WorkScope.GetAll<BlogPost>().Where(s => s.Id == PostId).FirstOrDefaultAsync();
            Post.CountView += 1;
            await WorkScope.UpdateAsync(Post);
            return await (from s in WorkScope.GetRepo<BlogPost>().GetAllIncluding(s => s.User, k => k.Reaction)
                         .Where(s => s.Status == StatusOfBlog.Public && s.Id == PostId)
                         .OrderByDescending(s => s.CreationTime)
                          join l in WorkScope.GetAll<UserBlog>()
                             on s.CreatorUserId equals l.CreatorUserId into temped1
                          from ll in temped1.DefaultIfEmpty()
                          select new DetailAPostOutPut
                          {
                              Status = s.Status,
                              Detail = s.Detail,
                              FullName = $"{s.User.Surname} {s.User.Name}",
                              PostId = s.Id,
                              Tilte = s.Title,
                              TimeCreated = s.CreationTime,
                              UrlAvatar = ll == null ? s.User.Avatar : (ll.AvatarUrl.IsNullOrWhiteSpace() ? s.User.Avatar : ll.AvatarUrl),
                              UserId = s.User.Id,
                              TotalView = s.CountView,
                              TotalReaction = s.Reaction != null ? s.Reaction.LongCount() : 0,
                              Reaction = s.Reaction.Where(s => s.UserID == (AbpSession.UserId ?? 0)).Select(s => s.Reaction).FirstOrDefault()

                          }).FirstOrDefaultAsync();
        }
        /* public async Task<GridResult<UserAndReaction>> ShowReaction(ShowUserReactionInputDto input)
         {
             var Reaction = await WorkScope.GetAll<UserLikeOrDislikeBlogPost>().Where(s => s.PostId == input.PostId).ToListAsync();
             foreach (var i in Reaction)
             {
                 var
             }
         }*/
        [HttpGet]
        public async Task<PostAndReaction> ReactionPost(Reaction reaction, long PostId)
        {

            var Reaction = await WorkScope.GetAll<UserLikeOrDislikeBlogPost>().Where(s => s.PostId == PostId && s.UserID == AbpSession.UserId).FirstOrDefaultAsync();
            var TotalReact = await WorkScope.GetAll<UserLikeOrDislikeBlogPost>().Where(s => s.PostId == PostId).LongCountAsync();
            //Check xem no da react phat nao chua

            if (Reaction == null)
            {
                var addReation = new UserLikeOrDislikeBlogPost
                {
                    PostId = PostId,
                    Reaction = reaction,
                    UserID = AbpSession.UserId ?? 0,
                };
                await WorkScope.InsertAsync(addReation);
                var userInfo = await WorkScope.GetAll<UserBlog>().Where(s => s.CreatorUserId == AbpSession.UserId).AnyAsync() ?
                    await WorkScope.GetRepo<UserBlog>().GetAllIncluding(s => s.User).Where(s => s.CreatorUserId == AbpSession.UserId).Select(s => new { fullName = $"{s.User.Surname} {s.User.Name}", avatar = s.AvatarUrl }).FirstOrDefaultAsync() :
                    await WorkScope.GetAll<User>().Where(s => s.Id == (AbpSession.UserId ?? 0)).Select(s => new { fullName = $"{s.Surname} {s.Name}", avatar = s.Avatar }).FirstOrDefaultAsync();
                var tagName = new SystemNotification
                {
                    IsRead = false,
                    Type = NotificationType.BlogPost,
                    Messsage = userInfo.fullName + " đã bày tỏ cảm xúc với bài viết của bạn của bạn",
                    EntityId = PostId,
                    EntityName = EntityConstant.BlogEntity,
                    UserId = AbpSession.UserId ?? 0,
                    AuthorAvatar = userInfo.avatar
                };
                await WorkScope.InsertAsync(tagName);
                return new PostAndReaction { PostId = PostId, Reaction = reaction, TotalReact = (TotalReact + 1) };
            }
            else
            {
                if (Reaction.Reaction == reaction)
                {
                    await WorkScope.DeleteAsync(Reaction);
                    return new PostAndReaction { PostId = PostId, Reaction = null, TotalReact = (TotalReact - 1) };
                }
                else
                {
                    await WorkScope.DeleteAsync(Reaction);
                    var addReation = new UserLikeOrDislikeBlogPost
                    {
                        PostId = PostId,
                        Reaction = reaction,
                        UserID = AbpSession.UserId ?? 0,
                    };
                    await WorkScope.InsertAsync(addReation);
                    return new PostAndReaction { PostId = PostId, Reaction = reaction, TotalReact = TotalReact };

                }
            }

        }
        [HttpGet]
        public async Task<PagedResultDto<BlogFeedDto>> FindHashtag(string hashtag, int pageSize, int pageNumber)
        {
            /*var IdHashTag = await WorkScope.GetAll<HashTag>().Where(s => s.Hashtag == hashtag).FirstOrDefaultAsync();*/
            var danhSachBaiVietCoHashtag = await WorkScope.GetRepo<PostHashtag>().GetAllIncluding(s => s.HashTags)
                .Where(s => s.HashTags.Hashtag == hashtag)
                .Select(s => s.IdPost).ToListAsync();
            var Comment = WorkScope.GetAll<MainComment>().Include(s => s.SubComments).Where(s => s.EntityName == EntityConstant.BlogEntity).AsEnumerable().GroupBy(s => s.EntityId)
                 .Select(s => new CommentCount
                 {
                     Id = s.First().Id,
                     EntityName = EntityConstant.BlogEntity,
                     EntityId = s.Key,
                     Commentcount = s.Select(s => s.Id).LongCount(),
                     SubCommentCount = s.Select(s => s.SubComments.LongCount()).FirstOrDefault(),
                 });
            var query = (from s in WorkScope.GetRepo<BlogPost>().GetAllIncluding(s => s.User, k => k.Reaction)

                            .Where(s => (s.Status == StatusOfBlog.Public && s.CreatorUserId != AbpSession.UserId) || (s.CreatorUserId == AbpSession.UserId))
                            .Where(s => danhSachBaiVietCoHashtag.Contains(s.Id))
                            .OrderByDescending(s => s.LastModificationTime).ThenByDescending(s => s.CreationTime).AsEnumerable()
                         join l in WorkScope.GetAll<UserBlog>()
                         on s.CreatorUserId equals l.CreatorUserId into temped1
                         from ll in temped1.DefaultIfEmpty()
                         join u in Comment
                         on s.Id equals u.EntityId into temped
                         from i in temped.DefaultIfEmpty()
                         select new BlogFeedDto
                         {
                             Status = s.Status,
                             Detail = s.Detail,
                             FullName = $"{s.User.Surname} {s.User.Name}",
                             PostId = s.Id,
                             Tilte = s.Title,
                             TimeCreated = s.CreationTime,
                             UrlAvatar = ll == null ? s.User.Avatar : (ll.AvatarUrl.IsNullOrWhiteSpace() ? s.User.Avatar : ll.AvatarUrl),
                             UserId = s.User.Id,
                             TotalView = s.CountView,
                             TotalComment = ((i?.SubCommentCount ?? 0) + (i?.Commentcount ?? 0)),
                             Reaction = s.Reaction.Where(s => s.UserID == (AbpSession.UserId ?? 0)).Select(s => s.Reaction).FirstOrDefault(),
                             TotalReaction = s.Reaction != null ? s.Reaction.LongCount() : 0,
                         }).AsQueryable();
            var Result = query.OrderByDescending(s => s.TimeCreated).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();                
            var TotalCount = query.Count();
            return new PagedResultDto<BlogFeedDto>
            {
                Items = Result,
                TotalCount = TotalCount
            };

        }
        [HttpGet]
        public async Task<List<Top10HashTag>> Top10Hashtag()
        {
            return WorkScope.GetRepo<PostHashtag>().GetAllIncluding(s => s.HashTags)
                .AsEnumerable()
                .GroupBy(s => s.IdHashTag)
                .Select(s => new Top10HashTag
                {
                    Hashtag = s.First().HashTags.Hashtag,
                    UsedTimes = s.LongCount(),
                }).OrderByDescending(s => s.UsedTimes).
               Take(10).ToList();
        }
        [HttpPost]
        public async Task<PagedResultDto<BlogFeedDto>> Searching(FindHashtagDto input)
        {
            var idhashtag = await WorkScope.GetAll<HashTag>().Where(s => s.Hashtag.ToLower().Contains(input.Hashtag.ToLower())).OrderByDescending(s => s.Hashtag.Length).Select(s => s.Id).ToListAsync();
            var idBaiViet = await WorkScope.GetAll<PostHashtag>().Where(s => idhashtag.Contains(s.IdHashTag)).OrderByDescending(s => s.CreationTime).Select(s => s.IdPost).ToListAsync();
            var Comment =   WorkScope.GetAll<MainComment>().Include(s => s.SubComments).Where(s => s.EntityName == EntityConstant.BlogEntity && idBaiViet.Contains(s.EntityId)).ToList().GroupBy(s => s.EntityId)
                    .Select(s => new CommentCount
                    {
                        Id = s.First().Id,
                        EntityName = "Blog",
                        EntityId = s.Key,
                        Commentcount = s.Select(s => s.Id).LongCount(),
                        SubCommentCount = s.Select(s => s.SubComments.LongCount()).FirstOrDefault(),
                    }).ToList();

            var query = (from s in WorkScope.GetRepo<BlogPost>().GetAllIncluding(s => s.User, k => k.Reaction)
                         .Where(s => idBaiViet.Contains(s.Id))
                         .Where(s => (s.Status == StatusOfBlog.Public && s.CreatorUserId != AbpSession.UserId) || (s.CreatorUserId == (AbpSession.UserId)))
                         .OrderByDescending(s => s.CreationTime).AsEnumerable()
                         join l in WorkScope.GetAll<UserBlog>()
                         on s.CreatorUserId equals l.CreatorUserId into temped1
                         from ll in temped1.DefaultIfEmpty()
                         join u in Comment
                         on s.Id equals u.EntityId into temped
                         from i in temped.DefaultIfEmpty()
                         select new BlogFeedDto
                         {
                             Status = s.Status,
                             Detail = s.Detail,
                             FullName = $"{s.User.Surname} {s.User.Name}",
                             PostId = s.Id,
                             Tilte = s.Title,
                             TimeCreated = s.CreationTime,
                             UrlAvatar = ll == null ? s.User.Avatar : (ll.AvatarUrl.IsNullOrWhiteSpace() ? s.User.Avatar : ll.AvatarUrl),
                             UserId = s.User.Id,
                             TotalView = s.CountView,
                             TotalComment = ((i?.SubCommentCount ?? 0) + (i?.Commentcount ?? 0)),
                             Reaction = s.Reaction.Where(s => s.UserID == (AbpSession.UserId ?? 0)).Select(s => s.Reaction).FirstOrDefault(),
                             TotalReaction = s.Reaction != null ? s.Reaction.LongCount() : 0,
                         }).AsQueryable();
            var Result = query.OrderByDescending(s => s.TimeCreated).Skip((input.PageNumber - 1) * input.PageSize).Take(input.PageSize).ToList();
            var ListPostId = Result.Select(s => s.PostId).ToList();
            var TotalCount = query.Count();
            return new PagedResultDto<BlogFeedDto>
            {
                Items = Result,
                TotalCount = TotalCount
            };
        }

    }
}
