using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Services.FaceIDService.Dto
{
    public class DecideImageIsMeDto
    {
        [JsonProperty("verifiedImageId")]
        public string ImageId { get; set; }
        [JsonProperty("isAccept")]
        public bool IsApproved { get; set; }
    }
}
