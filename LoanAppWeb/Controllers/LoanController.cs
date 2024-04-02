using LoanAppWeb.Models;
using LoanAppWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LoanAppWeb.Controllers
{
    public class LoanController : Controller
    {
        private readonly ILoanApplicationService _loanApplicationService;

        public LoanController(ILoanApplicationService loanApplicationService)
        {
            _loanApplicationService = loanApplicationService ?? throw new ArgumentNullException(nameof(loanApplicationService));
        }

        public async Task<IActionResult> LoanApplication(int id)
        {
            try
            {
                var loanApplication = await _loanApplicationService.GetLoanApplication(id);
                return View(loanApplication);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CalculateQuote(LoanApplicationViewModel model)
        {
            try

            {
                if (!ModelState.IsValid)
                {
                    // If model state is invalid, return the view with validation errors
                    return View("LoanApplication", model);
                }
                var repaymentAmount = await _loanApplicationService.CalculateRepaymentAmount(model);
                model.RepaymentAmount = repaymentAmount;
                return View("Quote", model);
            }
            catch (Exception ex)
            {
                // Log the exception
                ViewData["ErrorMessage"] = ex.Message;
                return View("LoanApplication", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitLoanApplication(LoanApplicationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // If model state is invalid, return the view with validation errors
                    return View("LoanApplication", model);
                }

                // Perform validation checks
                if (!await _loanApplicationService.ValidateLoanApplication(model))
                {
                    throw new Exception("Validation checks failed. Please review your information.");
                }

                // Apply for the loan
                await _loanApplicationService.ApplyForLoanAsync(model);

                // Redirect to success page
                return RedirectToAction("Success", "Loan");
            }
            catch (Exception ex)
            {
                // Log the exception
                ViewData["ErrorMessage"] = ex.Message;
                return View("Quote", model);
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
