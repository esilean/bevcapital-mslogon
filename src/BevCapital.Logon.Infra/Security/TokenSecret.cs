using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager.Model;
using BevCapital.Logon.Application.Gateways.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace BevCapital.Logon.Infra.Security
{
    public class TokenSecret : ITokenSecret
    {
        private readonly string _tokenSecretName;
        private readonly ILogger<TokenSecret> _logger;

        private readonly IAmazonSecretsManager _amazonSecretsManager;
        private SecretsManagerCache _secretsManagerCache;

        public TokenSecret(IOptions<TokenSettings> options,
                           ILogger<TokenSecret> logger)
        {
            _tokenSecretName = options.Value.TokenSecretName;
            _logger = logger;

            _amazonSecretsManager = new AmazonSecretsManagerClient(RegionEndpoint.SAEast1);
        }

        public async Task<string> GetSecretAsync()
        {
            try
            {
                string secret = "";
                if (_secretsManagerCache != null)
                    secret = await _secretsManagerCache.GetSecretString(_tokenSecretName);

                if (string.IsNullOrEmpty(secret))
                {
                    var request = new GetSecretValueRequest
                    {
                        SecretId = _tokenSecretName,
                        VersionStage = "AWSCURRENT"
                    };

                    var response = await _amazonSecretsManager.GetSecretValueAsync(request);
                    _secretsManagerCache = new SecretsManagerCache(_amazonSecretsManager);

                    secret = response.SecretString;
                }

                return secret;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, _tokenSecretName);
                return "";
            }
        }

        public void Dispose()
        {
            _amazonSecretsManager.Dispose();
            _secretsManagerCache.Dispose();
        }
    }
}
