using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8.dataIndexing;

// documentation : https://developers.traceparts.com/v2/reference/get_v2-search-productandcategorylist
public static class ListOfProductsAndCategories
{
    private static async Task<ApiResponse> GetListOfProductsAndCategories(string token, string classificationCode,
        string? partFamilyCode)
    {
        Console.WriteLine("📘 Warning! Any tries will be recorded in the Production data.");
        
        // As partFamilyCodeString is optional, if it is null it is just ignored
        var partFamilyCodeString = "";
        if (partFamilyCode is not null) partFamilyCodeString = "&partFamilyCode=" + MyUrlEncoder.Encode(partFamilyCode);

        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(RootApiUrl.Url + "v2/Search/ProductAndCategoryList" +
                                 "?classificationCode=" + MyUrlEncoder.Encode(classificationCode) +
                                 partFamilyCodeString
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
        Console.WriteLine(GetListOfProductsAndCategories(opts.token, opts.classificationCode, opts.partFamilyCode)
            .Result);
        return 0;
    }

    [Verb("list-of-products-and-categories", HelpText = "Get list of products and categories")]
    public class Options
    {
        [Value(0, MetaName = "token", Required = true,
            HelpText = "Token generated with the Tenant Unique ID and the API key")]
        public string token { get; set; }

        [Value(1, MetaName = "classificationCode", Required = true,
            HelpText = "TraceParts code of the classification (to use in combination with partNumber).")]
        public string classificationCode { get; set; }

        [Option("partFamilyCode", HelpText = "TraceParts code of the product family.")]
        public string partFamilyCode { get; set; }
    }
}