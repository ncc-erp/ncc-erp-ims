using Erpinfo.Paging;
using System;
using System.Collections.Generic;
using System.Text;
using static Erpinfo.Constant.StatusEnum;

namespace Erpinfo.Services.HRMv2.Dto
{
    public class GetAllPunishmentFundsDto
    {
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
    }
    public class InputToGetAllPagingDto
    {
        public GridParam GridParam { get; set; }
        public FilterByComparision FilterByComparision { get; set; }
    }

    public class FilterByComparision
    {
        public Comparision OperatorComparison { get; set; }
        public string Value { get; set; }
    }
}
