using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Constant
{
    public class StatusEnum
    {
        public enum ProjectStatus
        {
            Active = 0,
            Deactive = 1,
        }
        public enum ProjectType
        {
            TimeAndMaterials = 0,
            FixedFee = 1,
            NoneBillable = 2,
            ODC = 3,
            Product = 4,
            Training = 5
        }
        public enum TaskType
        {
            CommonTask = 0,
            OrtherTask = 1
        }
        public enum TypeOfWork
        {
            NormalWorkingHours = 0,
            OverTime = 1
        }
        public enum TimesheetStatus
        {
            All = -1,
            None = 0,
            Pending = 1,
            Approve = 2,
            Reject = 3
        }
        public enum TimesheetSort
        {
            Project = 0,
            People = 1,
            Week = 2
        }

        public enum Usertype : byte
        {
            Staff = 0,
            Internship = 1,
            Collaborators = 2

            //Internship = 0,
            //Collaborators = 1,
            //Staff = 2,
            //ProbationaryStaff = 3
        }
        public enum UserLevel : byte
        {
            Intern_0 = 0,
            Intern_600K = 1,
            Intern_2M = 2,
            Intern_4M = 3,
            FresherMinus = 4,
            Fresher = 5,
            FresherPlus = 6,
            JuniorMinus = 7,
            Junior = 8,
            JuniorPlus = 9,
            MiddleMinus = 10,
            Middle = 11,
            MiddlePlus = 12,
            SeniorMinus = 13,
            Senior = 14,
            SeniorPlus = 15,
        }
        public enum AbsenceStatus
        {
            All = 0,
            Pending = 1,
            Approved = 2,
            Rejected = 3,
        }

        public enum RequestType
        {
            Off = 0,
            Onsite = 1,
            Remote = 2,
        }

        public enum AbsenceType
        {
            ToPM = 0,
            ToAdmin = 1
        }

        public enum Branch : byte
        {
            HaNoi = 0,
            DaNang = 1,
            HoChiMinh = 2,
            Vinh = 3
        }

        public enum Sex : byte
        {
            Male = 0,
            Female = 1
        }
        public enum DayType : byte
        {
            Fullday = 1,
            Morning = 2,
            Afternoon = 3,
            Custom = 4,
        }

        public enum OnDayType : byte
        {
            BeginOfDay = 1,
            MiddleOfDay = 2,
            EndOfDay = 3,
        }

        public enum DayOffStatus : byte
        {
            Allowed = 1,
            NotAllowed = 2
        }

        public enum SalaryMonthStatus : byte
        {
            None = 0,
            SendEmail = 1,
            Confirmed = 2
        }

        public enum SalaryStatus : byte
        {
            Active = 0,
            Deactive = 1,
        }

        public enum SalaryType : byte
        {
            BasicSalary = 0,
            Allowances = 1,
            Normal = 2
        }

        public enum LockUnlockTimesheetType : byte
        {
            MyTimesheet = 0,
            ApproveRejectTimesheet = 1
        }

        public enum PunishmentStatus : byte
        {
            Punish = 1,
            Normal = 0
        }

        public enum FundStatus : byte
        {
            Proceeds = 0,
            Payment = 1
        }

        public enum ReviewInternStatus
        {
            Draft = 0,
            Reviewed = 1,
            Approved = 2,
            SentEmail = 3,
            Rejected = -1
        }
        public enum Comparision
        {
            Equal = 0,
            LessThan = 1,
            GreaterThan = 3,
        }
    }
}
