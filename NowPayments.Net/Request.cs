using Compila.Net.Utils.Http;

using RestSharp;

namespace NowPayments.Net
{
	internal class Request : BaseRequest
	{
		public bool RequireApiKey { get; set; } = false;
		public bool RequireAuth { get; set; } = false;
		public bool RequirePassword { get; set; } = false;

		public Request(RestRequest restRequest) : base(restRequest)
		{

		}

		public Request WithApiKey()
		{
			RequireApiKey = true;
			return this;
		}

		public Request WithAuth()
		{
			RequireAuth = true;
			return this;
		}

		public Request WithPassword()
		{
			RequirePassword = true;
			return this;
		}
	}
}
