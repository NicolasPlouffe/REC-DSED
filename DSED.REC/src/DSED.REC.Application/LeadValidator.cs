using FluentValidation;
using DSED.REC.Entity;

namespace DSED.REC.Application;

public class LeadValidator : AbstractValidator<LeadEntity>
{
    public LeadValidator()
    {
        RuleFor(lead => lead.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Lead ID must not be empty");
        
        RuleFor(lead => lead.FirstName)
            .NotEmpty()
            .WithMessage("Lead First Name cannot be empty")
            .Length(CONST.MIN_NAME_LENGTH, CONST.MAX_NAME_LENGTH)
            .WithMessage(
                $"First name must be between {CONST.MIN_NAME_LENGTH} and {CONST.MAX_NAME_LENGTH} characters long");
        
        RuleFor(lead => lead.LastName)
            .NotEmpty()
            .WithMessage("Lead Last Name cannot be empty")
            .Length(CONST.MIN_NAME_LENGTH, CONST.MAX_NAME_LENGTH)
            .WithMessage(
                $"Last name must be between {CONST.MIN_NAME_LENGTH} and {CONST.MAX_NAME_LENGTH} characters long");

        RuleFor(lead => lead.Email)
            .NotEmpty()
            .WithMessage("Lead Email cannot be empty")
            .EmailAddress()
            .WithMessage("Provide a valid email address"); 
    }
}