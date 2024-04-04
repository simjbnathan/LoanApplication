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
            LoanApplicationRequestModel loanApplication = await _loanRepository.GetLoanApplicationByIdAsync(loanApplicationId);
            decimal repaymentAmount = _loanCalculator.CalculateRepaymentAmount(loanApplication.AmountRequired, loanApplication.Term, product);
            return repaymentAmount;
        }
    }
}
