using System.Text.RegularExpressions;
using FluentValidation;
using Visualizesse.Service.Commands.User;

namespace Visualizesse.Service.Validators.User;

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(s => s.Password)
            .Must(ValidPassword)
            .WithMessage(
                "A senha deve conter pelo menos 8 caracteres, um número, uma letra minúscula e maiúscula e um caracetere especial.");
    }

    static bool ValidPassword(string password)
    {
        var regex = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$");

        return regex.IsMatch(password);
    }
}