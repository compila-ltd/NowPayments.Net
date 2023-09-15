using Compila.Net.Utils.Http;

namespace NowPayments.Net
{
	internal interface IProductionHttpApiClient : IHttpApiClient
	{

	}

	internal class ProductionHttpApiClient : GenericHttpApiClient, IProductionHttpApiClient
	{
		public ProductionHttpApiClient(IEndpointData endpointData) : base(endpointData)
		{

		}
		public ProductionHttpApiClient(IEndpointData endpointData, string apiKey) : base(endpointData, apiKey)
		{
		}

		public ProductionHttpApiClient(IEndpointData endpointData, string apiKey, string email, string password) : base(endpointData, apiKey, email, password)
		{
		}
	}
}
