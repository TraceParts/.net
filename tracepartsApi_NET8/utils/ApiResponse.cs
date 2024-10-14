using System.Text.Json;

namespace tracepartsApi_NET8.utils;

public sealed class ApiResponse(HttpResponseMessage response)
{
    public int StatusCode { get; } = (int)response.StatusCode;
    private HttpContent Body { get; } = response.Content;

    public override string ToString()
    {
        return ToJsonString().Result;
    }

    private async Task<string> ToJsonString()
    {
        var body = await Body.ReadAsStringAsync();

        var bodyString = "null";
        // This prevents errors linked to an empty body
        if (!string.IsNullOrEmpty(body)) bodyString = body;

        var jsonString = $"{{\"StatusCode\":{StatusCode}, \"Body\":{bodyString}}}";
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(jsonString);
        var prettyJson =
            JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { WriteIndented = true });

        return prettyJson;
    }
}