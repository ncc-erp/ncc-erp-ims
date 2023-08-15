using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Sessions
{
   public interface IErpSession: IAbpSession
    {
        public long? GroupId { get; }
        public string Role { get; }
    }
}
