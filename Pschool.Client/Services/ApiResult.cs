   using System.Text.Json;

   namespace Pschool.Client.Services;

   // The outcome of a create, edit or delete call: success, or a message for the user.
   public class ApiResult
   {
       public bool Success { get; private init; }
       public string? ErrorMessage { get; private init; }

       public static ApiResult Ok() => new() { Success = true };
       public static ApiResult Fail(string message) => new() { Success = false, ErrorMessage = message };

       // Turn the API's reply into an ApiResult.
       public static async Task<ApiResult> FromResponseAsync(HttpResponseMessage response)
       {
           if (response.IsSuccessStatusCode)
           {
               return Ok();
           }

           var body = await response.Content.ReadAsStringAsync();
           return Fail(ReadErrorMessage(body));
       }

       // The API sends errors in two shapes:
       //  - validation: JSON with an "errors" list per field (from [Required] etc.)
       //  - plain text: e.g. "This parent still has students..."
       private static string ReadErrorMessage(string body)
       {
           if (string.IsNullOrWhiteSpace(body))
           {
               return "Something went wrong. Please try again.";
           }

           try
           {
               using var json = JsonDocument.Parse(body);

               if (json.RootElement.TryGetProperty("errors", out var errors))
               {
                   // Collect every field's messages into one list.
                   var messages = errors.EnumerateObject()
                       .SelectMany(field => field.Value.EnumerateArray())
                       .Select(message => message.GetString())
                       .Where(message => !string.IsNullOrWhiteSpace(message));

                   return string.Join(" ", messages);
               }

               if (json.RootElement.TryGetProperty("title", out var title))
               {
                   return title.GetString() ?? body;
               }
           }
           catch (JsonException)
           {
               // Not JSON: it's a plain-text message, use it as it is.
           }

           return body;
       }
   }