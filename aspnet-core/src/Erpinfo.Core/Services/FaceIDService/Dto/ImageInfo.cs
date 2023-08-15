using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using static Erpinfo.Constant.StatusEnum;

namespace Erpinfo.Services.FaceIDService.Dto
{
    public class ImageInfo
    {
        [JsonProperty("imageVerifyId")]
        public string ImageId { get; set; }
        [JsonProperty("image")]
        public string UriImage { get; set; }
        [JsonProperty("clockStatus")]
        public string Status { get; set; }
        [JsonProperty("timeVerify")]
        public DateTime DateVerify { get; set; }

    }
    public class ImagesInfo
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("img")]
        public string Img { get; set; }
        [JsonProperty("checkInAt")]
        public DateTime CheckInAt { get; set; }
    
    }
    public class ImagesInfoUser
    {
        [JsonProperty("img")]
        public string Img { get; set; }
        [JsonProperty("checkInAt")]
        public DateTime CheckInAt { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Type { get; set; }
        public string Branch { get; set; }
        public IEnumerable<PUDto> ProjectUsers { get; set; }


    }
    public class PUDto
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<string> Pms { get; set; }
    }
    public class MappingDataImage
    {
        [JsonProperty("totalElements")]
        public int TotalCount { get; set; }
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }
        [JsonProperty("content")]
        public List<ImageInfo> Content { get; set; }
    }
   
}
