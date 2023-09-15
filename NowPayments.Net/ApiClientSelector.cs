using NowPayments.Net.Http;

namespace NowPayments.Net
{
	internal static class ApiClientSelector
	{
		public static ISandboxHttpApiClient SandboxHttpApiClient()
		{
			return new SandboxHttpApiClient(NowPaymentsSandboxEndpointService.ServiceUrl);
		}

		public static ISandboxHttpApiClient SandboxHttpApiClient(string apiKey)
		{
			return new SandboxHttpApiClient(NowPaymentsSandboxEndpointService.ServiceUrl, apiKey);
		}
		public static ISandboxHttpApiClient SandboxHttpApiClient(string apiKey, string email, string password)
		{
			return new SandboxHttpApiClient(NowPaymentsSandboxEndpointService.ServiceUrl, apiKey, email, password);
		}

		public static IProductionHttpApiClient ProductionHttpApiClient()
		{
			return new ProductionHttpApiClient(NowPaymentsEndpointService.ServiceUrl);
		}

		public static IProductionHttpApiClient ProductionHttpApiClient(string apiKey)
		{
			return new ProductionHttpApiClient(NowPaymentsEndpointService.ServiceUrl, apiKey);
		}

		public static IProductionHttpApiClient ProductionHttpApiClient(string apiKey, string email, string password)
		{
			return new ProductionHttpApiClient(NowPaymentsEndpointService.ServiceUrl, apiKey, email, password);
		}
	}
}
