#region usings

using MediaService.PL.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace MediaService.PL.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public async void Index()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = await controller.Index(null);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}