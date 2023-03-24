using Nancy;
using Nancy.Testing;
using LiftPassPricing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace LiftPassPricingTests
{
    [TestClass]
    public class PricesTest
    {
        private static Prices prices;
        private static Browser browser;

        [ClassInitialize]
        public static void SetupClass(TestContext context)
        {
            prices = new Prices();
            browser = new Browser(with => with.Module(prices));
        }

        [ClassCleanup]
        public static void CleanupClass()
        {
            prices.connection.Close();
        }

        [TestMethod]
        public void DoesSomething()
        {
            var result = browser.Get("/prices", with =>
            {
                with.Query("type", "1jour");
                with.Query("age", "23");
                with.Query("date", "2019-02-18");
                with.HttpRequest();
            });

            result.Result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Result.ContentType.Should().Be("application/json");

            Response json = result.Result.Body.DeserializeJson<Response>();
            json.cost.Should().Be(35);
        }
    }

    class Response
    {
        public int cost { get; set; }
    }
}
