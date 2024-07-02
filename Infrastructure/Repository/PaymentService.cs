using Newtonsoft.Json;
using PracticeForRevision.DAL;
using PracticeForRevision.Infrastructure.Interface;
using PracticeForRevision.Models;
using System.Net.Http.Headers;
using System.Text;

namespace PracticeForRevision.Infrastructure.Repository
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public PaymentService(HttpClient httpClient, ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
            _configuration = configuration;

            _httpClient.BaseAddress = new Uri(_configuration["PayPal:BaseUrl"]);
        }

        public async Task<string> CreatePayPalPaymentAsync(string amount)
        {
            string clientId = _configuration["PayPal:ClientId"];
            string clientSecret = _configuration["PayPal:ClientSecret"];

            string accessToken = await GetAccessTokenAsync(clientId, clientSecret);

            var requestBody = new Dictionary<string, object>
            {
                { "intent", "sale" },
                { "payer", new Dictionary<string, string> { { "payment_method", "paypal" } } },
                { "transactions", new List<object>
                    {
                        new Dictionary<string, object>
                        {
                            { "amount", new Dictionary<string, string> { { "total", amount }, { "currency", "USD" } } }
                        }
                    }
                },
                { "redirect_urls", new Dictionary<string, string>
                    {
                        { "return_url", "http://localhost:5250/Products/Success" },
                        { "cancel_url", "https://yourdomain.com/cancel" }
                    }
                }
            };

            var requestBodyJson = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.PostAsync("/v1/payments/payment", content);

            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var approvalUrl = JsonConvert.DeserializeObject<dynamic>(responseJson)["links"][1]["href"].ToString();

            return approvalUrl;
        }

        private async Task<string> GetAccessTokenAsync(string clientId, string clientSecret)
        {
            string authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
            var tokenRequest = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" }
            };
            var tokenRequestContent = new FormUrlEncodedContent(tokenRequest);
            var tokenResponse = await _httpClient.PostAsync("/v1/oauth2/token", tokenRequestContent);
            tokenResponse.EnsureSuccessStatusCode();

            var tokenResponseJson = await tokenResponse.Content.ReadAsStringAsync();
            var accessToken = JsonConvert.DeserializeObject<dynamic>(tokenResponseJson)["access_token"].ToString();

            return accessToken;
        }

        public async Task StorePaymentDetailsAsync(string paymentId, string token, string payerId)
        {
            var paymentDetails = new PaymentDetails
            {
                PaymentId = paymentId,
                Token = token,
                PayerId = payerId
            };

            _dbContext.PaymentDetails.Add(paymentDetails);
            await _dbContext.SaveChangesAsync();
        }
    }
}
