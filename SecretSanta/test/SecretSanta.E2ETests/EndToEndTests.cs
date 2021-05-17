using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlaywrightSharp;

namespace SecretSanta.E2ETests
{
    [TestClass]
    public class EndToEndTests
    {
        [TestMethod]
        public async Task LandOnHomepage()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Firefox.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            var response = await page.GoToAsync("https://localhost:5001");

            Assert.IsTrue(response.Ok);

            var headerContent = await page.GetTextContentAsync("//html/body/header/div/a");
            Assert.AreEqual("Secret Santa", headerContent);
        }
    }
}
