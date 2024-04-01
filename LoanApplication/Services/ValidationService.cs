namespace LoanApplicationApi.Services
{
    public class ValidationService
    {
        private readonly List<string> blacklistedMobileNumbers = new List<string> { "1234567890", "9876543210" };
        private readonly List<string> blacklistedDomains = new List<string> { "example.com", "test.com" };

        public bool IsApplicantOverMinimumAge(DateTime dateOfBirth, int minimumAge)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-age))
                age--;

            return age >= minimumAge;
        }

        public bool IsMobileNumberNotBlacklisted(string mobileNumber)
        {
            return !blacklistedMobileNumbers.Contains(mobileNumber);
        }

        public bool IsEmailDomainNotBlacklisted(string email)
        {
            var domain = GetDomainFromEmail(email);
            return !blacklistedDomains.Contains(domain);
        }

        // Helper method to extract domain from email
        private string GetDomainFromEmail(string email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Host;
            }
            catch (FormatException)
            {
                // Invalid email format
                return null;
            }
        }
    }
}
