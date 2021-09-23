using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.DTOs.Validators
{
    public class CategoriaValidator : AbstractValidator<CategoriaDTO>
    {
        public CategoriaValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Campo Obrigatório")
                .MaximumLength(50).WithMessage("Maximum length must be 50 characters");
            RuleFor(x => x.ImagemUrl)
                .NotEmpty().WithMessage("Campo Obrigatório")
                .MaximumLength(300).WithMessage("Maximum length must be 300 characters");

        }

    }
}
