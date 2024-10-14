using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8.dataIndexing;

// documentation : https://developers.traceparts.com/v2/reference/get_v1-search-partnumberlist
public static class ListOfPartNumbers
{
    private static async Task<ApiResponse> GetListOfPartNumbers(string token, string partFamilyCode,
        bool returnYourOwnCodes)
    {
        Console.WriteLine("📘 Warning! Any tries will be recorded in the Production data.");
        
        // As the default value is false, if returnYourOwnCodes is false it is just ignored
        var returnYourOwnCodesString = "";
        if (returnYourOwnCodes) returnYourOwnCodesString = "&returnYourOwnCodes=true";

        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(RootApiUrl.Url + "v1/Search/PartNumberList" +
                                 "?partFamilyCode=" + MyUrlEncoder.Encode(partFamilyCode) +
                                 returnYourOwnCodesString
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
        Console.WriteLine(GetListOfPartNumbers(opts.token, opts.partFamilyCode, opts.returnYourOwnCodes)
            .Result);
        return 0;
    }

    [Verb("list-of-part-numbers", HelpText = "Get list of part numbers")]
    public class Options
    {
        [Value(0, MetaName = "token", Required = true,
            HelpText = "Token generated with the Tenant Unique ID and the API key")]
        public string token { get; set; }

        [Value(1, MetaName = "partFamilyCode", Required = true,
            HelpText = "TraceParts code of the product family.")]
        public string partFamilyCode { get; set; }

        [Option("returnYourOwnCodes",
            HelpText = "If available, your own codes (i.e.: SKU, internal_code, Part_ID) are returned.")]
        public bool returnYourOwnCodes { get; set; }
    }
}