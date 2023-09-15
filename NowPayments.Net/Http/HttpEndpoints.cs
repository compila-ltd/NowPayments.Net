using Compila.Net.Utils.Http;

namespace NowPayments.Net.Http
{
	public class NowPaymentsSandboxEndpointService
	{
		public static IEndpointData ServiceUrl => new EndpointSite(EnvironmentData.SANDBOX_URL);
	}

	public static class NowPaymentsEndpointService
	{
		public static IEndpointData ServiceUrl => new EndpointSite(EnvironmentData.PRODUCTION_URL);
	}
}
