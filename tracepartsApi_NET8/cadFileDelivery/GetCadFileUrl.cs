using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8.cadFileDelivery;

// documentation : https://developers.traceparts.com/v2/reference/get_v2-product-cadfileurl
public static class GetCadFileUrl
{
    private static async Task<ApiResponse> GetGetCadFileUrl(string token, int cadRequestId)
    {
        Console.WriteLine("📘 Warning! Any tries will be recorded in the Production data.");
        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(RootApiUrl.Url + "v2/Product/cadFileUrl" +
                                 "?cadRequestId=" + cadRequestId),
            Headers =
            {
                { "accept", "application/json" },
                { "authorization", "Bearer " + token }
            }
        };
        return new ApiResponse(await client.SendAsync(request));
    }

    private static async Task<ApiResponse> LoopGetGetCadFileUrlRequest(string token, int cadRequestId)
    {
        const int timeout = 10; // in minutes
        const int interval = 2; // in seconds

        ApiResponse lastResponse = null;
        var resultMessage =
            $"Timeout reached ({timeout} minutes with {interval} seconds interval). Your model couldn't be generated.";

        var nbrOfIterations = timeout * 60 / interval;
        for (var i = 0; i < nbrOfIterations; i++)
        {
            Console.WriteLine($"Request {i + 1}/{nbrOfIterations}");
            lastResponse = GetGetCadFileUrl(token, cadRequestId).Result;
            // status code 204 means wait
            if (lastResponse!.StatusCode == 204)
            {
                await Task.Delay(interval * 1000);
                // Thread.Sleep(milliseconds); // this could also be used but be aware that it completely stops the execution of the current thread for X milliseconds.
            }
            else
            {
                if (lastResponse.StatusCode is >= 200 and <= 299)
                    resultMessage = "Success, results arrived !";
                else
                    resultMessage = "An error occurred. Please refer to the status code to know what append.";

                break;
            }
        }

        Console.WriteLine(resultMessage);
        return lastResponse!;
    }

    public static int Run(Options opts)
    {
        if (opts.loopRequest)
        {
            Console.WriteLine(LoopGetGetCadFileUrlRequest(opts.token, opts.cadRequestId).Result);
        }
        else
        {
            var response = GetGetCadFileUrl(opts.token, opts.cadRequestId).Result;
            if (response.StatusCode == 204)
                Console.WriteLine(
                    "Status code 204 means the file is generating. " +
                    "You must repeat this request periodically to see if the file ends its generation. " +
                    "If you want, you can add the option '-l' or '--loopRequest' to repeat automatically the request and get a definitive answer.");

            Console.WriteLine(response);
        }

        return 0;
    }

    [Verb("get-cad-file-url", HelpText = "Get CAD file URL to download the model")]
    public class Options
    {
        [Value(0, MetaName = "token", Required = true,
            HelpText = "Token generated with the Tenant Unique ID and the API key")]
        public string token { get; set; }

        [Value(1, MetaName = "cadRequestId", Required = true,
            HelpText = "ID of the request provided by the cadRequest end point")]
        public int cadRequestId { get; set; }

        [Option('l', "loopRequest", HelpText = "Loop request until there is a definitive answer (can take a while)")]
        public bool loopRequest { get; set; }
    }
}