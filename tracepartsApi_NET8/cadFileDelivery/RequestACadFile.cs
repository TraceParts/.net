using System.Net.Http.Headers;
using System.Text.Json;
using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8.cadFileDelivery;

//📘 Warning! Any tries will be recorded in the Production data.
// documentation : https://developers.traceparts.com/v2/reference/post_v3-product-cadrequest
public static class RequestACadFile
{
    private static async Task<ApiResponse> PostRequestACadFile(string token, string userEmail, string cultureInfo,
        int cadFormatId, Dictionary<string, string> possibleOptions)
    {
        Console.WriteLine("📘 Warning! Any tries will be recorded in the Production data.");
        // Gathering all parameters
        var parameters = new Dictionary<string, string>
        {
            { "userEmail", userEmail },
            { "cultureInfo", cultureInfo },
            { "cadFormatId", cadFormatId.ToString() }
        };
        foreach (var possibleOption in possibleOptions)
            // Checking for each option if the value is usable (not empty)
            if (!string.IsNullOrEmpty(possibleOption.Value))
                parameters.Add(possibleOption.Key, possibleOption.Value);

        // Format the options in a JSON string
        var parametersString = JsonSerializer.Serialize(parameters);
        Console.WriteLine(parametersString);

        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(RootApiUrl.Url + "v3/Product/cadRequest"),
            Headers =
            {
                { "accept", "application/json" },
                { "authorization", "Bearer " + token }
            },
            Content = new StringContent(parametersString)
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
        Console.WriteLine(
            "You will be given an ID in the response bellow. You will use it to get the CAD file URL so don't forget it.");
        Console.WriteLine(PostRequestACadFile(opts.token, opts.userEmail, opts.cultureInfo, opts.cadFormatId,
            GetPossibleOptions(opts)).Result);
        return 0;
    }

    private static Dictionary<string, string> GetPossibleOptions(Options opts)
    {
        var possibleOptions = new Dictionary<string, string>
        {
            { "partFamilyCode", opts.partFamilyCode },
            { "partNumber", opts.partNumber },
            { "classificationCode", opts.classificationCode },
            { "selectionPath", opts.selectionPath },

            { "cadDetailLevelId", opts.cadDetailLevelId },

            { "languageId", opts.languageId }
        };
        return possibleOptions;
    }

    [Verb("request-a-cad-file",
        HelpText =
            "Request a CAD file. Request to get a CAD file list can be done with:\n\n" +
            "partFamilyCode and selectionPath (without selectionPath, the default configuration is used)\n" +
            "classificationCode and partNumber (both parameters are required with this way)")]
    public class Options
    {
        [Value(0, MetaName = "token", Required = true,
            HelpText = "Token generated with the Tenant Unique ID and the API key")]
        public string token { get; set; }

        [Value(1, MetaName = "userEmail", Required = true,
            HelpText = "Email address associated to the CAD request event.")]
        public string userEmail { get; set; }

        [Value(2, MetaName = "cultureInfo", Required = true,
            HelpText = "Language for the labels of the CAD formats.")]
        public string cultureInfo { get; set; }

        [Value(3, MetaName = "cadFormatId", Required = true,
            HelpText = "Language for the labels of the CAD formats.")]
        public int cadFormatId { get; set; }

        [Option("partFamilyCode", HelpText = "TraceParts ID of the CAD format.")]
        public string partFamilyCode { get; set; }

        [Option("partNumber",
            HelpText =
                "Identifier of a product (to use in combination with classificationCode). Part number as stored in the TraceParts database.")]
        public string partNumber { get; set; }

        [Option("classificationCode",
            HelpText = "TraceParts code of the classification (to use in combination with partNumber).")]
        public string classificationCode { get; set; }

        [Option("selectionPath",
            HelpText =
                "Selected configuration (it is used in combination with partFamilyCode if not provided part will be loaded with default configuration)")]
        public string selectionPath { get; set; }


        [Option("cadDetailLevelId",
            HelpText =
                "TraceParts ID of the optional detail level for the CAD model.")]
        public string cadDetailLevelId { get; set; }


        [Option("languageId",
            HelpText =
                "[DEPRECATED] TraceParts ID of the language (obsolete - please use cultureInfo).")]
        public string languageId { get; set; }
    }
}