using System.Threading.Tasks;
using Erpinfo.Configuration.Dto;

namespace Erpinfo.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
