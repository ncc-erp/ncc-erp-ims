using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erpinfo.QuickNew
{
    public interface IQuickNewsAppService : IApplicationService
    {
        Task<CreateQuickNewsDto> Create(CreateQuickNewsDto input);

        Task<UpdateQuickNewsDto> Update(UpdateQuickNewsDto input);

        Task<PagedResultDto<GetAllQuickNewsDto>> GetAllQuickNews(AllQuickNewsInputDto input);

        Task<PagedResultDto<GetContentConvertQuickNew>> ChangeFormatTextQuickNews(AllQuickNewsInputDto input);

        IEnumerable<GetAllQuickNewsDto> GetListQuickNew();
    }
}