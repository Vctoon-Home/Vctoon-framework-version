using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Abp.Localization.Avalonia;

namespace VctoonClient.Handlers;

public class VctoonHttpClientHandler : DelegatingHandler, ITransientDependency
{
    private readonly ILocalizationManager _localizationManager;

    public VctoonHttpClientHandler(ILocalizationManager localizationManager)
    {
        _localizationManager = localizationManager;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        return await base.SendAsync(request, cancellationToken);
    }
}