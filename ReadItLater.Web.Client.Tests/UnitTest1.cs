using NUnit.Framework;
using ReadItLater.Web.Client.Pages;
using System;
using System.Net.Http;

namespace ReadItLater.Web.Client.Tests
{
    public class Tests
    {
        private readonly Menu menu;
        //private readonly 

        public Tests()
        {
            menu = new Menu();
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //Arrange
            var id = Guid.NewGuid();

            //Act
            menu.FolderChosen(id);

            //Assert
            Assert.Pass();
        }
    }
}