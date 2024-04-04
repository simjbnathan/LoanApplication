using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LoanAppWeb.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace LoanAppWeb.Services
{
    public class LoanApplicationService : ILoanApplicationService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public LoanApplicationService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiSettings = apiSettings?.Value ?? throw new ArgumentNullException(nameof(apiSettings));
        }

        public async Task<LoanApplicationViewModel> ApplyForLoanAsync(LoanApplicationViewModel model)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiSettings.LoanApiBaseUrl}api/LoanApplication/apply", jsonContent);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<LoanApplicationViewModel>();
        }

        public async Task<decimal> CalculateRepaymentAmount(LoanApplicationViewModel model)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiSettings.LoanApiBaseUrl}api/LoanApplication/calculate-repayment", jsonContent);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            string unescapedJson = responseContent.Replace("\\", "");
            var responseObject = JObject.Parse(unescapedJson);
            var repaymentAmount = responseObject.Value<decimal>("repaymentAmount");

            return repaymentAmount;
        }

        public async Task<LoanApplicationViewModel> GetLoanApplication(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiSettings.LoanApiBaseUrl}api/LoanApplication/loanApi/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<LoanApplicationViewModel>();
        }


        public async Task<(bool isValid, List<string> validationErrors)> ValidateLoanApplication(LoanApplicationViewModel model)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiSettings.LoanApiBaseUrl}api/LoanApplication/validate", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var isValid = JsonSerializer.Deserialize<bool>(responseContent);
                return (isValid, null); 
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var validationErrors = JsonSerializer.Deserialize<List<string>>(responseContent);
                return (false, validationErrors);
            }

            return (false, null);
        }

    }
}
