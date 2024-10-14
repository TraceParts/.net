using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8.dataIndexing;

// documentation : https://developers.traceparts.com/v2/reference/get_v2-search-cataloglist
public static class ListOfCatalogs
{
    private static async Task<ApiResponse> GetListOfCatalogs(string token, string cultureInfo)
    {
        Console.WriteLine("📘 Warning! Any tries will be recorded in the Production data.");
        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(RootApiUrl.Url + "v2/Search/CatalogList" +
                                 "?cultureInfo=" + MyUrlEncoder.Encode(cultureInfo)
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
        Console.WriteLine(GetListOfCatalogs(opts.token, opts.cultureInfo).Result);
        return 0;
    }

    [Verb("list-of-catalogs", HelpText = "Get list of catalogs")]
    public class Options
    {
        [Value(0, MetaName = "token", Required = true,
            HelpText = "Token generated with the Tenant Unique ID and the API key")]
        public string token { get; set; }

        [Value(1, MetaName = "cultureInfo", Required = true,
            HelpText = "Language of the labels.")]
        public string cultureInfo { get; set; }
    }
}