namespace LoanApplicationApi.Services
{
    public class LoanCalculator
    {
        private decimal _establishmentFee = 300;
        private decimal _interestRate = 0.05m; // Default interest rate (5%)

        public void SetInterestRate(decimal rate)
        {
            _interestRate = rate;
        }

        public decimal CalculateRepaymentAmount(decimal loanAmount, int durationInMonths, string product)
        {
            switch (product)
            {
                case "ProductA":
                    _interestRate = 0.0m; // No interest
                    break;
                case "ProductB":
                    _interestRate = 0.05m; // 5% interest rate
                    break;
                case "ProductC":
                    _interestRate = 0.1m; // 10% interest rate
                    break;
                default:
                    // Handle unsupported product
                    throw new ArgumentException("Invalid product");
            }
            decimal monthlyInterestRate = _interestRate / 12;
            int totalPayments = durationInMonths;
            decimal repaymentAmount = loanAmount * (monthlyInterestRate / (1 - (decimal)Math.Pow((double)(1 + monthlyInterestRate), -totalPayments)));
            repaymentAmount += _establishmentFee;

            return repaymentAmount;
        }
    }
}
