using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.DashBoard.Dto
{
    public class ListGroupId
    {
        public long EntityId { get; set; }
        public List<long> GroupIds { get; set; }
    }
}
