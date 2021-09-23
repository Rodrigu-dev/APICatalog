using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
               .NotNull()
               .Length(8).WithMessage("Informe a senha");
            RuleFor(x => x.ConfirmPassword)
               .Matches("").WithMessage("Senha é inválida");
        }
    }
}
