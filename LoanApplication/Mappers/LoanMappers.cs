using LoanApplicationApi.Dtos;
using LoanApplicationApi.Models;

namespace LoanApplicationApi.Mappers
{
    public static class LoanMappers
    {
        public static LoanApplicationRequestDto MapToLoanApplicationModel(this LoanApplicationRequestModel loanModel)
        {
            return new LoanApplicationRequestDto
            {
                AmountRequired = loanModel.AmountRequired,
                Term = loanModel.Term,
                Title = loanModel.Title,
                FirstName = loanModel.FirstName,
                LastName = loanModel.LastName,
                DateOfBirth = loanModel.DateOfBirth,
                Mobile = loanModel.Mobile,
                Email = loanModel.Email
            };
        }

        public static LoanApplicationRequestModel ToLoanFromCreateDto(this CreateLoanRequestDto loanRequestDto)
        {
            return new LoanApplicationRequestModel
            {
                AmountRequired = loanRequestDto.AmountRequired,
                Term = loanRequestDto.Term,
                Title = loanRequestDto.Title,
                FirstName = loanRequestDto.FirstName,
                LastName = loanRequestDto.LastName,
                DateOfBirth = loanRequestDto.DateOfBirth,
                Mobile = loanRequestDto.Mobile,
                Email = loanRequestDto.Email
            };
        }

    }
}
