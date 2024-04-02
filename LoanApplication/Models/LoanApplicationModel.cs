namespace LoanApplicationApi.Models
{
    public enum LoanApplicationStatus
    {
        Pending,
        Approved,
        Declined
    }
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
        public int ProductId { get; set; }
        public ProductModel Product { get; set; } // Navigation property
        public LoanApplicationStatus Status { get; set; }

        // Additional calculated properties
        public decimal RepaymentAmount { get; set; }
        public decimal EstablishmentFee { get; set; }
        public decimal TotalInterest { get; set; }
    }
}
