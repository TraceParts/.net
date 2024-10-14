using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8.account;

// documentation : https://developers.traceparts.com/v2/reference/get_v2-account-checklogin-useremail
public static class CheckTheExistenceOfAUserAccount
{
    private static async Task<ApiResponse> GetCheckTheExistenceOfAUserAccount(string token, string userEmail)
    {
        Console.WriteLine("📘 Warning! Any tries will be recorded in the Production data.");
        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(RootApiUrl.Url + "v2/Account/CheckLogin/"
                                                + MyUrlEncoder.Encode(userEmail)
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
        Console.WriteLine(
            "Status code 200 means the user account exists and 404 means user not found. For other codes please refer to the documentation : https://developers.traceparts.com/v2/reference/get_v2-account-checklogin-useremail");
        Console.WriteLine(GetCheckTheExistenceOfAUserAccount(opts.token, opts.userEmail)
            .Result);
        return 0;
    }

    [Verb("check-the-existence-of-a-user-account", HelpText = "Check the existence of a user account")]
    public class Options
    {
        [Value(0, MetaName = "token", Required = true,
            HelpText = "Token generated with the Tenant Unique ID and the API key")]
        public string token { get; set; }

        [Value(1, MetaName = "userEmail", Required = true,
            HelpText = "Email address linked to the account.")]
        public string userEmail { get; set; }
    }
}