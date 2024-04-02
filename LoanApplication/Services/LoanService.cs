using LoanApplicationApi.Models;
using LoanApplicationApi.Repositories;

namespace LoanApplicationApi.Services
{
    public class LoanService
    {
        private readonly ILoanApplicationRepository _loanRepository;
        private readonly LoanCalculatorService _loanCalculator;

        public LoanService(ILoanApplicationRepository loanRepository, LoanCalculatorService loanCalculator)
        {
            _loanRepository = loanRepository;
            _loanCalculator = loanCalculator;
        }

        public async Task<decimal> CalculateQuote(int loanApplicationId, string product)
        {
            // Retrieve loan application data from the repository based on the ID
            LoanApplicationRequestModel loanApplication = await _loanRepository.GetLoanApplicationByIdAsync(loanApplicationId);

            // Calculate quote based on loan application data and product using LoanCalculator service
            decimal repaymentAmount = _loanCalculator.CalculateRepaymentAmount(loanApplication.AmountRequired, loanApplication.Term, product);

            // Perform any additional checks or calculations as needed

            return repaymentAmount;
        }
    }
}
