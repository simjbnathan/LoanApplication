using LoanApplicationApi.Dtos;
using LoanApplicationApi.Models;

namespace LoanApplicationApi.Repositories
{
    public interface ILoanApplicationRepository
    {

        Task<List<LoanApplicationModel>> GetAllLoanApplications();
        Task<LoanApplicationModel> GetLoanApplicationByIdAsync(int id);
        Task<LoanApplicationModel> ProcessLoanApplicationAsync(LoanApplicationModel model);
        Task<LoanApplicationModel> SaveLoanApplicationAsync(LoanApplicationModel loanModel);
    }
}
