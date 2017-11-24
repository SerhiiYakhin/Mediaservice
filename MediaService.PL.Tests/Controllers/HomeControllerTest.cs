#region usings

using System;
using MediaService.BLL.DTO;
using MediaService.PL.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaService.BLL.Infrastructure;
using MediaService.DAL.Entities;

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

    [TestClass]
    public class HomeControllerTest2
    {
        [TestMethod]
        public async void Download()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = await controller.Index(null);

            // Assert
            Assert.IsNotNull(result);
        }
    }

    //[TestClass]
    //public class AutoMapperTest
    //{
    //    [TestMethod]
    //    public void DtoToEntityIgnoreDefault()
    //    {
    //        // Arrange
    //        var mapper = MapperModule.GetMapper();
    //        var dirDto = new DirectoryEntryDto
    //        {
    //            Name = "Geo",
    //            Modified = DateTime.Now
    //        };

    //        var dirEntity = new DirectoryEntry
    //        {
    //            Name = "Sux",
    //            Modified = DateTime.Now.AddDays(-5),
    //            Created = DateTime.Now.AddDays(-10),
    //            Downloaded = DateTime.Now.AddDays(-10),
    //            Id = Guid.NewGuid(),
    //            NodeLevel = 5,
    //            Owner = new User { Email = "Opa@gmail.com" },
    //            OwnerId = "sdf",
    //            Parent = new DirectoryEntry { Name = "root" },
    //            ParentId = Guid.NewGuid(),
    //        };
    //        // Act
    //        var result = mapper.Map<DirectoryEntryDto, DirectoryEntry>(dirDto, dirEntity);

    //        // Assert
    //        Assert.IsNotNull(result);
    //        Assert.AreEqual(result.Name, dirDto.Name);
    //        Assert.AreEqual(result.Modified, dirDto.Modified);
    //        Assert.AreEqual(result.Created, dirEntity.Created);
    //        Assert.AreEqual(result.Downloaded, dirEntity.Downloaded);
    //        Assert.AreEqual(result.Id, dirEntity.Id);
    //        Assert.AreEqual(result.NodeLevel, dirEntity.NodeLevel);
    //        Assert.AreEqual(result.Owner, dirEntity.Owner);
    //        Assert.AreEqual(result.OwnerId, dirEntity.OwnerId);
    //        Assert.AreEqual(result.Parent, dirEntity.Parent);
    //        Assert.AreEqual(result.ParentId, dirEntity.ParentId);
    //    }
    //}
}