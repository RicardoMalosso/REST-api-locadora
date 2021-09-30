using Microsoft.VisualStudio.TestTools.UnitTesting;
using REST_api_locadora.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_api_locadora.Controllers.Tests
{
    [TestClass()]
    public class FilmesControllerTests
    {
        [TestMethod()]
        public void PostFilmeTest()
        {
            //arrange
            string validName = "Nome do Filme";
            Array invalidNames = [-1, null, ""];

            Assert.Fail();
        }
    }
}