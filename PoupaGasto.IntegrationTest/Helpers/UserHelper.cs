using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Visualizesse.Domain.Entities;
using Visualizesse.Domain.Exceptions;
using Visualizesse.Service.Commands.User;

namespace PoupaGasto.IntegrationTest.Helpers;

public class UserHelper
{
    private readonly HttpClient _client;

    public UserHelper(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> CreateUserAsync(string name, string email, string password)
    {
        var registerCommand = new SignUpCommand(name, email, password);
        var registerContent = new StringContent(JsonConvert.SerializeObject(registerCommand), Encoding.UTF8, "application/json");
        var registerResponse = await _client.PostAsync("/api/v1/users/register", registerContent);
        
        registerResponse.EnsureSuccessStatusCode();

        var registerResponseData = await registerResponse.Content.ReadAsStringAsync();
        var registerResult = JsonConvert.DeserializeObject<OperationResult>(registerResponseData);
        var user = JsonConvert.DeserializeObject<User>(registerResult.Data.ToString());
        return user.Uuid.ToString();
    }

    public async Task<string> AuthenticateUserAsync(string email, string password)
    {
        var signInCommand = new SignInCommand(email, password);
        var signInResponse = await _client.PostAsJsonAsync("/api/v1/users/signin", signInCommand);
        var signInResponseData = await signInResponse.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<OperationResult>(signInResponseData);
        string jsonData = JsonConvert.SerializeObject(result);
        Console.WriteLine(jsonData);
        JObject jObject = JObject.Parse(jsonData);
        string tokenValue = jObject["Data"]["token"].ToString();
        return tokenValue;
    }
}
