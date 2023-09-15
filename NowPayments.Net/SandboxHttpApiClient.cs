using Compila.Net.Utils.Http;

namespace NowPayments.Net
{
	internal interface ISandboxHttpApiClient : IHttpApiClient
	{
		
	}

	internal class SandboxHttpApiClient : GenericHttpApiClient, ISandboxHttpApiClient
	{
		public SandboxHttpApiClient(IEndpointData endpointData) : base(endpointData) { }
		public SandboxHttpApiClient(IEndpointData endpointData, string apiKey) : base(endpointData, apiKey) { }
		public SandboxHttpApiClient(IEndpointData endpointData, string apiKey, string email, string password) : base(endpointData, apiKey, email, password) { }
	}
}
