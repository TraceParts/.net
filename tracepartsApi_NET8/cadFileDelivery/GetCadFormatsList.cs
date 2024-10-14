using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8.cadFileDelivery;

// documentation : https://developers.traceparts.com/v2/reference/get_v3-product-caddataavailability
public static class GetCadFormatsList
{
    private static async Task<ApiResponse> GetGetCadFormatsList(string token,
        Dictionary<string, string> possibleOptions, string cultureInfo)
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
            RequestUri = new Uri(RootApiUrl.Url + "v3/Product/CadDataAvailability" +
                                 "?cultureInfo=" + MyUrlEncoder.Encode(cultureInfo) +
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
        Console.WriteLine(GetGetCadFormatsList(opts.token, GetPossibleOptions(opts), opts.cultureInfo).Result);
        return 0;
    }

    private static Dictionary<string, string> GetPossibleOptions(Options opts)
    {
        var possibleOptions = new Dictionary<string, string>
        {
            { "partFamilyCode", opts.partFamilyCode },
            { "selectionPath", opts.selectionPath },
            { "classificationCode", opts.classificationCode },
            { "partNumber", opts.partNumber }
        };
        return possibleOptions;
    }

    [Verb("get-cad-formats-list",
        HelpText =
            "Get CAD formats list. Request to get CAD formats list can manage two ways:\n\npartFamilyCode and selectionPath (without selectionPath, the default configuration is used)\nclassificationCode and partNumber (both parameters are required with this way)")]
    public class Options
    {
        [Value(0, MetaName = "token", Required = true,
            HelpText = "Token generated with the Tenant Unique ID and the API key")]
        public string token { get; set; }

        [Option("partFamilyCode", HelpText = "TraceParts code of the product family.")]
        public string partFamilyCode { get; set; }

        [Option("selectionPath",
            HelpText =
                "Selected configuration (it is used in combination with partFamilyCode if not provided part will be loaded with default configuration)")]
        public string selectionPath { get; set; }

        [Option("classificationCode",
            HelpText = "TraceParts code of the classification (to use in combination with partNumber).")]
        public string classificationCode { get; set; }

        [Option("partNumber",
            HelpText =
                "Identifier of a product (to use in combination with classificationCode). Part number as stored in the TraceParts database.")]
        public string partNumber { get; set; }

        [Value(1, MetaName = "cultureInfo", Required = true,
            HelpText = "Language for the labels of the CAD formats.")]
        public string cultureInfo { get; set; }
    }
}