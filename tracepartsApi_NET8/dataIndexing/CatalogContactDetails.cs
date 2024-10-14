using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8.dataIndexing;

// documentation : https://developers.traceparts.com/v2/reference/get_v1-contact-catalog
public static class CatalogContactDetails
{
    private static async Task<ApiResponse> GetCatalogContactDetails(string token, string classificationCode)
    {
        Console.WriteLine("📘 Warning! Any tries will be recorded in the Production data.");
        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(RootApiUrl.Url + "v1/Contact/Catalog" +
                                 "?classificationCode=" + MyUrlEncoder.Encode(classificationCode)
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
        Console.WriteLine(GetCatalogContactDetails(opts.token, opts.classificationCode)
            .Result);
        return 0;
    }

    [Verb("catalog-contact-details", HelpText = "Get catalog contact details")]
    public class Options
    {
        [Value(0, MetaName = "token", Required = true,
            HelpText = "Token generated with the Tenant Unique ID and the API key")]
        public string token { get; set; }

        [Value(1, MetaName = "classificationCode", Required = true,
            HelpText = "TraceParts code of the classification.")]
        public string classificationCode { get; set; }
    }
}