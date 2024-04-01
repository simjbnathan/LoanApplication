using LoanApplicationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanApplicationApi.Data
{
    public class LoanDbContext: DbContext
    {
        public LoanDbContext(DbContextOptions<LoanDbContext> options) : base(options)
        {
        }

        public DbSet<LoanApplicationModel> LoanApplications { get; set; }
    }
}
