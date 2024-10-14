using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8.search.checkAvailabilityWith;

// documentation : https://developers.traceparts.com/v2/reference/get_v2-search-partnumber-availability
public static class PartNumber
{
    private static async Task<ApiResponse> GetPartNumber(string token, string partNumber, string catalog,
        bool removeChar)
    {
        Console.WriteLine("📘 Warning! Any tries will be recorded in the Production data.");
        
        // As the default value is false, if returnYourOwnCodes is false it is just ignored
        var removeCharString = "";
        if (removeChar) removeCharString = "&removeChar=true";

        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(RootApiUrl.Url + "v2/Search/PartNumber/Availability" +
                                 "?partNumber=" + MyUrlEncoder.Encode(partNumber) +
                                 "&catalog=" + MyUrlEncoder.Encode(catalog) +
                                 removeCharString
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
        Console.WriteLine(GetPartNumber(opts.token, opts.partNumber, opts.catalog, opts.removeChar).Result);
        return 0;
    }

    [Verb("part-number", HelpText = "Check catalog availability with part number and catalog label")]
    public class Options
    {
        [Value(0, MetaName = "token", Required = true,
            HelpText = "Token generated with the Tenant Unique ID and the API key")]
        public string token { get; set; }

        [Value(1, MetaName = "partNumber", Required = true,
            HelpText = "Part Number as you have in your own data.")]
        public string partNumber { get; set; }

        [Value(2, MetaName = "catalog", Required = true,
            HelpText = "Catalog label as you have in your own data.")]
        public string catalog { get; set; }

        [Option("removeChar", Required = false,
            HelpText = "The following characters are not evaluating (\" \", \".\", \"-\", \"/\", \"+\").")]
        public bool removeChar { get; set; }
    }
}