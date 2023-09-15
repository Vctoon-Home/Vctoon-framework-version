using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using VctoonClient.Oidc;

internal class Program
{
    [STAThread]
    public static async Task Main(string[] args)
    {
        if (args.Any())
            await ProcessCallback(args[0]);
    }
    private static async Task ProcessCallback(string args)
    {
        var response = new AuthorizeResponse(args);
        if (!string.IsNullOrWhiteSpace(response.State))
        {
            Console.WriteLine($"Found state: {response.State}");
            var callbackManager = new CallbackManager(response.State);
            await callbackManager.RunClient(args);
        }
        else
        {
            Console.WriteLine("Error: no state on response");
        }
    }
}