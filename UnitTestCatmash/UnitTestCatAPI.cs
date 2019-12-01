using CatmashAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
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
        int nb_total_cat = 100;

        [TestMethod]
        public void GetAllCats_Should_Return_All_Cats()
        {
            var result = _client.GetAsync(url + "Cats/").Result;
            List<Cat> cat_list = new List<Cat>();
            if (result.IsSuccessStatusCode)
                cat_list = JsonConvert.DeserializeObject<List<Cat>>(result.Content.ReadAsStringAsync().Result);
            int nb_cats = cat_list.Count;
            Assert.AreEqual(nb_total_cat, nb_cats);
        }

        [TestMethod]
        public void GetTwoCatRandom_Should_Return_two_Cats_randomly()
        {
            var result = _client.GetAsync(url + "Cats/random/").Result;
            List<Cat> cat_list = new List<Cat>();
            if (result.IsSuccessStatusCode)
                cat_list = JsonConvert.DeserializeObject<List<Cat>>(result.Content.ReadAsStringAsync().Result);
            int nb_cats = cat_list.Count;
            Assert.AreEqual(2, nb_cats);
            Assert.AreNotEqual(cat_list[0], cat_list[1]);
        }

        [TestMethod]
        public void GetCat_Should_Return_Cat_By_Id()
        {
            var result = _client.GetAsync(url + "Cats/MTgwODA3MA").Result;
            Cat cat = new Cat();
            if (result.IsSuccessStatusCode)
                cat = JsonConvert.DeserializeObject<Cat>(result.Content.ReadAsStringAsync().Result);

            Assert.AreEqual("http://24.media.tumblr.com/tumblr_m82woaL5AD1rro1o5o1_1280.jpg", cat.Url);
        }

        [TestMethod]
        public void CreateCat_Should_Add_Cat()
        {
            Cat cat_to_add = new Cat()
            {
                Id = "tt",
                Url = "http://24.media.tumblr.com/tumblr_m29a9d62C81r2rj8po1_500.jpg",
                Score = 0
            };
            string cat_string = JsonConvert.SerializeObject(cat_to_add);
            Cat cat_added = new Cat();
            var result = _client.PostAsync(url + "Cats/", new StringContent(cat_string, Encoding.UTF8, "application/json")).Result;

            if (result.IsSuccessStatusCode)
                cat_added = JsonConvert.DeserializeObject<Cat>(result.Content.ReadAsStringAsync().Result);

            Assert.AreEqual("http://24.media.tumblr.com/tumblr_m29a9d62C81r2rj8po1_500.jpg", cat_added.Url);
        }

        [TestMethod]
        public void UpdateCat_Should_Update_Cat()
        {
            Cat cat_to_update = new Cat()
            {
                Id = "tt",
                Url = "http://24.media.tumblr.com/tumblr_m29a9d62C81r2rj8po1_500.jpg",
                Score = 10
            };
            string cat_string = JsonConvert.SerializeObject(cat_to_update);
            Cat cat_updated = new Cat();
            var result = _client.PutAsync(url + "Cats/tt", new StringContent(cat_string, Encoding.UTF8, "application/json")).Result;

            if (result.IsSuccessStatusCode)
                cat_updated = JsonConvert.DeserializeObject<Cat>(result.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(10, cat_updated.Score);
        }

        [TestMethod]
        public void DeleteCat_Should_Delete_Cat_By_Id()
        {
            var result_delete = _client.DeleteAsync(url + "Cats/tt").Result;
            var result = _client.GetAsync(url + "Cats/tt").Result;
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public void GetAllCatFromAtelierJson_Should_Get_All_Cat_From_Json_List_On_Atelier_co()
        {
            string cat_string;
            Cat cat_added = new Cat();
            HttpResponseMessage response;
            string url_list = "https://latelier.co/data/cats.json";
            var result = _client.GetAsync(url_list).Result;
            List<Cat> cat_list = new List<Cat>();
            var res = result.Content.ReadAsStringAsync().Result;
            JObject resultJson = JObject.Parse(res);
            JArray array = resultJson["images"].Value<JArray>();
            cat_list = ((JArray)array).Select(x => new Cat()
            {
                Id = (string)x["id"],
                Url = (string)x["url"],
                Score = 0
            }).ToList();
            foreach(Cat cat in cat_list)
            {
                cat_string = JsonConvert.SerializeObject(cat);               
                response = _client.PostAsync(url + "Cats/", new StringContent(cat_string, Encoding.UTF8, "application/json")).Result;
            }
            Assert.AreEqual(100, cat_list.Count);
        }
    }
}