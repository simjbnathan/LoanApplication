namespace LoanApplicationApi.Dtos
{
    public class LoanRequestCalculateDto
    {
        public decimal AmountRequired { get; set; }
        public int Term { get; set; }
        public string Product { get; set; }

    }
}
