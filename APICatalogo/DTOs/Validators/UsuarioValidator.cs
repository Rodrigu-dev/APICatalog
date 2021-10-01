using FluentValidation;

namespace APICatalogo.DTOs.Validators
{
    public class UsuarioValidator : AbstractValidator<UsuarioDTO>
    {
        public UsuarioValidator()
        {
            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("Campo Obrigatório")
               .EmailAddress().WithMessage("E-mail é Inválido");
            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Campo Obrigatório")
               .Length(8).WithMessage("Deve conter 8 caracteres");
            RuleFor(x => x.ConfirmPassword)
               .Matches("").WithMessage("Senha é inválida");
        }
    }
}
