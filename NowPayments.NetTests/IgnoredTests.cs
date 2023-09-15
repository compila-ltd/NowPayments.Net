using System;
using System.Reflection;

using Compila.Net.Utils;
using Compila.Net.Utils.ServiceResponses;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NowPayments.Net;
using NowPayments.Net.Objects;

namespace NowPayments.NetTests
{
    [TestClass()]
    [TestCategory("Ignore")]
    public class IgnoredTests
    {
        private readonly IConfiguration Configuration;
        private string apiKey, email, password, ipnCallbackUrl;

        public IgnoredTests()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets(Assembly.GetExecutingAssembly()).AddEnvironmentVariables();
            Configuration = builder.Build();
            apiKey = Configuration["Production:Api_Key"] ?? "";
            email = Configuration["Production:Email"] ?? "";
            password = Configuration["Production:Password"] ?? "";
            ipnCallbackUrl = Configuration["Production:https://a1300cdba1f711b3fe848e94af054769.m.pipedream.net/payments"] ?? "";
        }

        [TestMethod()]
        public void AuthenticateAsyncTest()
        {
            var serviceResponse = new ClientSdk(apiKey, email, password).AuthenticateAsync().Result;
            Assert.IsNotNull(serviceResponse);
            Assert.IsTrue(serviceResponse.Success);
            var authStatus = serviceResponse.GetResult<AuthenticationStatus>();
            Assert.IsTrue(authStatus.IsAuthenticated);
        }

        [TestMethod()]
        public void FailAuthenticateAsyncTest()
        {
            var serviceResponse = new ClientSdk(apiKey, "wronguser@example-email.com", "wrongPasswordTesting").AuthenticateAsync().Result;
            Assert.IsNotNull(serviceResponse);
            Assert.IsFalse(serviceResponse.Success);

            Console.WriteLine("Message: {0}", ((ServiceErrorResponse)serviceResponse).Message);
        }

        [TestMethod()]
        public void CreatePaymentAsyncTest()
        {
            var serviceResponse = new ClientSdk(apiKey).CreatePaymentAsync(182, "usd", 0.3m, "bch", RandomStringGenerator.GenerateRandomToken(7), "Test Order", ipnCallbackUrl, true).Result;
            Assert.IsNotNull(serviceResponse);
            Assert.IsTrue(serviceResponse.Success);
            var payment = serviceResponse.GetResult<Payment>();
            Assert.IsNotNull(payment);
            Assert.IsNotNull(payment.PaymentId);
            Assert.IsNotNull(payment.PayCurrency);
            Assert.IsNotNull(payment.PayAmount);
            Assert.IsNotNull(payment.PayAddress);

            Console.WriteLine("Payment id: {0} \n Payment address: {1} \n Pay amount: {2}", payment.PaymentId, payment.PayAddress, payment.PayAmount);
        }

        [TestMethod()]
        public void GetPaymentStatusAsyncTest()
        {
            _ = long.TryParse(Configuration["Production:ValidPaymentId"], out var paymentId);
            var serviceResponse = new ClientSdk(apiKey).GetPaymentStatusAsync(paymentId).Result;
            Assert.IsNotNull(serviceResponse);
            Assert.IsTrue(serviceResponse.Success);
            var paymentStatus = serviceResponse.GetResult<Payment>();
            Assert.IsNotNull(paymentStatus);
            Assert.IsNotNull(paymentStatus.PaymentStatus);
            Assert.IsNotNull(paymentStatus.PayCurrency);

            Console.WriteLine("Payment status: {0}", paymentStatus.PaymentStatus);
        }
    }
}
