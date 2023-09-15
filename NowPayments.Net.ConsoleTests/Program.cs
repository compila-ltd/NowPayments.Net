// See https://aka.ms/new-console-template for more information
using Compila.Net.Utils;

using NowPayments.Net;
using NowPayments.Net.Objects;

var client = new ClientSdk();

var statusResponse = await client.GetStatusAsync();

var serverStatus = statusResponse.Success ? statusResponse.GetResult<ServiceStatus>() : throw new Exception("Status can not be retrieved.");

Console.WriteLine("Server status is {0}", serverStatus.Message);