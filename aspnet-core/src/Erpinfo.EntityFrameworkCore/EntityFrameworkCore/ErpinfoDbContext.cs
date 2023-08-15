using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Erpinfo.Authorization.Roles;
using Erpinfo.Authorization.Users;
using Erpinfo.MultiTenancy;
using Erpinfo.Entities;

namespace Erpinfo.EntityFrameworkCore
{
    public class ErpinfoDbContext : AbpZeroDbContext<Tenant, Role, User, ErpinfoDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<QuickNews> QuickNews { get; set; } 
        public DbSet<News> News { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<Policies> Policies { get; set; }
        public DbSet<GuildLines> GuildLines { get; set; }
        public DbSet<EntityType> EntityType { get; set; }
        public DbSet<EntityChangeLog> EntityChangeLog { get; set; }
        public DbSet<EntityGroups> EntityGroups { get; set; }
        public DbSet<UserCalendar> UserCalendars { get; set; }
        public DbSet<MainComment> MainComments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }
        public DbSet<UserLikeMainComment> UserLikeMainComments { get; set; }
        public DbSet<UserLikeSubComment> UserLikeSubComments { get; set; }
        public DbSet<SystemNotification> SystemNotifications { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }        
        public DbSet<HashTag> HashTags { get; set; }
        public DbSet<UserBlog> UserBlogs { get; set; }
        public DbSet<UserLikeOrDislikeBlogPost> UserLikeOrDislikeBlogPosts { get; set; }
        public DbSet<PostHashtag> PostHashtags { get; set; }   
        public DbSet<ContestImage> ContestImages { set; get; }
        public DbSet<ImagePublish> ImagePublishes { get; set; }
        public DbSet<UserLikePublishImage> UserLikePublishImages { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<UserJob> UserJobs { get; set; }
        public DbSet<TraditionAlbum> TranditionAlbums { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<UserLikeOrDislikeEvents> UserLikeOrDislikeEvents { get; set; }
        public DbSet<UserLikeOrDislikeGuideLines> UserLikeOrDislikeGuideLines { get; set; }
        public DbSet<UserLikeOrDislikeNews> UserLikeOrDislikeNews { get; set; }
        public DbSet<UserLikeOrDislikePolicy> UserLikeOrDislikePolicies { get; set; }

        public ErpinfoDbContext(DbContextOptions<ErpinfoDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ContestImage>().HasQueryFilter(i => i.IsDeleted==false);
        }
    }
}
