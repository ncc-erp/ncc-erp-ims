using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Services.KomuService.KomuDto
{
    public class KomuUserDto
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("userid")]
        public long? KomuUserId { get; set; }
    }
}
