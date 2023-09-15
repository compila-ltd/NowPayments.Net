using System.Text.Json;

using NowPayments.Net.Objects.RequestPayload;

using RestSharp;

namespace NowPayments.Net.Requests
{
	internal static class SandboxRequests
	{
		public static RestRequest CreatePaymentRequest(PaymentRequestPayload_Sandbox payment)
		{
			return new RestRequest($"/payment", method: Method.Post)
				.AddBody(JsonSerializer.Serialize(payment), "application/json");
		}
	}
}
