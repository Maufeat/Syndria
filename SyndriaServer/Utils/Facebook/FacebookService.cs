using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

public interface IFacebookService
{
    Task<DebugToken> VerifyAccessToken(string accessToken);
    Task PostOnWallAsync(string accessToken, string message);
}

public class FacebookService : IFacebookService
{
    private readonly IFacebookClient _facebookClient;

    public FacebookService(IFacebookClient facebookClient)
    {
        _facebookClient = facebookClient;
    }

    public async Task<DebugToken> VerifyAccessToken(string accessToken)
    {
        var result = await _facebookClient.GetAsync<dynamic>(
            accessToken, "debug_token", $"input_token={accessToken}");

        if (result == null)
        {
            return new DebugToken();
        }

        var data = result.data;

        var debugToken = new DebugToken
        {
            AppId = data.app_id,
            Type = data.type,
            Application = data.application,
            IsValid = data.is_valid,
            UserId = data.user_id
        };
        return debugToken;
    }
    
    public async Task PostOnWallAsync(string accessToken, string message)
        => await _facebookClient.PostAsync(accessToken, "me/feed", new { message });
}