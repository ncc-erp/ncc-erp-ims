using Abp.MultiTenancy;
using System.Collections.Generic;
using static Erpinfo.Authorization.Roles.StaticRoleNames;

namespace Erpinfo.Authorization
{
    public static class PermissionNames
    {
        public const string Pages_Tenants = "Pages.Tenants";

        public const string Pages_Users = "Pages.Users";

        public const string Pages_Roles = "Pages.Roles";

        public class Configuration
        {
            public const string View = "Configuration.View";
            public const string Edit = "Configuration.Edit";
        }
        public class EntityType
        {
            public const string Edit = "Entity.Edit";
            public const string View = "Entity.View";
        }
        public class New
        {
            public const string News = "New";
            public const string Edit = "New.Edit";
            public const string View = "New.View";
            public const string Delete = "New.Delete";
            public const string Create = "New.Create";
            public const string ViewApprovedOnly = "New.ViewApprovedOnly";
        }
        public class Policy
        {
            public const string Policies = "Policy";
            public const string Edit = "Policy.Edit";
            public const string View = "Policy.View";
            public const string Delete = "Policy.Delete";
            public const string Create = "Policy.Create";
            public const string ViewApprovedOnly = "Policy.ViewApprovedOnly";
        }
        public class GuildLine
        {
            public const string GuildLines = "GuildLine";
            public const string Edit = "GuildLine.Edit";
            public const string View = "GuildLine.View";
            public const string Delete = "GuildLine.Delete";
            public const string Create = "GuildLine.Create";
            public const string ViewApprovedOnly = "GuildLine.ViewApprovedOnly";

        }
        public class Event
        {
            public const string Events = "Event";
            public const string Edit = "Event.Edit";
            public const string View = "Event.View";
            public const string Delete = "Event.Delete";
            public const string Create = "Event.Create";
            public const string ViewApprovedOnly = "Event.ViewApprovedOnly";
        }
        public class FaceId
        {
            public const string FaceIds = "FaceId";
            public const string ManagerFaceId = "ManagerFaceId";
        }

        public class Job
        {
            public const string Jobs = "Job";
            public const string Create = "Job.Create";
            public const string Edit = "Job.Edit";
            public const string View = "Job.View";
            public const string Delete = "Job.Delete";
            public const string ChangeStatus = "Job.ChangeStatus";
            public const string ViewDetail = "Job.ViewDetail";
        }
        public class MyJob
        {
            public const string MyJobs = "MyJob";
            public const string View = "MyJob.View";
            public const string ChangeStatus = "MyJob.ChangeStatus";
            public const string ChangeOrder = "MyJob.ChangeOrder";
            public const string ViewDetail = "MyJob.ViewDetail";
            public const string Remind = "MyJob.Remind";
            public const string ViewJobInDashboard = "MyJob.ViewJobInDashboard";
        }
        public class UnlockTimesheet
        {
            public const string ViewTimesheetlocked = "UnlockTimesheet.ViewTimesheetlocked";
            public const string Unlock = "UnlockTimesheet.Unlock";
            public const string ViewHistory = "UnlockTimesheet.ViewHistory";
            public const string ViewTopUnlock = "UnlockTimesheet.ViewTopUnlock";
            public const string UnlockSaturday = "UnlockTimesheet.UnlockSaturday";
        }
         public class TraditionAlbum
        {
            public const string GetAllAlbum = "TraditionAlbum.GetAllAlbum";
            public const string ViewDetailAlbum = "TraditionAlbum.ViewDetailAlbum";
            public const string Create = "TraditionAlbum.Create";
            public const string Edit = "TraditionAlbum.Edit";
            public const string Delete = "TraditionAlbum.Delete";
        }
        public class Feedback
        {
            public const string GetAllFeedbackAdmin = "Feedback.GetAllFeedbackAdmin";
            public const string GetAllFeedbackUser = "Feedback.GetAllFeedbackUser";
            public const string Approve = "Feedback.Approve";
            public const string Reject = "Feedback.Reject";
        }
        public class ChangeStatus
        {
            public const string ChangeStatuses = "ChangeStatus";
            public class New
            {
                public const string CanWaitingFromDraft = "New.CanWaitingFromDraft";
                public const string CanAppoveFromReturn = "New.CanAppoveFromReturn";
                public const string CanAppoveFromWaiting = "New.CanAppoveFromWaiting";
                public const string CanReturnFromWaiting = "New.CanReturnFromWaiting";
                public const string CanWaitingFromReturn = "New.CanWaitingFromReturn";
                public const string CanReturnFromApprove = "New.CanReturnFromApprove";
                public const string CanDisableFromApprove = "New.CanDisableFromApprove";
                public const string CanHiddenFromApprove = "New.CanHiddenFromApprove";
            }
            public class Event
            {
                public const string CanWaitingFromDraft = "Event.CanWaitingFromDraft";
                public const string CanAppoveFromReturn = "Event.CanAppoveFromReturn";
                public const string CanAppoveFromWaiting = "Event.CanAppoveFromWaiting";
                public const string CanReturnFromWaiting = "Event.CanReturnFromWaiting";
                public const string CanWaitingFromReturn = "Event.CanWaitingFromReturn";
                public const string CanReturnFromApprove = "Event.CanReturnFromApprove";
                public const string CanDisableFromApprove = "Event.CanDisableFromApprove";
                public const string CanHiddenFromApprove = "Event.CanHiddenFromApprove";
            }
            public class GuildLine
            {
                public const string CanWaitingFromDraft = "GuildLine.CanWaitingFromDraft";
                public const string CanAppoveFromReturn = "GuildLine.CanAppoveFromReturn";
                public const string CanAppoveFromWaiting = "GuildLine.CanAppoveFromWaiting";
                public const string CanReturnFromWaiting = "GuildLine.CanReturnFromWaiting";
                public const string CanWaitingFromReturn = "GuildLine.CanWaitingFromReturn";
                public const string CanReturnFromApprove = "GuildLine.CanReturnFromApprove";
                public const string CanDisableFromApprove = "GuildLine.CanDisableFromApprove";
                public const string CanHiddenFromApprove = "GuildLine.CanHiddenFromApprove";
            }
            public class Policy
            {
                public const string CanWaitingFromDraft = "Policy.CanWaitingFromDraft";
                public const string CanAppoveFromReturn = "Policy.CanAppoveFromReturn";
                public const string CanAppoveFromWaiting = "Policy.CanAppoveFromWaiting";
                public const string CanReturnFromWaiting = "Policy.CanReturnFromWaiting";
                public const string CanWaitingFromReturn = "Policy.CanWaitingFromReturn";
                public const string CanReturnFromApprove = "Policy.CanReturnFromApprove";
                public const string CanDisableFromApprove = "Policy.CanDisableFromApprove";
                public const string CanHiddenFromApprove = "Policy.CanHiddenFromApprove";
            }
            public const string CanWaitingFromDraft = "CanWaitingFromDraft";
            public const string CanAppoveFromReturn = "CanAppoveFromReturn";
            public const string CanAppoveFromWaiting = "CanAppoveFromWaiting";
            public const string CanReturnFromWaiting = "CanReturnFromWaiting";
            public const string CanWaitingFromReturn = "CanWaitingFromReturn";
            public const string CanReturnFromApprove = "CanReturnFromApprove";
            public const string CanDisableFromApprove = "CanDisableFromApprove";
            public const string CanHiddenFromApprove = "CanHiddenFromApprove";
        }
    }
    public class SystemPermission
    {
        public string Permission { get; set; }
        public MultiTenancySides MultiTenancySides { get; set; }
        public string DisplayName { get; set; }
        public bool IsConfiguration { get; set; }
        //public List<SystemPermission> Children { get; set; }

        public static List<SystemPermission> ListPermissions = new List<SystemPermission>()
        {
            new SystemPermission{Permission = PermissionNames.Pages_Tenants, MultiTenancySides= MultiTenancySides.Host, DisplayName= "Tenants"},
            new SystemPermission{Permission = PermissionNames.Pages_Roles, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName="Roles"},
            new SystemPermission{Permission = PermissionNames.Pages_Users, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName="Users"},

            new SystemPermission{Permission = PermissionNames.Configuration.View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Configuration.View, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Configuration.Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Configuration.Edit, IsConfiguration = true},

            new SystemPermission{Permission = PermissionNames.EntityType.Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.EntityType.Edit, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.EntityType.View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.EntityType.View, IsConfiguration = true},

            new SystemPermission{Permission = PermissionNames.New.View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.New.View, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.New.Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.New.Edit, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.New.Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.New.Delete, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.New.Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.New.Create, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.New.ViewApprovedOnly, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.New.ViewApprovedOnly, IsConfiguration = true},


            new SystemPermission{Permission = PermissionNames.Event.Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Event.Edit, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Event.View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Event.View, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Event.Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Event.Delete, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Event.Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Event.Create, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Event.ViewApprovedOnly, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Event.ViewApprovedOnly, IsConfiguration = true},


            new SystemPermission{Permission = PermissionNames.GuildLine.Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.GuildLine.Edit, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.GuildLine.View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.GuildLine.View, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.GuildLine.Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.GuildLine.Delete, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.GuildLine.Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.GuildLine.Create, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.GuildLine.ViewApprovedOnly, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.GuildLine.ViewApprovedOnly, IsConfiguration = true},


            new SystemPermission{Permission = PermissionNames.Policy.Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Policy.Delete, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Policy.View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Policy.View, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Policy.Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Policy.Edit, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Policy.Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Policy.Create, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Policy.ViewApprovedOnly, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Policy.ViewApprovedOnly, IsConfiguration = true},

            new SystemPermission{Permission = PermissionNames.TraditionAlbum.Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.TraditionAlbum.Delete, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.TraditionAlbum.ViewDetailAlbum, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.TraditionAlbum.ViewDetailAlbum, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.TraditionAlbum.Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.TraditionAlbum.Edit, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.TraditionAlbum.Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.TraditionAlbum.Create, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.TraditionAlbum.GetAllAlbum, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.TraditionAlbum.GetAllAlbum, IsConfiguration = true},


            new SystemPermission{Permission = PermissionNames.ChangeStatus.CanWaitingFromDraft, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanWaitingFromDraft, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.CanAppoveFromReturn, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanAppoveFromReturn, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.CanAppoveFromWaiting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanAppoveFromWaiting, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.CanReturnFromWaiting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanReturnFromWaiting, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.CanWaitingFromReturn, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanWaitingFromReturn, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.CanReturnFromApprove, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanReturnFromApprove, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.CanDisableFromApprove, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanDisableFromApprove, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.CanHiddenFromApprove, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanHiddenFromApprove, IsConfiguration = true},

             new SystemPermission{Permission = PermissionNames.ChangeStatus.New.CanWaitingFromDraft, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanWaitingFromDraft, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.New.CanAppoveFromReturn, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanAppoveFromReturn, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.New.CanAppoveFromWaiting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanAppoveFromWaiting, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.New.CanReturnFromWaiting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanReturnFromWaiting, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.New.CanWaitingFromReturn, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanWaitingFromReturn, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.New.CanReturnFromApprove, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanReturnFromApprove, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.New.CanDisableFromApprove, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanDisableFromApprove, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.New.CanHiddenFromApprove, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanHiddenFromApprove, IsConfiguration = true},

            new SystemPermission{Permission = PermissionNames.ChangeStatus.Policy.CanWaitingFromDraft, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanWaitingFromDraft, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.Policy.CanAppoveFromReturn, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanAppoveFromReturn, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.Policy.CanAppoveFromWaiting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanAppoveFromWaiting, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.Policy.CanReturnFromWaiting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanReturnFromWaiting, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.Policy.CanWaitingFromReturn, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanWaitingFromReturn, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.Policy.CanReturnFromApprove, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanReturnFromApprove, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.Policy.CanDisableFromApprove, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanDisableFromApprove, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.Policy.CanHiddenFromApprove, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanHiddenFromApprove, IsConfiguration = true},

            new SystemPermission{Permission = PermissionNames.ChangeStatus.GuildLine.CanWaitingFromDraft, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanWaitingFromDraft, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.GuildLine.CanAppoveFromReturn, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanAppoveFromReturn, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.GuildLine.CanAppoveFromWaiting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanAppoveFromWaiting, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.GuildLine.CanReturnFromWaiting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanReturnFromWaiting, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.GuildLine.CanWaitingFromReturn, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanWaitingFromReturn, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.GuildLine.CanReturnFromApprove, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanReturnFromApprove, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.GuildLine.CanDisableFromApprove, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanDisableFromApprove, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.GuildLine.CanHiddenFromApprove, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanHiddenFromApprove, IsConfiguration = true},

            new SystemPermission{Permission = PermissionNames.ChangeStatus.Event.CanWaitingFromDraft, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanWaitingFromDraft, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.Event.CanAppoveFromReturn, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanAppoveFromReturn, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.Event.CanAppoveFromWaiting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanAppoveFromWaiting, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.Event.CanReturnFromWaiting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanReturnFromWaiting, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.Event.CanWaitingFromReturn, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanWaitingFromReturn, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.Event.CanReturnFromApprove, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanReturnFromApprove, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.Event.CanDisableFromApprove, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanDisableFromApprove, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.ChangeStatus.Event.CanHiddenFromApprove, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.ChangeStatus.CanHiddenFromApprove, IsConfiguration = true},

            //job
            new SystemPermission{Permission = PermissionNames.Job.Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Job.Edit, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Job.Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Job.Create, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Job.Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Job.Delete, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Job.ChangeStatus, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Job.ChangeStatus, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Job.View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Job.View, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Job.ViewDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Job.ViewDetail, IsConfiguration = true},
            //myjob
            new SystemPermission{Permission = PermissionNames.MyJob.View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.MyJob.View, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.MyJob.ChangeStatus, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.MyJob.ChangeStatus, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.MyJob.ChangeOrder, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.MyJob.ChangeOrder, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.MyJob.ViewDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.MyJob.ViewDetail, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.MyJob.ViewJobInDashboard, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.MyJob.ViewJobInDashboard, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.MyJob.Remind, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.MyJob.Remind, IsConfiguration = true},

            //unlock timesheet
            new SystemPermission{Permission = PermissionNames.UnlockTimesheet.ViewTimesheetlocked, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.UnlockTimesheet.ViewTimesheetlocked, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.UnlockTimesheet.ViewTopUnlock, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.UnlockTimesheet.ViewTopUnlock, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.UnlockTimesheet.ViewHistory, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.UnlockTimesheet.ViewHistory, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.UnlockTimesheet.Unlock, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.UnlockTimesheet.Unlock, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.UnlockTimesheet.UnlockSaturday, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.UnlockTimesheet.UnlockSaturday, IsConfiguration = true},

            //feedback
            new SystemPermission{Permission = PermissionNames.Feedback.GetAllFeedbackAdmin, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Feedback.GetAllFeedbackAdmin, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Feedback.GetAllFeedbackUser, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Feedback.GetAllFeedbackUser, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Feedback.Approve, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Feedback.Approve, IsConfiguration = true},
            new SystemPermission{Permission = PermissionNames.Feedback.Reject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = PermissionNames.Feedback.Reject, IsConfiguration = true}
    };
    } 
    public class GrantPermissionRoles
    {
        public static Dictionary<string, List<string>> PermissionRoles = new Dictionary<string, List<string>>
        {
            {
                Tenants.Ceo,
                    new List<string>
                    {
                        PermissionNames.Pages_Users,
                        PermissionNames.Pages_Roles,
                        PermissionNames.Pages_Tenants,
                        PermissionNames.EntityType.Edit,
                        PermissionNames.EntityType.View,
                        PermissionNames.New.Edit,
                        PermissionNames.New.View,
                        PermissionNames.New.Delete,
                        PermissionNames.GuildLine.Edit,
                        PermissionNames.GuildLine.View,
                        PermissionNames.GuildLine.Delete,
                        PermissionNames.Event.Delete,
                        PermissionNames.Event.Edit,
                        PermissionNames.Event.View,
                        PermissionNames.Policy.View,
                        PermissionNames.Policy.Edit,
                        PermissionNames.Policy.Delete,
                        PermissionNames.ChangeStatus.CanAppoveFromReturn,
                        PermissionNames.ChangeStatus.CanAppoveFromWaiting,
                        PermissionNames.ChangeStatus.CanDisableFromApprove,
                        PermissionNames.ChangeStatus.CanHiddenFromApprove,
                        PermissionNames.ChangeStatus.CanReturnFromApprove,
                        PermissionNames.ChangeStatus.CanReturnFromWaiting,
                        PermissionNames.Job.ChangeStatus,
                        PermissionNames.Job.Edit,
                        PermissionNames.Job.Delete,
                        PermissionNames.Job.Create,
                        PermissionNames.Job.View,
                        PermissionNames.Job.ViewDetail,
                        PermissionNames.MyJob.ChangeStatus,
                        PermissionNames.MyJob.View,
                        PermissionNames.MyJob.ChangeOrder,
                        PermissionNames.MyJob.ViewDetail,
                        PermissionNames.MyJob.ViewJobInDashboard,
                        PermissionNames.MyJob.Remind,
                        PermissionNames.UnlockTimesheet.ViewTimesheetlocked,
                        PermissionNames.UnlockTimesheet.ViewHistory,
                        PermissionNames.UnlockTimesheet.ViewTopUnlock,
                        PermissionNames.UnlockTimesheet.Unlock,
                        PermissionNames.UnlockTimesheet.UnlockSaturday,
                        PermissionNames.TraditionAlbum.Create,
                        PermissionNames.TraditionAlbum.Edit,
                        PermissionNames.TraditionAlbum.Delete,
                        PermissionNames.TraditionAlbum.GetAllAlbum,
                        PermissionNames.TraditionAlbum.ViewDetailAlbum,
                        PermissionNames.Feedback.GetAllFeedbackAdmin,
                        PermissionNames.Feedback.GetAllFeedbackUser,
                        PermissionNames.Feedback.Approve,
                        PermissionNames.Feedback.Reject
                    }
            },
            {
               Tenants.Hr,
                    new List<string>
                    {
                        PermissionNames.Pages_Users,
                        PermissionNames.Pages_Roles,
                        PermissionNames.Pages_Tenants,
                        PermissionNames.EntityType.Edit,
                        PermissionNames.EntityType.View,
                        PermissionNames.New.Edit,
                        PermissionNames.New.View,
                        PermissionNames.New.Delete,
                        PermissionNames.GuildLine.Edit,
                        PermissionNames.GuildLine.View,
                        PermissionNames.GuildLine.Delete,
                        PermissionNames.Event.Delete,
                        PermissionNames.Event.Edit,
                        PermissionNames.Event.View,
                        PermissionNames.Policy.View,
                        PermissionNames.Policy.Edit,
                        PermissionNames.Policy.Delete,
                        PermissionNames.ChangeStatus.CanWaitingFromDraft,
                        PermissionNames.ChangeStatus.CanWaitingFromReturn,
                        PermissionNames.New.Create,
                        PermissionNames.GuildLine.Create,
                        PermissionNames.Policy.Create,
                        PermissionNames.Event.Create,
                        PermissionNames.Job.ChangeStatus,
                        PermissionNames.Job.Edit,
                        PermissionNames.Job.Delete,
                        PermissionNames.Job.Create,
                        PermissionNames.Job.View,
                        PermissionNames.Job.ViewDetail,
                        PermissionNames.MyJob.ChangeStatus,
                        PermissionNames.MyJob.View,
                        PermissionNames.MyJob.ChangeOrder,
                        PermissionNames.MyJob.ViewDetail,
                        PermissionNames.MyJob.ViewJobInDashboard,
                        PermissionNames.MyJob.Remind,
                        PermissionNames.UnlockTimesheet.ViewTimesheetlocked,
                        PermissionNames.UnlockTimesheet.ViewHistory,
                        PermissionNames.UnlockTimesheet.ViewTopUnlock,
                        PermissionNames.UnlockTimesheet.Unlock,
                        PermissionNames.UnlockTimesheet.UnlockSaturday,
                        PermissionNames.TraditionAlbum.Create,
                        PermissionNames.TraditionAlbum.Edit,
                        PermissionNames.TraditionAlbum.Delete,
                        PermissionNames.TraditionAlbum.ViewDetailAlbum,
                        PermissionNames.TraditionAlbum.GetAllAlbum,
                        PermissionNames.Feedback.GetAllFeedbackAdmin,
                        PermissionNames.Feedback.GetAllFeedbackUser,
                        PermissionNames.Feedback.Approve,
                        PermissionNames.Feedback.Reject
                    }

            },
             {
               Tenants.Employee,
                    new List<string>
                    {
                        PermissionNames.EntityType.View,
                        PermissionNames.New.View,
                        PermissionNames.Policy.View,
                        PermissionNames.GuildLine.View,
                        PermissionNames.Event.View,
                        PermissionNames.New.ViewApprovedOnly,
                        PermissionNames.Policy.ViewApprovedOnly,
                        PermissionNames.GuildLine.ViewApprovedOnly,
                        PermissionNames.Event.ViewApprovedOnly,
                        PermissionNames.Job.ChangeStatus,
                        PermissionNames.Job.Edit,
                        PermissionNames.Job.Delete,
                        PermissionNames.Job.Create,
                        PermissionNames.Job.View,
                        PermissionNames.Job.ViewDetail,
                        PermissionNames.MyJob.ChangeStatus,
                        PermissionNames.MyJob.View,
                        PermissionNames.MyJob.ChangeOrder,
                        PermissionNames.MyJob.ViewDetail,
                        PermissionNames.MyJob.ViewJobInDashboard,
                        PermissionNames.MyJob.Remind,
                        PermissionNames.UnlockTimesheet.ViewTimesheetlocked,
                        PermissionNames.UnlockTimesheet.ViewHistory,
                        PermissionNames.UnlockTimesheet.ViewTopUnlock,
                        PermissionNames.UnlockTimesheet.Unlock,
                        PermissionNames.UnlockTimesheet.UnlockSaturday,
                        PermissionNames.TraditionAlbum.ViewDetailAlbum,
                        PermissionNames.TraditionAlbum.GetAllAlbum,
                        PermissionNames.Feedback.GetAllFeedbackUser,
                    }

            },
              {
               Tenants.Intern,
                   new List<string>
                   {
                        PermissionNames.EntityType.View,
                        PermissionNames.New.View,
                        PermissionNames.Policy.View,
                        PermissionNames.GuildLine.View,
                        PermissionNames.Event.View,
                        PermissionNames.New.ViewApprovedOnly,
                        PermissionNames.Policy.ViewApprovedOnly,
                        PermissionNames.GuildLine.ViewApprovedOnly,
                        PermissionNames.Event.ViewApprovedOnly,
                        PermissionNames.Job.ChangeStatus,
                        PermissionNames.Job.Edit,
                        PermissionNames.Job.Delete,
                        PermissionNames.Job.Create,
                        PermissionNames.Job.View,
                        PermissionNames.Job.ViewDetail,
                        PermissionNames.MyJob.ChangeStatus,
                        PermissionNames.MyJob.View,
                        PermissionNames.MyJob.ChangeOrder,
                        PermissionNames.MyJob.ViewDetail,
                        PermissionNames.MyJob.ViewJobInDashboard,
                        PermissionNames.MyJob.Remind,
                        PermissionNames.UnlockTimesheet.ViewTimesheetlocked,
                        PermissionNames.UnlockTimesheet.ViewHistory,
                        PermissionNames.UnlockTimesheet.ViewTopUnlock,
                        PermissionNames.UnlockTimesheet.Unlock,
                        PermissionNames.UnlockTimesheet.UnlockSaturday,
                        PermissionNames.TraditionAlbum.ViewDetailAlbum,
                        PermissionNames.TraditionAlbum.GetAllAlbum,
                        PermissionNames.Feedback.GetAllFeedbackUser,
                   }

            },
            {
                Tenants.Admin,
                    new List<string>
                    {
                        PermissionNames.Pages_Users,
                        PermissionNames.Pages_Roles,
                        PermissionNames.Pages_Tenants,
                        PermissionNames.Configuration.View,
                        PermissionNames.Configuration.Edit,
                        PermissionNames.EntityType.Edit,
                        PermissionNames.EntityType.View,
                        PermissionNames.New.Edit,
                        PermissionNames.New.View,
                        PermissionNames.New.Delete,
                        PermissionNames.GuildLine.Edit,
                        PermissionNames.GuildLine.View,
                        PermissionNames.GuildLine.Delete,
                        PermissionNames.Event.Delete,
                        PermissionNames.Event.Edit,
                        PermissionNames.Event.View,
                        PermissionNames.Policy.View,
                        PermissionNames.Policy.Edit,
                        PermissionNames.Policy.Delete,
                        PermissionNames.ChangeStatus.CanAppoveFromReturn,
                        PermissionNames.ChangeStatus.CanAppoveFromWaiting,
                        PermissionNames.ChangeStatus.CanDisableFromApprove,
                        PermissionNames.ChangeStatus.CanHiddenFromApprove,
                        PermissionNames.ChangeStatus.CanReturnFromApprove,
                        PermissionNames.ChangeStatus.CanReturnFromWaiting,
                        PermissionNames.ChangeStatus.CanWaitingFromDraft,
                        PermissionNames.ChangeStatus.CanWaitingFromReturn,
                        PermissionNames.New.Create,
                        PermissionNames.GuildLine.Create,
                        PermissionNames.Policy.Create,
                        PermissionNames.Event.Create,
                        PermissionNames.Job.ChangeStatus,
                        PermissionNames.Job.Edit,
                        PermissionNames.Job.Delete,
                        PermissionNames.Job.Create,
                        PermissionNames.Job.View,
                        PermissionNames.Job.ViewDetail,
                        PermissionNames.MyJob.ChangeStatus,
                        PermissionNames.MyJob.View,
                        PermissionNames.MyJob.ChangeOrder,
                        PermissionNames.MyJob.ViewDetail,
                        PermissionNames.MyJob.ViewJobInDashboard,
                        PermissionNames.MyJob.Remind,
                        PermissionNames.UnlockTimesheet.ViewTimesheetlocked,
                        PermissionNames.UnlockTimesheet.ViewHistory,
                        PermissionNames.UnlockTimesheet.ViewTopUnlock,
                        PermissionNames.UnlockTimesheet.Unlock,
                        PermissionNames.UnlockTimesheet.UnlockSaturday,
                        PermissionNames.TraditionAlbum.Create,
                        PermissionNames.TraditionAlbum.Edit,
                        PermissionNames.TraditionAlbum.Delete,
                        PermissionNames.TraditionAlbum.ViewDetailAlbum,
                        PermissionNames.TraditionAlbum.GetAllAlbum,
                        PermissionNames.Feedback.GetAllFeedbackAdmin,
                        PermissionNames.Feedback.GetAllFeedbackUser,
                        PermissionNames.Feedback.Approve,
                        PermissionNames.Feedback.Reject
                    }
            }
        };
    }
}
