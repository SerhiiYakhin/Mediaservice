using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaService.PL.Controllers;

namespace MediaService.PL.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public async void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ActionResult result = await controller.Index(null);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
