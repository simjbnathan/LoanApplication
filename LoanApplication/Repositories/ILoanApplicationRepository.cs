using LoanApplicationApi.Dtos;
using LoanApplicationApi.Models;

namespace LoanApplicationApi.Repositories
{
    public interface ILoanApplicationRepository
    {

        Task<List<LoanApplicationRequestModel>> GetAllLoanApplications();
        Task<LoanApplicationRequestModel> GetLoanApplicationByIdAsync(int id);
        Task<LoanApplicationRequestModel> ProcessLoanApplicationAsync(LoanApplicationRequestModel model);
        Task<LoanApplicationRequestModel> SaveLoanApplicationAsync(LoanApplicationRequestModel loanModel);
        Task<(bool loanExists, string existingRedirectUrl)> GetExistingLoan(string applicantIdentifier, LoanApplicationRequestModel loanModel);
        Task<LoanApplicationRequestModel> UpdateLoanApplicationAsync(LoanApplicationRequestModel savedLoanApplication);
        void DeclineLoanApplication(int id, List<string> validationErrors);
    }
}
