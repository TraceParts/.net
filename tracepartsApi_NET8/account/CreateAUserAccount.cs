using System.Net.Http.Headers;
using System.Text.Json;
using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8.account;

//📘 Warning! Any tries will be recorded in the Production data.
// documentation : https://developers.traceparts.com/v2/reference/post_v2-account-signup
public static class CreateAUserAccount
{
    private static async Task<ApiResponse> PostCreateAUserAccount(string token, string userEmail,
        Dictionary<string, string> possibleOptions)
    {
        Console.WriteLine("📘 Warning! Any tries will be recorded in the Production data.");
        // Gathering all parameters
        var parameters = new Dictionary<string, string> { { "userEmail", userEmail } };
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
            RequestUri = new Uri(RootApiUrl.Url + "v2/Account/SignUp"),
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
        Console.WriteLine(PostCreateAUserAccount(opts.token, opts.userEmail, GetPossibleOptions(opts))
            .Result);
        return 0;
    }

    private static Dictionary<string, string> GetPossibleOptions(Options opts)
    {
        var possibleOptions = new Dictionary<string, string>
        {
            { "company", opts.company },
            { "country", opts.country },
            { "name", opts.name },
            { "fname", opts.fname },
            { "addr1", opts.addr1 },
            { "addr2", opts.addr2 },
            { "addr3", opts.addr3 },
            { "city", opts.city },
            { "state", opts.state },
            { "zipCode", opts.zipCode },
            { "phone", opts.phone },
            { "fax", opts.fax },
            {
                "tpOptIn", opts.tpOptIn.ToString().ToLower()
            }, //TODO: test if "true" and "false" are convenient for boolean parameters -> if not must give them apart in the function call
            {
                "partnersOptIn", opts.partnersOptIn.ToString().ToLower()
            } //TODO: test if "true" and "false" are convenient for boolean parameters -> if not must give them apart in the function call
        };
        return possibleOptions;
    }

    [Verb("create-a-user-account", HelpText = "Create a user account")]
    public class Options
    {
        [Value(0, MetaName = "token", Required = true,
            HelpText = "Token generated with the Tenant Unique ID and the API key")]
        public string token { get; set; }

        [Value(1, MetaName = "userEmail", Required = true,
            HelpText = "Email address linked to the account.")]
        public string userEmail { get; set; }

        [Option("company", HelpText = "User company.")]
        public string company { get; set; }

        [Option("country", HelpText = "User country. ISO 3166-2 characters.")]
        public string country { get; set; }

        [Option("name", HelpText = "User last name.")]
        public string name { get; set; }

        [Option("fname", HelpText = "User first name.")]
        public string fname { get; set; }

        [Option("addr1", HelpText = "First field for the user address.")]
        public string addr1 { get; set; }

        [Option("addr2", HelpText = "Second field for the user address.")]
        public string addr2 { get; set; }

        [Option("addr3", HelpText = "Third field for the user address.")]
        public string addr3 { get; set; }

        [Option("city", HelpText = "User city.")]
        public string city { get; set; }

        [Option("state", HelpText = "User state, for North America.")]
        public string state { get; set; }

        [Option("zipCode", HelpText = "User ZIP code")]
        public string zipCode { get; set; }

        [Option("phone", HelpText = "User phone number.")]
        public string phone { get; set; }

        [Option("fax", HelpText = "User FAX number.")]
        public string fax { get; set; }

        [Option("tpOptIn",
            HelpText = "Consent to receive information sent by TraceParts by email about TraceParts services.")]
        public bool tpOptIn { get; set; }

        [Option("partnersOptIn",
            HelpText =
                "Consent to receive information sent by TraceParts by email about TraceParts’ partners’ services.")]
        public bool partnersOptIn { get; set; }
    }
}