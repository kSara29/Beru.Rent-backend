using System.Text.RegularExpressions;
using FluentValidation;
using User.Application.DTO;

namespace User.Application.Extencions.Validation;

public class CreateUserValidation: AbstractValidator<CreateUserDto>
{
    public CreateUserValidation()
    {
        RuleFor(u => u.FirstName).NotEmpty().WithMessage("Поле обязательно для заполнения.");
        RuleFor(u => u.LastName).NotEmpty().WithMessage("Поле обязательно для заполнения.");
        RuleFor(u => u.UserName).NotEmpty().WithMessage("Поле обязательно для заполнения.");
        RuleFor(u => u.Iin).NotEmpty().WithMessage("Поле обязательно для заполнения.");
        RuleFor(u => u.Mail).NotEmpty().WithMessage("Поле обязательно для заполнения.")
            .EmailAddress().WithMessage("Некорректный адрес почты.");
        RuleFor(u => u.Phone)
            .NotEmpty().WithMessage("Поле обязательно для заполнения.")
            .Matches(new Regex(@"^\(?[+]?([0-9]{1})\)?[- ]?([0-9]{3})[- ]?([0-9]{3})[- ]?([0-9]{2})[- ]?([0-9]{2})$"))
            .WithMessage("Не корректный ввод номера");
        RuleFor(u => u.Password).NotEmpty().WithMessage("Поле обязательно для заполнения.")
            .Equal(u => u.ConfirmPassword).WithMessage("Пароли не совпадают.");
        
    }
}