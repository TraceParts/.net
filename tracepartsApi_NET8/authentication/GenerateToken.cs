using System.Net.Http.Headers;
using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8.authentication;

// documentation : https://developers.traceparts.com/v2/reference/post_v2-requesttoken
public static class GenerateToken
{
    private static async Task<ApiResponse> PostGenerateToken(string tenantUid, string apiKey)
    {
        Console.WriteLine("📘 Warning! Any tries will be recorded in the Production data.");
        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(RootApiUrl.Url + "v2/RequestToken"),
            Headers =
            {
                { "accept", "application/json" }
            },
            Content = new StringContent(
                "{\"tenantUid\":\"" + tenantUid + "\"," +
                "\"apiKey\":\"" + apiKey + "\"}")
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/*+json")
                }
            }
        };
        return new ApiResponse(await client.SendAsync(request));
    }


    public static int Run(Options opts)
    {
        Console.WriteLine(PostGenerateToken(opts.tenantUid, opts.apiKey).Result);
        return 0;
    }

    [Verb("generate-token", HelpText = "Generate a token that will be used for all other requests")]
    public class Options
    {
        [Value(0, MetaName = "tenantUid", Required = true,
            HelpText = "Tenant Unique ID provided in the email giving you access to our API")]
        public string tenantUid { get; set; }

        [Value(1, MetaName = "apiKey", Required = true,
            HelpText = "API key provided in the email giving you access to our API")]
        public string apiKey { get; set; }
    }
}