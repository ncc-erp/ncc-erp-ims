using Abp;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Notifications;
using Erpinfo.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Erpinfo.Notification
{
    public class Notification:ITransientDependency
    {
        private readonly INotificationSubscriptionManager notificationSubscriptionManager;
        public Notification(INotificationSubscriptionManager notificationSubscriptionManager)
        {
            this.notificationSubscriptionManager = notificationSubscriptionManager;
        }
        public async Task SendNotifAboutReaction(long userId,long PostId)
        {
            await notificationSubscriptionManager.SubscribeAsync(new UserIdentifier(null,userId), "Reaction", new EntityIdentifier(typeof(BlogPost), PostId));
        }

        
    }
}
