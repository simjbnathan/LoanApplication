using LoanApplicationApi.Data;
using LoanApplicationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanApplicationApi.Repositories
{
    public class LoanApplicationRepository : ILoanApplicationRepository
    {
        private readonly LoanDbContext _dbContext;
        public LoanApplicationRepository(LoanDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task<List<LoanApplicationRequestModel>> GetAllLoanApplications()
        {
            return await _dbContext.LoanApplications.ToListAsync();
        }

        public async Task<LoanApplicationRequestModel> SaveLoanApplicationAsync(LoanApplicationRequestModel loanApplicationDto)
        {
            await _dbContext.AddAsync(loanApplicationDto);
            await _dbContext.SaveChangesAsync();
            return loanApplicationDto;
        }

        public async Task<LoanApplicationRequestModel> GetLoanApplicationByIdAsync(int id)
        {
            return await _dbContext.FindAsync<LoanApplicationRequestModel>(id);
        }

        public async Task<LoanApplicationRequestModel> ProcessLoanApplicationAsync(LoanApplicationRequestModel model)
        {
            await _dbContext.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<(bool loanExists, string existingRedirectUrl)> GetExistingLoan(string applicantIdentifier, LoanApplicationRequestModel loanModel)
        {
            var existingLoan =   await _dbContext.LoanApplications.FirstOrDefaultAsync(x => x.ApplicantIdentifier == applicantIdentifier);
            bool loanExists = existingLoan != null;
            string existingRedirectUrl = loanExists ? existingLoan.RedirectUrl : string.Empty;
            return (loanExists, existingRedirectUrl);
        }

        public async Task<LoanApplicationRequestModel> UpdateLoanApplicationAsync(LoanApplicationRequestModel savedLoanApplication)
        {
            _dbContext.Entry(savedLoanApplication).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return savedLoanApplication;
        }
    }
}
