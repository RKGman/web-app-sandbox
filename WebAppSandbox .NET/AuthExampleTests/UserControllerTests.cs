using AuthExample.Auth;
using AuthExample.Models;
using AuthService.Controllers;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace AuthExampleTests
{
    [TestClass]
    [TestCategory("L0")]
    [TestCategory("Gated")]
    [TestCategory("Parallel")] // TODO: Determine how / why these properties matter...
    public class UserControllerTests
    {

        private readonly UserController _userController;

        [TestInitialize]
        public void TestInitialize()
        {
        }

        public UserControllerTests()
        {
            // TODO: Interface things so that we can actually test things!!!
            // TODO: Will want some adapters and dependency injection to properly test controllers... Since all of our work is being done in the controller it's not ideal for testing at the moment.

            //Microsoft.Extensions.Configuration.IConfiguration configuration = null;

            //Mock<ApplicationDbContext> mockDbContext = new Mock<ApplicationDbContext>();
            //Mock<RoleManager<IdentityRole>> mockRoleManager = new Mock<RoleManager<IdentityRole>>();
            //Mock<UserManager<IdentityUser>> mockUserManager = new Mock<UserManager<IdentityUser>>();

            //_userController = new UserController(configuration, mockDbContext.Object, mockRoleManager.Object, mockUserManager.Object);

            //_userController = new UserController(null, null, null, null);
        }

        [TestMethod]

        public void UserController_Bogus_Test_Pass()
        {
            // Here is some test guaranteed to pass.

            var test = 1 + 1; 

            Assert.AreEqual(2, test);
        }

        [TestMethod]

        public void UserController_Bogus_Test_Fail()
        {
            // Here is some test guaranteed to fail.

            var test = 1 + 1;

            Assert.AreEqual(11, test);
        }
    }
}