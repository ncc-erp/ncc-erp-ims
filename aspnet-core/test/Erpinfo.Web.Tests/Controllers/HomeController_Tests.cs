using System.Threading.Tasks;
using Erpinfo.Models.TokenAuth;
using Erpinfo.Web.Controllers;
using Shouldly;
using Xunit;

namespace Erpinfo.Web.Tests.Controllers
{
    public class HomeController_Tests: ErpinfoWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}