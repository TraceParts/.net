using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8.search.checkAvailabilityWith;

// documentation : https://developers.traceparts.com/v2/reference/get_v2-search-catalog-availability
public static class Catalog
{
    private static async Task<ApiResponse> GetCatalog(string token, string catalog)
    {
        Console.WriteLine("📘 Warning! Any tries will be recorded in the Production data.");
        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(RootApiUrl.Url + "v2/Search/Catalog/Availability" +
                                 "?catalog=" + MyUrlEncoder.Encode(catalog)
            ),
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
        Console.WriteLine(GetCatalog(opts.token, opts.catalog).Result);
        return 0;
    }

    [Verb("catalog", HelpText = "Check catalog availability with catalog label")]
    public class Options
    {
        [Value(0, MetaName = "token", Required = true,
            HelpText = "Token generated with the Tenant Unique ID and the API key")]
        public string token { get; set; }

        [Value(1, MetaName = "catalog", Required = true,
            HelpText = "Catalog label as you have in your own data.")]
        public string catalog { get; set; }
    }
}