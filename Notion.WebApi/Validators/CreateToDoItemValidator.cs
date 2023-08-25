using FluentValidation;
using Notion.Application.Models.Request;

namespace Notion.WebApi.Validators;

public class CreateToDoItemValidator : AbstractValidator<CreateToDoItem>
{
    public CreateToDoItemValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull();
    }
}