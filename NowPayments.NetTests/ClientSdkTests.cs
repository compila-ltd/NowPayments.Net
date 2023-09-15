using NowPayments.Net;
using System;

using Compila.Net.Utils;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NowPayments.Net.Objects;
using System.Reflection;

namespace NowPayments.Net.Tests
{
    [TestClass()]
    [TestCategory("Execute")]
    public class ClientSdkTests
    {
        private readonly IConfiguration Configuration;
        private string apiKey, email, password, ipnCallbackUrl;

        public ClientSdkTests()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets(Assembly.GetExecutingAssembly()).AddEnvironmentVariables();
            Configuration = builder.Build();
            apiKey = Configuration["Production:Api_Key"] ?? "";
            email = Configuration["Production:Email"] ?? "";
            password = Configuration["Production:Password"] ?? "";
            ipnCallbackUrl = Configuration["Production:https://a1300cdba1f711b3fe848e94af054769.m.pipedream.net/payments"] ?? "";
        }

        [TestMethod()]
        public void GetStatusAsyncTest()
        {
            var serviceResponse = new ClientSdk().GetStatusAsync().Result;

            Assert.IsNotNull(serviceResponse);
            Assert.IsTrue(serviceResponse.Success);

            var status = serviceResponse.GetResult<ServiceStatus>();

            Assert.AreNotEqual(status.Message, "Error");
            Assert.AreNotEqual(status.Message, "No incoming message from API.");

            Console.WriteLine("Status: {0}", status.Message);
        }

        [TestMethod()]
        public void GetStatusOKAsyncTest()
        {
            var serviceResponse = new ClientSdk().GetStatusAsync().Result;

            Assert.IsNotNull(serviceResponse);
            Assert.IsTrue(serviceResponse.Success);

            var status = serviceResponse.GetResult<ServiceStatus>();

            Assert.IsTrue(status.Message == "OK");

            Console.WriteLine("Status: {0}", status.Message);
        }

        [TestMethod()]
        public void GetAvailableCurrenciesAsyncTest()
        {
            var serviceResponse = new ClientSdk(apiKey).GetAvailableCurrenciesAsync().Result;

            Assert.IsNotNull(serviceResponse);
            Assert.IsTrue(serviceResponse.Success);

            var currencies = serviceResponse.GetResult<CurrenciesList>();

            Assert.IsNotNull(currencies);
            Assert.IsNotNull(currencies.Currencies);

            //Print to console total count of currencies
            Console.WriteLine("Total currencies: {0}", currencies.Currencies.Count);
        }

        [TestMethod()]
        public void GetCheckedCurrenciesAsyncTest()
        {
            var serviceResponse = new ClientSdk(apiKey).GetCheckedCurrenciesAsync().Result;

            Assert.IsNotNull(serviceResponse);
            Assert.IsTrue(serviceResponse.Success);

            var currencies = serviceResponse.GetResult<CheckedCurrenciesList>();
            Assert.IsNotNull(currencies);
            Assert.IsNotNull(currencies.Currencies);

            //Print to console total count of currencies
            Console.WriteLine("Total currencies: {0}", currencies.Currencies.Count);
        }

        [TestMethod()]
        public void GetMinPaymentAmountAsyncTest()
        {
            var serviceResponse = new ClientSdk(apiKey).GetMinPaymentAmountAsync("bch", "usd").Result;
            Assert.IsNotNull(serviceResponse);
            Assert.IsTrue(serviceResponse.Success);
            var minPaymentAmount = serviceResponse.GetResult<MinPaymentAmount>();
            Assert.IsNotNull(minPaymentAmount);
            Assert.IsNotNull(minPaymentAmount.MinAmount);
            Assert.IsNotNull(minPaymentAmount.CurrencyFrom);
            Console.WriteLine("Min payment amount: {0} {1}", minPaymentAmount.MinAmount, minPaymentAmount.CurrencyFrom);
        }

        [TestMethod()]
        public void GetFullCurrenciesAsyncTest()
        {
            var serviceResponse = new ClientSdk(apiKey).GetFullCurrenciesAsync().Result;
            Assert.IsNotNull(serviceResponse);
            Assert.IsTrue(serviceResponse.Success);
            var currencies = serviceResponse.GetResult<FullCurrenciesList>();
            Assert.IsNotNull(currencies);
            Assert.IsNotNull(currencies.Currencies);
            Assert.IsTrue(currencies.Currencies.Count > 0);
            Console.WriteLine("Currencies count: {0}", currencies.Currencies.Count);

            //Print to console name, it position inside list and code for currency at a random position
            var random = new Random();
            var randomCurrency = currencies.Currencies[random.Next(0, currencies.Currencies.Count)];
            Console.WriteLine("Currency name: {0} \n Currency position: {1} \n Currency code: {2}", randomCurrency.Name, currencies.Currencies.IndexOf(randomCurrency), randomCurrency.Code);
        }

        [TestMethod()]
        public void EstimatePriceAsyncTest()
        {
            var serviceResponse = new ClientSdk(apiKey).EstimatePriceAsync(1, "btc", "usd").Result;
            Assert.IsNotNull(serviceResponse);
            Assert.IsTrue(serviceResponse.Success);
            var estimatePrice = serviceResponse.GetResult<EstimatedPrice>();
            Assert.IsNotNull(estimatePrice);
            Assert.IsNotNull(estimatePrice.EstimatedAmount);
            Assert.IsNotNull(estimatePrice.CurrencyFrom);
            Assert.IsNotNull(estimatePrice.CurrencyTo);
            Console.Write("Estimate price for {1}: {0} {2}", estimatePrice.EstimatedAmount, estimatePrice.CurrencyFrom, estimatePrice.CurrencyTo);
        }
    }
}