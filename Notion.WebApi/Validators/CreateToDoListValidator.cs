using FluentValidation;
using Notion.Application.Models.Request;

namespace Notion.WebApi.Validators;

public class CreateToDoListValidator : AbstractValidator<CreateToDoList>
{
    public CreateToDoListValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull();
    }
}