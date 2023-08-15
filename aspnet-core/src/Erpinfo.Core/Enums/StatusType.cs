using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Enums
{
    public enum StatusType
    {
        Draft = 1000,
        Waitting = 2000,
        Approved = 4000,
        Return = 3000,
        Hidden = 5000,
        Disable = 6000,
    }
    public enum CommentOrder
    {
        Oldest = 0,
        Latest = 1,
        Highlight = 2
    }
    public enum NotificationType
    {
        TagMainCommment,
        TagSubComment,
        RepComment,
        ReactComment,
        Calendar,
        Event,
        BlogPost,
        New,
        Policy
    }
    public enum JobStatus
    {
        Todo,
        Done
    }
    public enum FeedbackStatus
    {
        Default = 0,
        Approved = 1,
        Reject = 2,
        Used = 3
    }
    public enum FeedbackType
    {
        Product = 0,
        Company = 1,
        Talk = 2,
        NCC8 = 3
    }
}
