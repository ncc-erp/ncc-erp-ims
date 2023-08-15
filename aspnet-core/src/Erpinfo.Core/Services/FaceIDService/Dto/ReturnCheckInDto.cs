using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Services.FaceIDService.Dto
{
    public class ReturnCheckInDto
    {
        [JsonProperty("firstName")]
        public string FristName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("imageVerifyId")]
        public string ImageVerifyId { get; set; }
        [JsonProperty("showMessage")]
        public bool ShowMessage { get; set; }
    }
}
