using FluentValidation;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Library.Dto.Requests;
using BC = BCrypt.Net.BCrypt;

namespace wolds_hr_api.Validator;

public sealed class AuthenticateValidator : AbstractValidator<LoginRequest>
{
    private readonly IAccountRepository _accountRepository;

    public AuthenticateValidator(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;

        RuleSet("LoginValidation", () =>
        {
            RuleFor(login => login.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(8, 150).WithMessage("Username length between 8 and 150.")
                .EmailAddress().WithMessage("Invalid Username.");

            RuleFor(login => login.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(8, 50).WithMessage("Password length between 8 and 50.");

            RuleFor(_ => _)
                .Must(login => ValidLoginDetails(login))
                .WithMessage("Invalid login");
        });
    }

    private bool ValidLoginDetails(LoginRequest loginRequest)
    {
        var account = _accountRepository.Get(loginRequest.Username);
        if (account == null || !account.IsAuthenticated || !BC.Verify(loginRequest.Password, account.PasswordHash))
        {
            return false;
        }

        return true;
    }
}