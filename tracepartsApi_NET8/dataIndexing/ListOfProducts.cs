using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8.dataIndexing;

// documentation : https://developers.traceparts.com/v2/reference/get_v2-search-productlist
public static class ListOfProducts
{
    private static async Task<ApiResponse> GetListOfProducts(string token, string cultureInfo,
        string classificationCode,
        string? categoryCode)
    {
        Console.WriteLine("📘 Warning! Any tries will be recorded in the Production data.");
        
        // As categoryCode is optional, if it is null it is just ignored
        var categoryCodeString = "";
        if (categoryCode is not null) categoryCodeString = "&categoryCode=" + MyUrlEncoder.Encode(categoryCode);

        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(RootApiUrl.Url + "v2/Search/ProductList" +
                                 "?cultureInfo=" + MyUrlEncoder.Encode(cultureInfo) +
                                 "&classificationCode=" + MyUrlEncoder.Encode(classificationCode) +
                                 categoryCodeString
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
        Console.WriteLine(GetListOfProducts(opts.token, opts.cultureInfo, opts.classificationCode, opts.categoryCode)
            .Result);
        return 0;
    }

    [Verb("list-of-products", HelpText = "Get list of products")]
    public class Options
    {
        [Value(0, MetaName = "token", Required = true,
            HelpText = "Token generated with the Tenant Unique ID and the API key")]
        public string token { get; set; }

        [Value(1, MetaName = "cultureInfo", Required = true,
            HelpText = "Language of the labels.")]
        public string cultureInfo { get; set; }

        [Value(2, MetaName = "classificationCode", Required = true,
            HelpText = "TraceParts code of the classification (to use in combination with partNumber).")]
        public string classificationCode { get; set; }

        [Option("categoryCode", HelpText = "Unique category code in the related classification.")]
        public string categoryCode { get; set; }
    }
}