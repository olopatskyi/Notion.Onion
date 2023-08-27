using FluentValidation;
using Notion.Application.Models.Request;

namespace Notion.WebApi.Validators;

public class AddContributorValidator : AbstractValidator<AddContributorRequest>
{
    public AddContributorValidator()
    {
        RuleFor(x => x.ContributorId)
            .NotEmpty()
            .NotNull();
    }
}