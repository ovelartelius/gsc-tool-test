using Google.Apis.Auth.OAuth2;
using Google.Apis.SearchConsole.v1;
using Google.Apis.Services;
using Microsoft.Extensions.Options;

namespace GscToolTest.GoogleSearchConsole
{
    public interface ISearchConsoleServiceFactory
    {
        SearchConsoleService Create();
    }

    public class SearchConsoleServiceFactory : ISearchConsoleServiceFactory
    {
        private readonly GoogleSettings _googleSettings;
        private readonly BaseClientService.Initializer _initializer;

        public SearchConsoleServiceFactory(IOptions<GoogleSettings> settings)
        {
            _googleSettings = settings.Value;

            var clientSecretsFilePath = "client_secret.json";

            UserCredential credential = AsyncHelper.RunSync<UserCredential>(() => GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.FromStream(File.OpenRead(clientSecretsFilePath)).Secrets,
                new[] { SearchConsoleService.Scope.Webmasters, SearchConsoleService.Scope.WebmastersReadonly },
                "user",
                CancellationToken.None));

            _initializer = new BaseClientService.Initializer();
            _initializer.HttpClientInitializer = credential;
            _initializer.ApplicationName = _googleSettings.ApplicationName;
        }

        /// <inheritDoc />
        public virtual SearchConsoleService Create()
        {
            var service = new SearchConsoleService(_initializer);

            return service;
        }
    }
}
