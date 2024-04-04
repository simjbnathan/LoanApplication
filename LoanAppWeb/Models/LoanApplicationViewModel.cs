using System.ComponentModel.DataAnnotations;

namespace LoanAppWeb.Models
{


    public class LoanApplicationViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Amount Required is required")]
        [Display(Name = "Amount Required")]
        public decimal AmountRequired { get; set; }

        [Required(ErrorMessage = "Term is required")]
        public int Term { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string  Title { get; set; } 

        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public string Product { get; set; }

        public decimal RepaymentAmount { get; set; } 

    }
}
