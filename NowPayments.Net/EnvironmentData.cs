namespace NowPayments.Net
{
	static class EnvironmentData
	{
		public const string SANDBOX_URL = "https://api-sandbox.nowpayments.io/v1";
		public const string PRODUCTION_URL = "https://api.nowpayments.io/v1";
	}

	public enum Network
	{
		Production,
		Sandbox
	}
}
