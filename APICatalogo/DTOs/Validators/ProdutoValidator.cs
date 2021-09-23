using APICatalogo.DTOs;
using APICatalogo.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Validators
{
    public class ProdutoValidator : AbstractValidator<ProdutoDTO>
    {
        public ProdutoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Campo Obrigatório")
                .MaximumLength(50).WithMessage("Maximum length must be 50 characters");
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("Campo Obrigatório")
                .MaximumLength(300).WithMessage("Maximum length must be 300 characters");
            RuleFor(x => x.Preco)
                .NotEmpty().WithMessage("Campo Obrigatório")
                .GreaterThan(0.0M).WithMessage("Preço deve ser maior que zero");
            RuleFor(x => x.ImagemUrl)
                .NotEmpty().WithMessage("Campo Obrigatório")
                .MaximumLength(50).WithMessage("Maximum length must be 50 characters");

        }
        
    }
}
