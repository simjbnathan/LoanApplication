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



        public async Task<List<LoanApplicationModel>> GetAllLoanApplications()
        {
            return await _dbContext.LoanApplications.ToListAsync();
        }

        public async Task<LoanApplicationModel> SaveLoanApplicationAsync(LoanApplicationModel loanApplicationDto)
        {
            await _dbContext.AddAsync(loanApplicationDto);
            await _dbContext.SaveChangesAsync();
            return loanApplicationDto;
        }

        public async Task<LoanApplicationModel> GetLoanApplicationByIdAsync(int id)
        {
            return await _dbContext.FindAsync<LoanApplicationModel>(id);
        }

        public async Task<LoanApplicationModel> ProcessLoanApplicationAsync(LoanApplicationModel model)
        {
            await _dbContext.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }
    }
}
