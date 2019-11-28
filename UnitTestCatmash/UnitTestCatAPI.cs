using CatmashAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace UnitTestCatmash
{
    [TestClass]
    public class UnitTestCatAPI
    {
        HttpClient _client = new HttpClient();
        string url = "http://localhost:5050/api/";

        [TestMethod]
        public void GetAllCats_Should_Return_All_Cats()
        {
            
        }

       
    }
}
