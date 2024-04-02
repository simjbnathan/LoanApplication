using Microsoft.VisualBasic;

namespace LoanApplicationApi.Services
{
    public class LoanCalculatorService
    {
        private readonly IConfiguration _configuration;
        private decimal _establishmentFee = 300;

        public LoanCalculatorService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public decimal CalculateRepaymentAmount(decimal loanAmount, int durationInMonths, string product)
        {
            decimal interestRate = GetInterestRate(product);
            decimal monthlyInterestRate = interestRate / 12;
            int totalPayments = durationInMonths;
            
            decimal repaymentAmount = (decimal)Financial.Pmt((double)monthlyInterestRate, totalPayments, (double)-loanAmount);
            repaymentAmount += _establishmentFee;

            return repaymentAmount;
        }

        private decimal GetInterestRate(string product)
        {
            decimal defaultInterestRate = 0.05m; // Default interest rate (5%)
            decimal interestRate = _configuration.GetValue<decimal>($"LoanCalculator:InterestRates:{product}", defaultInterestRate);
            return interestRate;
        }
    }
}
