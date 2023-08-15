using Abp.UI;
using Erpinfo.HRMv2.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Public
{
    public class PublicAppService: ErpinfoAppServiceBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PublicAppService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public GetConnectDto CheckConnect()
        {
            CheckSecurityCode();
            return new GetConnectDto
            {
                IsConnected = true,
                Message = "Connected"
            };
        }

        public void CheckSecurityCode()
        {
            var securityCode = Constant.EntityConstant.SecurityCode;
            var header = _httpContextAccessor.HttpContext.Request.Headers;
            var securityCodeHeader = header["securityCode"];
            if (string.IsNullOrEmpty(securityCodeHeader))
            {
                securityCodeHeader = header["X-Secret-Key"];
            }
            if (securityCode != securityCodeHeader)
                throw new UserFriendlyException($"SecretCode does not match! IMS secretCode: {securityCode.Substring(securityCode.Length - 3)} != {securityCodeHeader}"); ;
  
        }
    }
}
