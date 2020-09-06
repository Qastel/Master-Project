using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using EducationalPlatform.Controllers;
using EducationalPlatform.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EducationalPlatform.Tests.Controllers
{
    [TestClass]
    public class CodebasesControllerTest
    {
        [TestMethod]
        public void TestInstructionsView()
        {

            var controller = new CodebasesController(); // create the CodebaseController object
            var result = controller.Instructions() as ViewResult; // invoke Instructions() action method and save the result returned
            Assert.AreEqual("Instructions", result.ViewName); // compare the result with the New view 

        }

        [TestMethod]
        public void TestInstructionsData()
        {
            var controller = new CodebasesController();
            var result = controller.Instructions() as ViewResult;
            var page =  result.ViewBag.Page;
            Assert.AreEqual("Instructions", page); // test if contains the ViewBag.Page = Instructions
        }

        [TestMethod]
        public void TestReturnedViewNew()
        {
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            var fakeHttpContext = new Mock<HttpContextBase>();
            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);
            CodebasesController controller = new CodebasesController();
            controller.ControllerContext = controllerContext.Object;
            var result = controller.New() as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(Codebases));
        }
    }
}
