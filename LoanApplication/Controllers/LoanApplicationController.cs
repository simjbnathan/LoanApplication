using LoanApplicationApi.Dtos;
using LoanApplicationApi.Mappers;
using LoanApplicationApi.Models;
using LoanApplicationApi.Repositories;
using LoanApplicationApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace LoanApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanApplicationApiController : ControllerBase
    {
        private readonly ILoanApplicationRepository _loanApplicationRepository;
        private readonly LoanCalculator _loanCalculator;
        private readonly ValidationService _validationService;

        public LoanApplicationApiController(ILoanApplicationRepository loanApplicationRepository, LoanCalculator loanCalculator, ValidationService validationService)
        {
            _loanApplicationRepository = loanApplicationRepository;
            _loanCalculator = loanCalculator;
            _validationService = validationService;
        }

        [HttpGet]
        public IActionResult GetAllLoan()
        {
            return Ok(_loanApplicationRepository.GetAllLoanApplications());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoanApplicationById([FromRoute] int id)
        {
            var loanApplication = await _loanApplicationRepository.GetLoanApplicationByIdAsync(id);
            if (loanApplication == null)
            {
                return NotFound();
            }
            return Ok(loanApplication);
        }
            
        [HttpPost]
        public async Task<IActionResult> PostLoanApplication([FromBody] CreateLoanRequestDto loanDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (loanDto == null)
                {
                    return BadRequest("Loan application data is required.");
                }

                var loanModel = loanDto.ToLoanFromCreateDto();
                // Save the loan application data
                var savedLoanApplication = await _loanApplicationRepository.SaveLoanApplicationAsync(loanModel);

                // Generate redirect URL to the ApplyLoan action in the LoanController
                var redirectUrl = "https://localhost:7144/Loan/LoanApplication/" + savedLoanApplication.Id;
                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        [Route("CalculateQuote")]
        public IActionResult CalculateQuote([FromBody] LoanApplicationModel loanApplication)
        {
            var repaymentAmount = _loanCalculator.CalculateRepaymentAmount(loanApplication.AmountRequired,loanApplication.Term, loanApplication.Product);
            return Ok(repaymentAmount);
        }

        [HttpPost("validate")]
        public IActionResult ValidateLoanApplication(LoanApplicationModel model)
        {
            var validationErrors = new List<string>();

            // Check if applicant is over minimum age
            if (!_validationService.IsApplicantOverMinimumAge(model.DateOfBirth, 18))
            {
                validationErrors.Add("Applicant must be at least 18 years old.");
            }

            // Check if mobile number is not blacklisted
            if (!_validationService.IsMobileNumberNotBlacklisted(model.Mobile))
            {
                validationErrors.Add("Mobile number is blacklisted.");
            }

            // Check if email domain is not blacklisted
            var emailDomain = GetDomainFromEmail(model.Email);
            if (emailDomain != null && !_validationService.IsEmailDomainNotBlacklisted(emailDomain))
            {
                validationErrors.Add("Email domain is blacklisted.");
            }

            // Return validation errors if any
            if (validationErrors.Count > 0)
            {
                return BadRequest(validationErrors);
            }

            // Validation passed
            return Ok(true);
        }

        private string GetDomainFromEmail(string email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Host;
            }
            catch (FormatException)
            {
                // Invalid email format
                return null;
            }
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyForLoanAsync([FromBody] LoanApplicationModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Perform additional validation if needed

                // Process the loan application asynchronously
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
