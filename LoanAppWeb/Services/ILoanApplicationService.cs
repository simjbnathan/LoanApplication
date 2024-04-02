
using LoanAppWeb.Models;


namespace LoanAppWeb.Services
{
    public interface ILoanApplicationService
    {
        Task<LoanApplicationViewModel> ApplyForLoanAsync(LoanApplicationViewModel model);
        Task<decimal> CalculateRepaymentAmount(LoanApplicationViewModel model);
        Task<LoanApplicationViewModel> GetLoanApplication(int id);
        Task<bool> ValidateLoanApplication(LoanApplicationViewModel model);
    }
}