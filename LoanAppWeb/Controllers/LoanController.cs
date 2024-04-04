using LoanAppWeb.Models;
using LoanAppWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Threading.Tasks;

namespace LoanAppWeb.Controllers
{
    public class LoanController : Controller
    {
        private readonly ILoanApplicationService _loanApplicationService;
        private readonly ILogger<HomeController> _logger;

        public LoanController(ILoanApplicationService loanApplicationService, ILogger<HomeController> logger)
        {
            _loanApplicationService = loanApplicationService ?? throw new ArgumentNullException(nameof(loanApplicationService));
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> LoanApplication(int id)
        {
            try
            {
                LoanApplicationViewModel loanApplication = await _loanApplicationService.GetLoanApplication(id);
                return View(loanApplication);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CalculateQuote(LoanApplicationViewModel model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View("LoanApplication", model);
                }
                var repaymentAmount = await _loanApplicationService.CalculateRepaymentAmount(model);
                model.RepaymentAmount = repaymentAmount;

                TempData["LoanApplicationModel"] = JsonConvert.SerializeObject(model);

                return View("Quote", model);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("LoanApplication", model);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> SubmitLoanApplication()
        {
            try
            {
                var serializedModel = TempData["LoanApplicationModel"] as string;
                if (serializedModel == null)
                {
                    return RedirectToAction("Error");
                }
                var model = JsonConvert.DeserializeObject<LoanApplicationViewModel>(serializedModel);
                var (isValid, validationErrors) = await _loanApplicationService.ValidateLoanApplication(model);

                if (!isValid)
                {
                    var errorMessage = "The following errors occurred: " + string.Join(", ", validationErrors);
                    ViewData["ErrorMessage"] = errorMessage;

                    return View("Quote", model);
                }

                var loanApplication = await _loanApplicationService.ApplyForLoanAsync(model);
                TempData.Clear();
                
                return RedirectToAction("Success", "Loan");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("LoanApplication", TempData["LoanApplicationModel"]);
            }
        }

        public IActionResult Test()
        {
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
