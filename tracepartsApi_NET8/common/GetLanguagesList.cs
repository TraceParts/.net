using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8.common;

// documentation : https://developers.traceparts.com/v2/reference/get_v2-supportedlanguages
public static class GetLanguagesList
{
    private static async Task<ApiResponse> GetGetLanguagesList(string token)
    {
        Console.WriteLine("📘 Warning! Any tries will be recorded in the Production data.");
        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(RootApiUrl.Url + "v2/SupportedLanguages"),
            Headers =
            {
                { "accept", "application/json" },
                { "authorization", "Bearer " + token }
            }
        };
        return new ApiResponse(await client.SendAsync(request));
    }


    public static int Run(Options opts)
    {
        Console.WriteLine(GetGetLanguagesList(opts.token).Result);
        return 0;
    }

    [Verb("get-languages-list", HelpText = "Get available languages list for your token")]
    public class Options
    {
        [Value(0, MetaName = "token", Required = true,
            HelpText = "Token generated with the Tenant Unique ID and the API key")]
        public string token { get; set; }
    }
}