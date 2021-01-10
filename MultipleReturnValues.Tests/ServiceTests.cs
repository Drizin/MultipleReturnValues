using MultipleReturnValues.Services;
using NUnit.Framework;

namespace MultipleReturnValues.Tests
{
    public class ServiceTests
    {
        UserService svc;

        [OneTimeSetUp]
        public void Setup()
        {
            svc = new UserService();
        }

        [Test, Order(0)]
        public void TestCreateUserOK()
        {
            var (user, error) = svc.CreateUser(new UserService.CreateUserDTO() { FirstName = "Rick", LastName = "Drizin", UserName = "drizin" });

            Assert.NotNull(user);
            Assert.IsNull(error);
        }

        [Test, Order(1)]
        public void TestCreateUserNotAvailable()
        {

            var (user, error) = svc.CreateUser(new UserService.CreateUserDTO() { FirstName = "Rick", LastName = "Drizin", UserName = "drizin" });

            Assert.IsNull(user);
            Assert.NotNull(error);
            Assert.That(error == UserService.CreateUserError.USERNAME_NOT_AVAILABLE);

        }

    }
}