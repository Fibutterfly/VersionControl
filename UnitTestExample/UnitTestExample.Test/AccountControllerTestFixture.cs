using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestExample.Controllers;
using System.Activities;
using UnitTestExample.Abstractions;
using UnitTestExample.Entities;
using UnitTestExample.Services;

namespace UnitTestExample.Test
{
    public class AccountControllerTestFixture
    {
        AccountController accountController = new AccountController();
        [
            Test,
            TestCase("abcd1234", false),
            TestCase("irf@uni-corvinus", false),
            TestCase("irf.uni-corvinus.hu", false),
            TestCase("irf@uni-corvinus.hu", true)
        ]
        public void TestValidateEmail(string email, bool expectedResult)
        {           
            // Act
            var actualResult = accountController.ValidateEmail(email);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
        [
            Test,
            TestCase("nInCsSzAm",false),
            TestCase("NINCSKISBETU123",false),
            TestCase("nincsnagybetu123",false),
            TestCase("rOvId12",false),
            TestCase("sZuPeRjeLsZo123",true),
            TestCase("123JoNaKkEnElEnNiE",true),
            TestCase("NaGyKeZdEs123", true)
        ]
        public void TestValidatePassword(string dummypass, bool exceptedResult)
        {
            var actualResult = accountController.ValidatePassword(dummypass);
            Assert.That(actualResult, Is.EqualTo(exceptedResult));
        }
        [
            Test,
            TestCase("irf@uni-corvinus.hu", "Abcd1234"),
            TestCase("illes@filep.hu", "sZuPeRjeLsZo123"),
            TestCase("ez@cim.hu","123JoNaKkEnElEnNiE"),
        ]
        public void TestRegisterHappyPath(string email, string password)
        {
            var actualResult = accountController.Register(email, password);
            Assert.That(email, Is.EqualTo(actualResult.Email));
            Assert.That(password, Is.EqualTo(actualResult.Password));
            Assert.That(Guid.Empty, Is.Not.EqualTo(actualResult.ID));
        }
        [
            Test,
            TestCase("irf@uni-corvinus", "Abcd1234"),
            TestCase("irf.uni-corvinus.hu", "Abcd1234"),
            TestCase("irf@uni-corvinus.hu", "abcd1234"),
            TestCase("irf@uni-corvinus.hu", "ABCD1234"),
            TestCase("irf@uni-corvinus.hu", "abcdABCD"),
            TestCase("irf@uni-corvinus.hu", "Ab1234"),
         ]
        public void TestRegisterValidateException(string email, string password)
        {
            try
            {
                var actualResult = accountController.Register(email,password);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<ValidationException>());
            }
        }
    }
}
