using Abp.Authorization;
using Abp.UI;
using Erpinfo.Comments.Dto;
using Erpinfo.DomainServices;
using Erpinfo.Entities;
using Erpinfo.Extension;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Erpinfo.Paging;
using Erpinfo.Authorization.Users;
using Erpinfo.Enums;
using Erpinfo.IoC;

namespace Erpinfo.Comments
{
    [AbpAuthorize]
    public class CommentsAppService : ErpinfoAppServiceBase
    {
        private readonly UploadImageService imageService;
        public CommentsAppService(UploadImageService imageService)
        {
            this.imageService = imageService;
        }

        [HttpPost]
        public async Task<SaveMainCommentDto> SaveMainComment([FromForm] SaveMainCommentDto input)
        {
            if (input.Id <= 0)
            {
                MainComment item = new MainComment
                {
                    EntityId = input.EntityId,
                    EntityName = input.EntityName,
                    Comment = input.Comment
                };
                item.Image = await imageService.UploadImage(input.Image);
                input.Id = await WorkScope.InsertAndGetIdAsync<MainComment>(item);
                CurrentUnitOfWork.SaveChanges();
                var author = await WorkScope.GetAsync<User>(item.CreatorUserId.Value);
                if (input.UserIds != null)
                {
                    foreach (var u in input.UserIds)
                    {
                        var tagName = new SystemNotification
                        {
                            CommentId = input.Id,
                            Messsage = $"{author.FullName} đã nhắc đến bạn trong một bình luận",
                            UserId = u,
                            EntityId = input.EntityId,
                            EntityName = input.EntityName,
                            IsRead = false,
                            Type = NotificationType.TagMainCommment,
                            AuthorAvatar = author.Avatar
                        };
                        await WorkScope.InsertAsync(tagName);
                    }
                }

            }
            else
            {
                var old = await WorkScope.GetAsync<MainComment>(input.Id);
                if (old == null)
                {
                    throw new UserFriendlyException(string.Format("Comment not found"));
                }
                var oldImage = old.Image;
                old.Comment = input.Comment;
                if (input.Image != null)
                {
                    old.Image = await imageService.UploadImage(input.Image);
                }
                await WorkScope.UpdateAsync<MainComment>(old);
                var author = await WorkScope.GetAsync<User>(old.CreatorUserId.Value);
                var oldUser = await WorkScope.GetAll<SystemNotification>().Where(s => s.CommentId == input.Id && s.Type == NotificationType.TagMainCommment)
                    .Select(s => s.UserId).ToListAsync();
                if (input.UserIds != null)
                {
                    var newUser = input.UserIds;
                    var deleteNoti = oldUser.Except(newUser);
                    var insertNoti = newUser.Except(oldUser);

                    foreach (var u in insertNoti)
                    {
                        var tagName = new SystemNotification
                        {
                            CommentId = input.Id,
                            Messsage = $"{author.FullName} đã nhắc đến bạn trong một bình luận",
                            UserId = u,
                            EntityId = input.EntityId,
                            EntityName = input.EntityName,
                            IsRead = false,
                            Type = NotificationType.TagMainCommment,
                            AuthorAvatar = author.Avatar
                        };
                        await WorkScope.InsertAsync(tagName);
                    }
                    var deleteSystemNo = await WorkScope.GetAll<SystemNotification>().Where(s => s.CommentId == input.Id)
                        .Where(s => deleteNoti.Contains(s.UserId)).Select(s => s.Id).ToListAsync();
                    foreach (var d in deleteSystemNo)
                    {
                        await WorkScope.DeleteAsync<SystemNotification>(d);
                    }
                }
            }
            return input;
        }
        [HttpPost]
        public async Task<SaveSubCommentDto> SaveSubComment([FromForm] SaveSubCommentDto input)
        {
            var mainComment = await WorkScope.GetAsync<MainComment>(input.MainCommentId);
            if (input.Id <= 0)
            {
                SubComment item = new SubComment
                {
                    MainCommentId = input.MainCommentId,
                    Comment = input.Comment
                };
                item.Image = await imageService.UploadImage(input.Image);
                input.Id = await WorkScope.InsertAndGetIdAsync<SubComment>(item);
                CurrentUnitOfWork.SaveChanges();
                var author = await WorkScope.GetAsync<User>(item.CreatorUserId.Value);
                if (input.UserIds != null)
                {
                    foreach (var u in input.UserIds)
                    {
                        var tagName = new SystemNotification
                        {
                            CommentId = input.MainCommentId,
                            Messsage = $"{author.FullName} đã nhắc đến bạn trong một bình luận",
                            UserId = u,
                            EntityId = mainComment.EntityId,
                            EntityName = mainComment.EntityName,
                            IsRead = false,
                            Type = NotificationType.TagSubComment,
                            AuthorAvatar = author.Avatar
                        };
                        await WorkScope.InsertAsync(tagName);
                    }
                    //thong bao tra loi binh luan cua ban
                    if (!input.UserIds.Any(s => s == author.Id))
                    {
                        var repComment = new SystemNotification
                        {
                            CommentId = input.MainCommentId,
                            Messsage = $"{author.FullName} đã trả lời bình luận của bạn",
                            UserId = author.Id,
                            EntityId = mainComment.EntityId,
                            EntityName = mainComment.EntityName,
                            IsRead = false,
                            Type = NotificationType.RepComment,
                            AuthorAvatar = author.Avatar
                        };
                        await WorkScope.InsertAsync(repComment);
                    }
                }
            }
            else
            {
                var old = await WorkScope.GetAsync<SubComment>(input.Id);
                if (old == null)
                {
                    throw new UserFriendlyException(string.Format("Comment not found"));
                }
                var oldImage = old.Image;
                old.Comment = input.Comment;
                if (input.Image != null)
                {
                    old.Image = await imageService.UploadImage(input.Image);
                }
                await WorkScope.UpdateAsync<SubComment>(old);
                var author = await WorkScope.GetAsync<User>(old.CreatorUserId.Value);
                var oldUser = await WorkScope.GetAll<SystemNotification>().Where(s => s.CommentId == input.Id && s.Type == NotificationType.TagSubComment)
                    .Select(s => s.UserId).ToListAsync();
                if (input.UserIds != null)
                {
                    var newUser = input.UserIds;
                    var deleteNoti = oldUser.Except(newUser);
                    var insertNoti = newUser.Except(oldUser);
                    foreach (var u in insertNoti)
                    {
                        var tagName = new SystemNotification
                        {
                            CommentId = input.MainCommentId,
                            Messsage = $"{author.FullName} đã nhắc đến bạn trong một bình luận",
                            UserId = u,
                            EntityId = mainComment.EntityId,
                            EntityName = mainComment.EntityName,
                            IsRead = false,
                            Type = NotificationType.TagSubComment,
                            AuthorAvatar = author.Avatar
                        };
                        await WorkScope.InsertAsync(tagName);
                    }
                    var deleteSystemNo = await WorkScope.GetAll<SystemNotification>().Where(s => s.CommentId == input.Id)
                        .Where(s => deleteNoti.Contains(s.UserId)).Select(s => s.Id).ToListAsync();
                    foreach (var d in deleteSystemNo)
                    {
                        await WorkScope.DeleteAsync<SystemNotification>(d);
                    }
                }
            }
            return input;
        }
        [HttpDelete]
        public async Task DeleteMainComment(long id)
        {
            var old = await WorkScope.GetAsync<MainComment>(id);
            if (old == null)
            {
                throw new UserFriendlyException(string.Format("Comment not found"));
            }
            var listSubComment = await WorkScope.GetAll<SubComment>().Where(s => s.MainCommentId == id).ToListAsync();
            foreach (var comment in listSubComment)
            {
                await WorkScope.DeleteAsync<SubComment>(comment.Id);
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            await WorkScope.DeleteAsync<MainComment>(id);
            //check main comment co Tag thi delete notification
            var tagUser = await WorkScope.GetAll<SystemNotification>().Where(s => s.CommentId == id && s.Type == NotificationType.TagMainCommment)
                .Select(s => s.Id).ToListAsync();
            if (tagUser != null)
            {
                foreach (var t in tagUser)
                {
                    await WorkScope.DeleteAsync<SystemNotification>(t);
                }
            }
        }
        [HttpDelete]
        public async Task DeleteSubComment(long id)
        {
            var old = await WorkScope.GetAsync<SubComment>(id);
            if (old == null)
            {
                throw new UserFriendlyException(string.Format("Comment not found"));
            }
            await WorkScope.DeleteAsync<SubComment>(id);
            //check sub comment co Tag thi delete notification
            var tagUser = await WorkScope.GetAll<SystemNotification>().Where(s => s.CommentId == id && s.Type == NotificationType.TagSubComment || s.Type == NotificationType.RepComment)
                .Select(s => s.Id).ToListAsync();
            if (tagUser != null)
            {
                foreach (var t in tagUser)
                {
                    await WorkScope.DeleteAsync<SystemNotification>(t);
                }
            }
        }
        [HttpPost]
        public async Task<ViewMainCommentDto> GetMainCommentPaging(ViewMainCommentInput input)
        {
            var query = WorkScope.GetAll<MainComment>().Include(s => s.UserLikeMainComments).Include(s => s.SubComments)
                        .Where(x => x.EntityId == input.EntityId && x.EntityName == input.EntityName)
                        .Select(s => new
                        {
                            Id = s.Id,
                            Comment = s.Comment,
                            Image = s.Image,
                            CreationTime = s.CreationTime,
                            LastModificationTime = s.LastModificationTime,
                            LastModifierUserId = s.LastModifierUserId,
                            CreatorUserId = s.CreatorUserId,
                            Like = s.UserLikeMainComments.Count(x => x.Liked == true),
                            TotalReply = s.SubComments.Count(),
                        });
            if (input.Order == CommentOrder.Oldest)
            {
                query = query.OrderBy(s => s.CreationTime);
            }
            else if (input.Order == CommentOrder.Latest)
            {
                query = query.OrderByDescending(s => s.CreationTime);
            }
            else if (input.Order == CommentOrder.Highlight)
            {
                query = query.OrderByDescending(s => s.Like);
            }
            var queryComment = query.Select(s => new
            {
                Id = s.Id,
                Comment = s.Comment,
                Image = s.Image,
                TotalReply = s.TotalReply,
                UserId = s.LastModifierUserId ?? s.CreatorUserId,
                Time = s.LastModificationTime ?? s.CreationTime,
                Like = s.Like
            }).Skip((input.PageNumber - 1) * input.PageSize).Take(input.PageSize);
            var currentUserId = AbpSession.UserId.Value;
            var listMainComment = await (from q in queryComment
                                         join u in WorkScope.GetAll<User>() on q.UserId equals u.Id
                                         select new
                                         {
                                             Id = q.Id,
                                             Comment = q.Comment,
                                             Image = q.Image,
                                             Time = q.Time,
                                             TotalReply = q.TotalReply,
                                             UserName = u.FullName,
                                             UserId = u.Id,
                                             Avatar = u.Avatar,
                                             Like = q.Like,
                                             UserLike = WorkScope.GetAll<UserLikeMainComment>()
                                             .FirstOrDefault(x => x.MainCommentId == q.Id && (x.CreatorUserId ?? x.LastModifierUserId) == currentUserId)
                                         }).Select(s => new MainCommentDto
                                         {
                                             Id = s.Id,
                                             Comment = s.Comment,
                                             Image = s.Image,
                                             Time = s.Time,
                                             TotalReply = s.TotalReply,
                                             UserName = s.UserName,
                                             UserId = s.UserId,
                                             Avatar = s.Avatar,
                                             Like = s.Like,
                                             Liked = s.UserLike == null ? false : s.UserLike.Liked
                                         }).ToListAsync();
            var total = WorkScope.GetAll<MainComment>().Count(s => s.EntityId == input.EntityId && s.EntityName == input.EntityName);
            var totalComment = total + (from m in WorkScope.GetAll<MainComment>().Where(s => s.EntityId == input.EntityId && s.EntityName == input.EntityName)
                                        join s in WorkScope.GetAll<SubComment>() on m.Id equals s.MainCommentId
                                        select s.Id).Count();
            return new ViewMainCommentDto
            {
                Items = listMainComment,
                Total = total,
                TotalComment = totalComment
            };
        }
        [HttpPost]
        public async Task<List<ViewSubCommentDto>> GetSubCommentPaging(ViewSubCommentInput input)
        {
            var query = WorkScope.GetAll<SubComment>().Include(s => s.UserLikeSubComments)
                        .Where(x => x.MainCommentId == input.MainCommentId)
                        .OrderBy(x => x.CreationTime)
                        .Select(s => new
                        {
                            Id = s.Id,
                            Comment = s.Comment,
                            Image = s.Image,
                            Like = s.UserLikeSubComments.Count(x => x.Liked == true),
                            UserId = s.LastModifierUserId ?? s.CreatorUserId,
                            Time = s.LastModificationTime ?? s.CreationTime
                        }).Skip((input.PageNumber - 1) * input.PageSize)
                        .Take(input.PageSize);
            var currentUserId = AbpSession.UserId.Value;
            return await (from q in query
                          join u in WorkScope.GetAll<User>() on q.UserId equals u.Id
                          select new
                          {
                              Id = q.Id,
                              Comment = q.Comment,
                              Image = q.Image,
                              Time = q.Time,
                              UserName = u.FullName,
                              UserId = u.Id,
                              Avatar = u.Avatar,
                              Like = q.Like,
                              UserLike = WorkScope.GetAll<UserLikeSubComment>()
                              .FirstOrDefault(x => x.SubCommentId == q.Id && (x.CreatorUserId ?? x.LastModifierUserId) == currentUserId)
                          }).Select(s => new ViewSubCommentDto
                          {
                              Id = s.Id,
                              Comment = s.Comment,
                              Image = s.Image,
                              Time = s.Time,
                              UserName = s.UserName,
                              UserId = s.UserId,
                              Avatar = s.Avatar,
                              Like = s.Like,
                              Liked = s.UserLike == null ? false : s.UserLike.Liked
                          }).ToListAsync();

        }
        [HttpPost]
        public async Task LikeMainComment(long id)
        {
            var old = await WorkScope.GetAll<UserLikeMainComment>()
                .FirstOrDefaultAsync(s => s.MainCommentId == id && (s.CreatorUserId ?? s.LastModifierUserId) == AbpSession.UserId.Value);
            if (old != null)
            {
                old.Liked = old.Liked == true ? false : true;
                await WorkScope.UpdateAsync<UserLikeMainComment>(old);
            }
            else
            {
                var userLike = await WorkScope.InsertAsync(new UserLikeMainComment
                {
                    MainCommentId = id,
                    Liked = true,
                    CreatorUserId = AbpSession.UserId.Value
                });
                var comment = await WorkScope.GetAsync<MainComment>(id);
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
                await WorkScope.InsertAsync(notification);
            }
        }
        [HttpPost]
        public async Task LikeSubComment(long id)
        {
            var old = await WorkScope.GetAll<UserLikeSubComment>()
                .FirstOrDefaultAsync(s => s.SubCommentId == id && (s.CreatorUserId ?? s.LastModifierUserId) == AbpSession.UserId.Value);
            var mainCommnentId = WorkScope.Get<SubComment>(id).MainCommentId;
            if (old != null)
            {
                old.Liked = old.Liked == true ? false : true;
                await WorkScope.UpdateAsync<UserLikeSubComment>(old);
            }
            else
            {
                var userLike = await WorkScope.InsertAsync(new UserLikeSubComment
                {
                    SubCommentId = id,
                    Liked = true,
                    CreatorUserId = AbpSession.UserId.Value
                });
                var comment = await WorkScope.GetAll<SubComment>().Where(s => s.Id == id).Select(s => s.MainComment).FirstOrDefaultAsync();
                var userLikeCm = await WorkScope.GetAsync<User>(userLike.CreatorUserId.Value);
                var notification = new SystemNotification
                {
                    IsRead = false,
                    Type = NotificationType.ReactComment,
                    Messsage = userLikeCm.FullName + " đã yêu thích bình luận của bạn",
                    EntityId = comment.EntityId,
                    EntityName = comment.EntityName,
                    UserId = comment.CreatorUserId.Value,
                    CommentId = mainCommnentId,
                    AuthorAvatar = userLikeCm.Avatar,
                };
                await WorkScope.InsertAsync(notification);
            }
        }
    }
}
