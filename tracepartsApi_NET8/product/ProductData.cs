using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8.product;

// documentation : https://developers.traceparts.com/v2/reference/get_v3-product-configure
public static class ProductData
{
    private static async Task<ApiResponse> GetProductData(string token, string partFamilyCode, string cultureInfo,
        Dictionary<string, string> possibleOptions)
    {
        Console.WriteLine("📘 Warning! Any tries will be recorded in the Production data.");
        // Gathering all parameters
        var possibleOptionsString = "";
        foreach (var possibleOption in possibleOptions)
            // Checking for each option if the value is usable (not empty)
            if (!string.IsNullOrEmpty(possibleOption.Value))
            {
                // Format all usable options and adding them ones after others
                var formatedValue = MyUrlEncoder.Encode(possibleOption.Value);
                possibleOptionsString += $"&{possibleOption.Key}={formatedValue}";
            }

        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(RootApiUrl.Url + "v3/Product/Configure" +
                                 "?partFamilyCode=" + MyUrlEncoder.Encode(partFamilyCode) +
                                 "&cultureInfo=" + MyUrlEncoder.Encode(cultureInfo) +
                                 possibleOptionsString),
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
        Console.WriteLine(GetProductData(opts.token, opts.partFamilyCode, opts.cultureInfo, GetPossibleOptions(opts))
            .Result);
        return 0;
    }

    private static Dictionary<string, string> GetPossibleOptions(Options opts)
    {
        var possibleOptions = new Dictionary<string, string>
        {
            { "selectionPath", opts.selectionPath },
            { "cadDetailLevel", opts.cadDetailLevel },

            { "currentStepNumber", opts.currentStepNumber.ToString() }
        };
        return possibleOptions;
    }

    [Verb("product-data", HelpText = "Get a product configuration")]
    public class Options
    {
        [Value(0, MetaName = "token", Required = true,
            HelpText = "Token generated with the Tenant Unique ID and the API key")]
        public string token { get; set; }

        [Value(1, MetaName = "partFamilyCode", Required = true,
            HelpText = "TraceParts code of the product family.")]
        public string partFamilyCode { get; set; }

        [Value(2, MetaName = "cultureInfo", Required = true,
            HelpText = "Language of the texts.")]
        public string cultureInfo { get; set; }


        [Option("selectionPath",
            HelpText =
                "Selected configuration (to use in combination with partFamilyCode. If not provided, the product is loaded with default configuration).")]
        public string selectionPath { get; set; }

        [Option("cadDetailLevel",
            Default = "-1",
            HelpText =
                "Integer related to the level of detail included in the CAD model.")]
        public string cadDetailLevel { get; set; }

        [Option("currentStepNumber",
            Default = 0,
            HelpText =
                "[DEPRECATED] Current step of configuration.")]
        public int currentStepNumber { get; set; }
    }
}