using System;
using System.Threading.Tasks;

using Compila.Net.Utils.ServiceResponses;

using NowPayments.Net.Objects;
using NowPayments.Net.Objects.RequestPayload;

namespace NowPayments.Net
{
	public class ClientSdk
	{
		private readonly IProductionHttpApiClient ApiClient;

		#region Constructors

		public ClientSdk()
		{
			ApiClient = ApiClientSelector.ProductionHttpApiClient();
		}

		public ClientSdk(string apiKey)
		{
			ApiClient = ApiClientSelector.ProductionHttpApiClient(apiKey);
		}

		public ClientSdk(string apiKey, string email, string password)
		{
			ApiClient = ApiClientSelector.ProductionHttpApiClient(apiKey, email, password);
		}

		#endregion
		#region Status and Authentication

		public async Task<ServiceBaseResponse> GetStatusAsync()
		{
			var response = await ApiClient.GetStatus();

			if (response.IsSuccessStatusCode)
			{
				return new ServiceOkResponse<ServiceStatus>(response.Data ?? new ServiceStatus { Message = "No incoming message from API." });
			}

			return new ServiceErrorResponse(response.ErrorMessage ?? "Error", (int)response.StatusCode);
		}

		public async Task<ServiceBaseResponse> AuthenticateAsync()
		{
			var response = await ApiClient.Authenticate();
			if (response.IsAuthenticated)
			{
				return new ServiceOkResponse<AuthenticationStatus>(response);
			}

			return new ServiceErrorResponse($"Error: {response.Message}", 0);
		}

		#endregion
		#region Currencies

		public async Task<ServiceBaseResponse> GetAvailableCurrenciesAsync()
		{
			var response = await ApiClient.GetAvailableCurrencies();

			if (response.IsSuccessStatusCode)
			{
				return new ServiceOkResponse<CurrenciesList>(response.Data ?? throw new Exception(response.ErrorMessage));
			}

			return new ServiceErrorResponse(response.ErrorMessage ?? "Error", (int)response.StatusCode);
		}

		public async Task<ServiceBaseResponse> GetCheckedCurrenciesAsync()
		{
			var response = await ApiClient.GetAvailableCheckedCurrencies();

			if (response.IsSuccessStatusCode)
			{
				return new ServiceOkResponse<CheckedCurrenciesList>(response.Data ?? throw new Exception(response.ErrorMessage));
			}

			return new ServiceErrorResponse(response.ErrorMessage ?? "Error", (int)response.StatusCode);
		}

		public async Task<ServiceBaseResponse> GetFullCurrenciesAsync()
		{
			var response = await ApiClient.GetAvailableFullCurrencies();
			if (response.IsSuccessStatusCode)
			{
				return new ServiceOkResponse<FullCurrenciesList>(response.Data ?? throw new Exception(response.ErrorMessage));
			}
			return new ServiceErrorResponse(response.ErrorMessage ?? "Error", (int)response.StatusCode);
		}

		#endregion
		#region Payments

		public async Task<ServiceBaseResponse> GetMinPaymentAmountAsync(string currencyFrom, string currencyTo)
		{
			var response = await ApiClient.GetMinPaymentAmount(currencyFrom, currencyTo);
			if (response.IsSuccessStatusCode)
			{
				return new ServiceOkResponse<MinPaymentAmount>(response.Data ?? throw new Exception(response.ErrorMessage));
			}
			return new ServiceErrorResponse(response.ErrorMessage ?? "Error", (int)response.StatusCode);
		}

		public async Task<ServiceBaseResponse> GetPaymentStatusAsync(long paymentId)
		{
			var response = await ApiClient.GetPaymentStatus(paymentId);
			if (response.IsSuccessStatusCode)
			{
				return new ServiceOkResponse<Payment>(response.Data ?? throw new Exception(response.ErrorMessage));
			}
			return new ServiceErrorResponse(response.ErrorMessage ?? "Error", (int)response.StatusCode);
		}

		public async Task<ServiceBaseResponse> CreatePaymentAsync(decimal priceAmount, string priceCurrency, decimal payAmount, string payCurrency, string orderId, string? orderDescription, string ipnCallback, bool isFeePaidByUser = false)
		{
			var paymentPayload = new PaymentRequestPayload
			{
				IPNCallbackUrl = ipnCallback,
				OrderId = orderId,
				OrderDescription = orderDescription,
				PriceAmount = priceAmount,
				PriceCurrency = priceCurrency,
				PayAmount = payAmount,
				PayCurrency = payCurrency,
				IsFeePaidByUser = isFeePaidByUser
			};
			var response = await ApiClient.CreatePayment(paymentPayload);
			if (response.IsSuccessStatusCode)
			{
				return new ServiceOkResponse<Payment>(response.Data ?? throw new Exception(response.ErrorMessage));
			}
			return new ServiceErrorResponse(response.ErrorMessage ?? "Error", (int)response.StatusCode);
		}

		public async Task<ServiceBaseResponse> EstimatePriceAsync(decimal amount, string currencyFrom, string currencyTo)
		{
			var paymentPayload = new EstimatePriceRequestPayload
			{
				Amount = amount,
				CurrencyFrom = currencyFrom,
				CurrencyTo = currencyTo
			};

			var response = await ApiClient.GetEstimatedPrice(paymentPayload);
			if (response.IsSuccessStatusCode)
			{
				return new ServiceOkResponse<EstimatedPrice>(response.Data ?? throw new Exception(response.ErrorMessage));
			}
			return new ServiceErrorResponse(response.ErrorMessage ?? "Error", (int)response.StatusCode);
		}

		/*
		public async Task<ServiceBaseResponse> GetPaymentListAsync(PaymentListPayload payload)
		{
			var response = await ApiClient.GetPaymentList(payload);
			if (response.IsSuccessStatusCode)
			{
				return new ServiceOkResponse<PaymentList>(response.Data ?? throw new Exception(response.ErrorMessage));
			}
			return new ServiceErrorResponse(response.ErrorMessage ?? "Error", (int)response.StatusCode);
		}*/

		#endregion
	}
}
