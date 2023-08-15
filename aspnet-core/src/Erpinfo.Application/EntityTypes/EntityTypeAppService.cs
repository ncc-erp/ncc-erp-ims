using Erpinfo.Entities;
using Erpinfo.EntityTypes.Dto;
using Erpinfo.Extension;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization;
using Erpinfo.Authorization;

namespace Erpinfo.EntityTypes
{
    [AbpAuthorize]
    public class EntityTypeAppService : ErpinfoAppServiceBase
    {
        [HttpGet]
        [AbpAuthorize(PermissionNames.EntityType.View)]
        public async Task<List<EntityTypeDto>> GetByEntity(string entity)
        {
            return await WorkScope.GetAll<EntityType>().Where(s => s.Entity == entity)
                .Select(s => new EntityTypeDto
                {
                    Id = s.Id,
                    Entity = s.Entity,
                    TypeName = s.TypeName
                }).OrderBy(x => x.TypeName).ToListAsync();
        }

        [AbpAuthorize(PermissionNames.EntityType.View)]
        public object GetEntityName()
        {
            return new
            {
                Constant.EntityConstant.NewEntity,
                Constant.EntityConstant.PolicyEntity,
                Constant.EntityConstant.EventEntity,
                Constant.EntityConstant.GuildLineEntity,
                Constant.EntityConstant.BlogEntity,
            };
        }
        [AbpAuthorize(PermissionNames.EntityType.Edit)]
        public async Task Save(EntityInputDto input)
        {
            if(ErpSession.GroupId.Value == 3 || ErpSession.GroupId.Value == 4)
            {
                var entityType = ObjectMapper.Map<EntityType>(input);
                await WorkScope.GetRepo<EntityType>().InsertOrUpdateAsync(entityType);
            }
        }
    }
}
