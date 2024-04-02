
using LoanAppWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace LoanAppWeb.Services
{
    public class LoanApplicationService: ILoanApplicationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public LoanApplicationService(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        public async Task<LoanApplicationViewModel> ApplyForLoanAsync(LoanApplicationViewModel model)
        {
            try
            {
                    var httpClient =  _httpClientFactory.CreateClient();
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("api/LoanApplicationApi/apply", jsonContent);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadFromJsonAsync<LoanApplicationViewModel>();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while applying for the loan.", ex);
            }
        }

        public async Task<decimal> CalculateRepaymentAmount(LoanApplicationViewModel model)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient(); 
                var response = await httpClient.PostAsJsonAsync($"{_apiSettings.LoanApiBaseUrl}api/LoanApplicationApi/calculate-repayment", model);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JObject.Parse(responseContent);
                var repaymentAmount = responseObject.Value<decimal>("repaymentAmount");

                return repaymentAmount;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while calculating repayment amount.", ex);
            }
        }

        public async Task<LoanApplicationViewModel> GetLoanApplication(int id)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync($"{_apiSettings.LoanApiBaseUrl}api/LoanApplicationApi/loanApi/{id}");

                if (response.IsSuccessStatusCode)
                {
                    

                    return await response.Content.ReadFromJsonAsync<LoanApplicationViewModel>();
                }
                else
                {
                    // Handle unsuccessful response
                    throw new HttpRequestException($"Failed to fetch loan application. Status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while processing the request.", ex);
            }
        }

        public async Task<bool> ValidateLoanApplication(LoanApplicationViewModel model)
        {
            try
            {
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                    var httpClient =  _httpClientFactory.CreateClient();
                    var response = await httpClient.PostAsync($"{_apiSettings.LoanApiBaseUrl}api/LoanApplicationApi/validate", jsonContent);
                    response.EnsureSuccessStatusCode();
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<bool>(responseContent);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while validating loan application.", ex);
            }
        }
    }
}

