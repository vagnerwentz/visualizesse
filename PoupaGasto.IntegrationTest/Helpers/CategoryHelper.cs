// namespace PoupaGasto.IntegrationTest.Helpers;
//
// public class CategoryHelper
// {
//     private readonly HttpClient _client;
//
//     public CategoryHelper(HttpClient client)
//     {
//         _client = client;
//     }
//
//     public async Task CreateWalletAsync(string description, string token, string userId)
//     {
//         var walletCommand = new CreateWalletRequest(description);
//         _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
//         _client.DefaultRequestHeaders.Add("x-user-identification", userId);
//         var createWalletContent = new StringContent(JsonConvert.SerializeObject(walletCommand), Encoding.UTF8, "application/json");
//         var response = await _client.PostAsync("/api/v1/wallet/create", createWalletContent);
//
//         response.EnsureSuccessStatusCode();
//     }
// }