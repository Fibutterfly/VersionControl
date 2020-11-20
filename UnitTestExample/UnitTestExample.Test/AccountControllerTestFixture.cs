using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestExample.Controllers;

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
            TestCase("sZuPeRjeLsZo123",true)
        ]
        public void TestValidatePassword(string dummypass, bool exceptedResult)
        {
            var actualResult = accountController.ValidatePassword(dummypass);
            Assert.That(actualResult, Is.EqualTo(exceptedResult));
        }
    }
}
