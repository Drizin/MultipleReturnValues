using MultipleReturnValues.Entities;
using MultipleReturnValues.Services;
using NUnit.Framework;

namespace MultipleReturnValues.Tests
{
    public class ResultOrErrorTests
    {

        enum CommandErrorEnum
        {
            [System.ComponentModel.Description("An error 1 occurred")]
            Error1,
            Error2,
            Error3,
        }
        MaybeError<CommandErrorEnum> Command1(bool success)
        {
            if (success)
                return null;
            else
                return (CommandErrorEnum.Error1);
        }
        [Test(Description = "Tests a method which may return an error or if success doesnt return anything - using MaybeError<TError>")]
        public void Test1()
        {
            var error = Command1(success: true);
            Assert.That(error == null);
            error = Command1(success: false);
            Assert.That(error != null);
            Assert.That(error == CommandErrorEnum.Error1);
            Assert.That(error.ErrorMessage == "An error 1 occurred");
        }

        
        ErrorResult<CommandErrorEnum> Command2(bool success)
        {
            if (success)
                return null;
            else
                return (CommandErrorEnum.Error1);
        }
        [Test(Description = "Tests a method which may return an error or if success doesnt return anything - using ErrorResult<TError>")]
        public void Test2()
        {
            var error = Command2(success: true);
            Assert.That(error == null);
            error = Command2(success: false);
            Assert.That(error != null);
            Assert.That(error == CommandErrorEnum.Error1);
            Assert.That(error.ErrorMessage == "An error 1 occurred");
        }

        ResultOrError<User, CommandErrorEnum> Command3(bool success)
        {
            if (success)
                return (new User() { UserName = "rick" }, null);
            else
                return (null, CommandErrorEnum.Error1);
        }
        [Test(Description = "Tests a method which may return an error or if success returns an entity - using ResultOrError<TSuccess, TError>")]
        public void Test3()
        {
            var (user, error) = Command3(success: true);
            Assert.That(error == null);
            Assert.That(user != null);
            (user, error) = Command3(success: false);
            Assert.That(error != null);
            Assert.That(error == CommandErrorEnum.Error1);
            Assert.That(error.ErrorMessage == "An error 1 occurred");
        }



    }
}