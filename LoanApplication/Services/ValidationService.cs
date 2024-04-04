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

            // Check if the birthday has passed this year
            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--; // Decrease age if birthday hasn't occurred yet
            }

            if (age < minimumAge)
            {
                return false;
            }
            else if (age == minimumAge)
            {
                return dateOfBirth.Date <= today.AddDays(-1);
            }
            else
            {
                return true;
            }
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

        private string GetDomainFromEmail(string email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Host;
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
