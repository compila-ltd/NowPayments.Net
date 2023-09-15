using System.Text.Json;

using NowPayments.Net.Objects.RequestPayload;

using RestSharp;

namespace NowPayments.Net.Requests
{
	internal static class GenericRequests
	{
		public static Request GetStatusRequest() => new Request(new RestRequest($"/status", method: Method.Get));

		public static Request AuthenticationRequest(string userName, string password) =>
			new Request(new RestRequest($"/auth", method: Method.Post).AddBody(JsonSerializer.Serialize(new AuthenticationRequestPayload { Email = userName, Password = password }), "application/json")).WithPassword();


		public static Request GetCurrenciesRequest()
		{
			return new Request(new RestRequest($"/currencies", method: Method.Get)).WithApiKey();
		}

		public static Request GetCheckedCurrenciesRequest()
		{
			return new Request(new RestRequest($"/merchant/coins", method: Method.Get)).WithApiKey();
		}

		public static Request GetAvailableFullCurrenciesRequest()
		{
			return new Request(new RestRequest($"/full-currencies", method: Method.Get)).WithApiKey();
		}

		public static Request GetMinPaymentAmountRequest(string currencyFrom, string currencyTo) => new Request(new RestRequest($"/min-amount?currency_from={currencyFrom}&currency_to={currencyTo}", method: Method.Get)).WithApiKey();

		public static Request CreatePaymentRequest(PaymentRequestPayload payment) => new Request(new RestRequest($"/payment", method: Method.Post).AddBody(JsonSerializer.Serialize(payment), "application/json")).WithApiKey();

		public static Request GetPaymentStatusRequest(long paymentId) => new Request(new RestRequest($"/payment/{paymentId}", method: Method.Get)).WithApiKey();

		public static Request GetEstimatedPriceRequest(EstimatePriceRequestPayload estimatePriceRequestPayload) => new Request(new RestRequest($"/estimate?{estimatePriceRequestPayload.ToQueryString}", method: Method.Get)).WithApiKey();
	}
}
