using System.IO;
using System.Xml.Serialization;
using Imgur.Service.Upload.ResponseStructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PrimeIM.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for ImgurUploaderTest and is intended
    ///to contain all ImgurUploaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ImgurUploaderTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for PostToImgur
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Flikr.Service.Upload.dll")]
        public void PostToImgurTest()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImgurResponse));
            const string imgFilePath = @"C:\Documents and Settings\bmj\Desktop\imgurresponse.xml";
            Stream reader = new FileStream(imgFilePath, FileMode.Open);
            var xbbb = serializer.Deserialize(reader);
        }
    }
}
