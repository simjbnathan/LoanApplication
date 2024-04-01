using LoanAppWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Text;


namespace LoanAppWeb.Controllers
{
    public class LoanController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public LoanController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClientFactory.CreateClient("LoanAppApi");
            _apiSettings = apiSettings.Value;
        }
        private decimal _establishmentFee = 300;
        private decimal _interestRate = 0.05m; // Default interest rate (5%)

        public void SetInterestRate(decimal rate)
        {
            _interestRate = rate;
        }

        public async Task<IActionResult> LoanApplication(int id)
        {
            try
            {
                // Send GET request to the API endpoint
                var response = await _httpClient.GetAsync($"{_apiSettings.LoanApiBaseUrl}api/LoanApplicationApi/{id}");

                // Handle response
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize response content and pass data to the view
                    var loanApplications = await response.Content.ReadFromJsonAsync<LoanApplicationModel>();
                    return View(loanApplications);
                }
                else
                {
                    // Handle unsuccessful response
                    return StatusCode((int)response.StatusCode, "Failed to fetch loan applications.");
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        [HttpPost]
        public IActionResult CalculateQuote(LoanApplicationModel model)
        {
            // Perform calculations and store repayment amount in the model
            model.RepaymentAmount = CalculateRepaymentAmount(model.AmountRequired, model.Term, model.Product);

            // Serialize LoanApplicationModel to JSON string
            string jsonLoanApplication = JsonConvert.SerializeObject(model);

            // Store JSON string in TempData
            TempData["LoanApplicationModel"] = jsonLoanApplication;

            // Redirect to the quote page
            return RedirectToAction("RedirectQoute");
        }

        public decimal CalculateRepaymentAmount(decimal loanAmount, int durationInMonths, string product)
        {
            try
            {
                if (durationInMonths <= 0)
                {
                    throw new ArgumentException("Duration must be a positive integer greater than zero.");
                }

                // Calculate monthly interest rate based on product
                decimal monthlyInterestRate = GetMonthlyInterestRate(product) / 12;

         
                int totalPayments = durationInMonths;

                // Calculate the repayment amount using Financial.Pmt method
                decimal repaymentAmount = (decimal)Financial.Pmt((double)monthlyInterestRate, totalPayments, (double)-loanAmount);

                repaymentAmount += _establishmentFee;

                return repaymentAmount;
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine(ex.Message);
                throw new InvalidOperationException("An error occurred while calculating the repayment amount.");
            }
        }
        private decimal GetMonthlyInterestRate(string product)
        {
            // Determine the interest rate based on the selected product
            switch (product)
            {
                case "ProductA":
                    return 0.0m; // No interest
                case "ProductB":
                    return 0.05m; // 5% interest rate
                case "ProductC":
                    return 0.1m; // 10% interest rate
                default:
                    throw new ArgumentException("Invalid product");
            }
        }
        public IActionResult RedirectQoute()
        {
            // Retrieve JSON string from TempData
            string jsonLoanApplication = TempData["LoanApplicationModel"] as string;

            // Deserialize JSON string back to LoanApplicationModel
            LoanApplicationModel model = JsonConvert.DeserializeObject<LoanApplicationModel>(jsonLoanApplication);

            // Pass the deserialized LoanApplicationModel to the view
            return View("Quote", model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitLoanApplication(LoanApplicationModel model)
        {
            try
            {
                // Call API for validation checks
                if (!await PerformValidationChecksAsync(model))
                {
                    throw new Exception("Validation checks failed. Please review your information.");
                }

                // Call the API to apply for the loan
                await ApplyForLoanAsync(model);

                // If successful, redirect to success page
                return RedirectToAction("SuccessPage");
            }
            catch (Exception ex)
            {
                // Handle exception
                ViewBag.ErrorMessage = ex.Message;
                return View("LoanApplication", model); // Return the view with error message
            }
        }

        private async Task<bool> PerformValidationChecksAsync(LoanApplicationModel model)
        {

            try
            {
                // Serialize loan application model to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                // Send POST request to validation endpoint
                var response = await _httpClient.PostAsync($"{_apiSettings.LoanApiBaseUrl}/api/LoanApplicationApi/validate", jsonContent);

                // Check if request was successful
                response.EnsureSuccessStatusCode();

                // Read response content
                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize response content to boolean
                var validationResult = JsonConvert.DeserializeObject<bool>(responseContent);

                return validationResult;
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request exception
                throw new Exception("Failed to perform validation checks. Please try again later.", ex);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                throw new Exception("An error occurred while performing validation checks.", ex);
            }
        }

        private async Task ApplyForLoanAsync(LoanApplicationModel model)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiSettings.LoanApiBaseUrl}/api/LoanApplicationApi/apply", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                // Handle unsuccessful response
                throw new Exception("Failed to apply for loan. Please try again later.");
            }
        }

    }


}
