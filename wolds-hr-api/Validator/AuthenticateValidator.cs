﻿using FluentValidation;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Helper.Dto.Requests;
using BC = BCrypt.Net.BCrypt;

namespace wolds_hr_api.Validator;

public class AuthenticateValidator : AbstractValidator<LoginRequest>
{
    private readonly IAccountRepository _accountRepository;

    public AuthenticateValidator(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;

        RuleSet("LoginValidation", () =>
        {
            RuleFor(login => login.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Length(8, 150).WithMessage("Email length between 8 and 150.")
                .EmailAddress().WithMessage("Invalid Email.");

            RuleFor(login => login.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(8, 50).WithMessage("Password length between 8 and 50.");

            RuleFor(_ => _)
                .Must(login => ValidLoginDetails(login))
                .WithMessage("Invalid login");
        });
    }

    protected bool ValidLoginDetails(LoginRequest loginRequest)
    {
        var account = _accountRepository.Get(loginRequest.Email);
        if (account == null || !account.IsAuthenticated || !BC.Verify(loginRequest.Password, account.PasswordHash))
        {
            return false;
        }

        return true;
    }
}