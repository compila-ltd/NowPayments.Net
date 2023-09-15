using System;
using System.Threading.Tasks;

using Compila.Net.Utils.Http;

using NowPayments.Net.Objects;
using NowPayments.Net.Objects.RequestPayload;
using NowPayments.Net.Requests;

using RestSharp;

namespace NowPayments.Net
{
	internal interface IHttpApiClient
	{
		Task<RestResponse<ServiceStatus>> GetStatus();
		Task<RestResponse<CurrenciesList>> GetAvailableCurrencies();
		Task<RestResponse<CheckedCurrenciesList>> GetAvailableCheckedCurrencies();
		Task<RestResponse<FullCurrenciesList>> GetAvailableFullCurrencies();
		Task<RestResponse<MinPaymentAmount>> GetMinPaymentAmount(string currencyFrom, string currencyTo);
		Task<AuthenticationStatus> Authenticate();
		Task<RestResponse<Payment>> CreatePayment(PaymentRequestPayload payment);
		Task<RestResponse<Payment>> GetPaymentStatus(long paymentId);
		Task<RestResponse<EstimatedPrice>> GetEstimatedPrice(EstimatePriceRequestPayload estimate);
	}
	internal abstract class GenericHttpApiClient : BaseHttpApiClient, IHttpApiClient
	{
		private readonly string? _apiKey;
		private readonly string? _email;
		private readonly string? _password;
		private string? BearerToken { get; set; }

		public GenericHttpApiClient(IEndpointData endpointData) : base(endpointData)
		{
		}

		public GenericHttpApiClient(IEndpointData endpointData, string apiKey) : base(endpointData)
		{
			_apiKey = apiKey ?? throw new ArgumentNullException(apiKey, nameof(apiKey));
		}

		public GenericHttpApiClient(IEndpointData endpointData, string apiKey, string email, string password) : base(endpointData)
		{
			_apiKey = apiKey ?? throw new ArgumentNullException(apiKey, nameof(apiKey));
			_email = email ?? throw new ArgumentNullException(email, nameof(email));
			_password = password ?? throw new ArgumentNullException(password, nameof(password));
		}

		protected override BaseRequest TransformHeaders(BaseRequest request)
		{
			var concreteRequest = (request as Request) ?? throw new Exception("Improve this");

			if (concreteRequest.RequireApiKey)
				request.RestRequest = request.RestRequest.AddHeader("x-api-key", _apiKey ?? string.Empty);

			if (concreteRequest.RequireAuth)
				request.RestRequest.AddHeader("Authorization", $"Bearer {BearerToken ?? string.Empty}");

			return base.TransformHeaders(request);
		}

		public async Task<RestResponse<ServiceStatus>> GetStatus()
		{
			return await ExecuteAsync<ServiceStatus>(GenericRequests.GetStatusRequest());
		}

		public async Task<RestResponse<CurrenciesList>> GetAvailableCurrencies()
		{
			return await ExecuteAsync<CurrenciesList>(GenericRequests.GetCurrenciesRequest());
		}

		public async Task<RestResponse<CheckedCurrenciesList>> GetAvailableCheckedCurrencies()
		{
			return await ExecuteAsync<CheckedCurrenciesList>(GenericRequests.GetCheckedCurrenciesRequest());
		}

		public async Task<RestResponse<FullCurrenciesList>> GetAvailableFullCurrencies()
		{
			return await ExecuteAsync<FullCurrenciesList>(GenericRequests.GetAvailableFullCurrenciesRequest());
		}

		public async Task<RestResponse<MinPaymentAmount>> GetMinPaymentAmount(string currencyFrom, string currencyTo)
		{
			return await ExecuteAsync<MinPaymentAmount>(GenericRequests.GetMinPaymentAmountRequest(currencyFrom, currencyTo));
		}

		public async Task<AuthenticationStatus> Authenticate()
		{
			var response = await ExecuteAsync<AuthToken>(GenericRequests.AuthenticationRequest(_email ?? string.Empty, _password ?? string.Empty));
			if (response.IsSuccessStatusCode)
			{
				BearerToken = response.Data?.Token ?? throw new ArgumentException("No token received.");
				return new AuthenticationStatus { IsAuthenticated = true };
			}
			return new AuthenticationStatus { IsAuthenticated = false, Message = $"{response.Data?.Message}({response.Data?.Code})" };
		}

		public async Task<RestResponse<Payment>> CreatePayment(PaymentRequestPayload payment)
		{
			return await ExecuteAsync<Payment>(GenericRequests.CreatePaymentRequest(payment));
		}

		public async Task<RestResponse<Payment>> GetPaymentStatus(long paymentId)
		{
			return await ExecuteAsync<Payment>(GenericRequests.GetPaymentStatusRequest(paymentId));
		}

		public async Task<RestResponse<EstimatedPrice>> GetEstimatedPrice(EstimatePriceRequestPayload estimate)
		{
			return await ExecuteAsync<EstimatedPrice>(GenericRequests.GetEstimatedPriceRequest(estimate));
		}
	}
}
