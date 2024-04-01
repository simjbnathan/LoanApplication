namespace LoanApplicationApi.Models
{
    public class LoanApplicationModel
    {
        public int Id { get; set; }
        public decimal AmountRequired { get; set; }
        public int Term { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Product { get; set; } = string.Empty;

        public decimal RepaymentAmount { get; set; }
    }
}
