using LoanApplicationApi.Dtos;
using LoanApplicationApi.Mappers;
using LoanApplicationApi.Models;
using LoanApplicationApi.Repositories;
using LoanApplicationApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;

namespace LoanApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanApplicationController : ControllerBase
    {
        private readonly ILoanApplicationRepository _loanApplicationRepository;
        private readonly LoanCalculatorService _loanCalculatorService;
        private readonly ValidationService _validationService;

        public LoanApplicationController(ILoanApplicationRepository loanApplicationRepository, LoanCalculatorService loanCalculatorService, ValidationService validationService)
        {
            _loanApplicationRepository = loanApplicationRepository;
            _loanCalculatorService = loanCalculatorService;
            _validationService = validationService;
        }

        [HttpPost("calculate-repayment")]
        public IActionResult CalculateLoanRepayment([FromBody] LoanRequestCalculateDto loanRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            decimal repaymentAmount = _loanCalculatorService.CalculateRepaymentAmount(loanRequest.AmountRequired, loanRequest.Term, loanRequest.Product);

            return Ok(new { RepaymentAmount = repaymentAmount });
        }

        [HttpGet]
        public IActionResult GetAllLoan()
        {
            return Ok(_loanApplicationRepository.GetAllLoanApplications());
        }

        [HttpGet("loanApi/{id:int}")]
        public async Task<IActionResult> GetLoanApplicationById([FromRoute] int id)
        {
            var loanApplication = await _loanApplicationRepository.GetLoanApplicationByIdAsync(id);
            if (loanApplication == null)
            {
                return NotFound();
            }
            return Ok(loanApplication);
        }

        [HttpPost("LoanRequest")]
        public async Task<IActionResult> PostLoanApplication([FromBody] CreateLoanRequestDto loanDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var loanModel = loanDto.ToLoanFromCreateDto();
                string applicantIdentifier = GenerateAppIdentifier(loanModel.FirstName, loanModel.LastName, loanModel.DateOfBirth);

                (bool loanExists, string existingRedirectUrl) = await _loanApplicationRepository.GetExistingLoan(applicantIdentifier, loanModel);

                if (loanExists)
                {
                    return Redirect(existingRedirectUrl);
                }
                else
                {
                    loanModel.ApplicantIdentifier = applicantIdentifier;

                    var savedLoanApplication = await _loanApplicationRepository.SaveLoanApplicationAsync(loanModel);

                    string redirectUrl = GenerateRedirectUrl(savedLoanApplication.Id);
                    savedLoanApplication.RedirectUrl = redirectUrl;

                    await _loanApplicationRepository.UpdateLoanApplicationAsync(savedLoanApplication);

                    return Redirect(redirectUrl);
                }

               
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        private string GenerateRedirectUrl(int loanAppId)
        {
            string baseUrl = "https://localhost:7144/Loan/LoanApplication/";
            return baseUrl + loanAppId;
        }

        private string GenerateAppIdentifier(string firstName, string lastName, DateTime dateOfBirth)
        {
            return $"{firstName.Substring(0, 1)}{lastName.Substring(0, 1)}{dateOfBirth:ddMMyyyy}";
        }

        [HttpPost]
        [Route("CalculateQuote")]
        public IActionResult CalculateQuote([FromBody] LoanApplicationRequestModel loanApplication)
        {
            var repaymentAmount = _loanCalculatorService.CalculateRepaymentAmount(loanApplication.AmountRequired,loanApplication.Term, loanApplication.Product);
            return Ok(repaymentAmount);
        }

        [HttpPost("validate")]
        public IActionResult ValidateLoanApplication(LoanApplicationRequestModel model)
        {
            var validationErrors = new List<string>();

            ValidateLoanRequest(model, validationErrors);

            if (validationErrors.Count > 0)
            {
                return BadRequest(validationErrors);

            }

            return Ok(true);
        }

        private void ValidateLoanRequest(LoanApplicationRequestModel model, List<string> validationErrors)
        {
            if (!_validationService.IsApplicantOverMinimumAge(model.DateOfBirth, 18))
            {
                validationErrors.Add("Applicant must be at least 18 years old.");
            }

            if (!_validationService.IsMobileNumberNotBlacklisted(model.Mobile))
            {
                validationErrors.Add("Mobile number is blacklisted.");
            }

            if (!_validationService.IsEmailDomainNotBlacklisted(model.Email))
            {
                validationErrors.Add("Email domain is blacklisted.");
            }
        }



        [HttpPost("apply")]
        public async Task<IActionResult> ApplyForLoanAsync([FromBody] LoanApplicationRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _loanApplicationRepository.ProcessLoanApplicationAsync(model);

                if (result == null)
                {
                    return BadRequest("An error occurred while processing your request.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }
}
