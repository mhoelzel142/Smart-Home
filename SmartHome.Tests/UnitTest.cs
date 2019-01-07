using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SmartHome.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void AssertAllColorsAreNotNull()
        {
            var x = UWP.Helpers.Colors.GetAvailableColors();
            Assert.IsNotNull(x);
        }

        [TestMethod]
        public void AssertRandomColorsAreNotNull()
        {
            var x = UWP.Helpers.Colors.GetAvailableColors();
            List<string> colorList = new List<string>();


            Random rand = new Random();

            for (int i = 0; i < x.Count; i++)
            {
                int r = rand.Next(x.Count - 1);
                colorList.Add(x[r]);
            }

            Assert.AreNotEqual(colorList, x);
        }

        [TestMethod]
        public void TrySpotifyLogin()
        {
            var scopes = "user-read-private user-read-email";

            var loginInfo = new LoginInfo()
            {
                client_id = "d98db7fceb334a52a6d4ea0b2cba32c4",
                client_secret = "e9fa845e7acf4e8a8c8f74fe978e6095",
                redirect_uri = "http://localhost/"
            };

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://accounts.spotify.com/authorize" +
                                         "?response_type=code" +
                                         "&client_id=" + loginInfo.client_id +
                                         "&scope=" + scopes +
                                         "&redirect_uri=" + loginInfo.redirect_uri);
            var res = client.GetAsync(client.BaseAddress).Result.Content;
            Console.WriteLine(res);
            Assert.IsNotNull(res);
        }
    }

    public class LoginInfo
    {
        internal string client_id;

        internal string client_secret;

        internal string redirect_uri;

    }
}
