using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore.Storage.Json;

using DSED.REC.Entity;

namespace DSED.REC.WebApp.Models;

public class LeadViewModel
{
    #region Properties
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "First Name is required")]
    [StringLength(CONST.MAX_NAME_LENGTH, MinimumLength = CONST.MIN_NAME_LENGTH, ErrorMessage = CONST.FIRST_NAME_LENGTH_ERROR)]
    public string FirstName { get; set; } = string.Empty;
   
    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(CONST.MAX_NAME_LENGTH, MinimumLength = CONST.MIN_NAME_LENGTH, ErrorMessage = CONST.LAST_NAME_LENGTH_ERROR)]
    public string LastName { get; set; } = string.Empty;
    
    [EmailAddress(ErrorMessage = "Email address is not valid")]
    public string Email { get; set; } = string.Empty;
    #endregion Properties
    #region Constructor

    public LeadViewModel()
    {
        ;
    }

    public LeadViewModel(LeadEntity lead)
    {
        Id = lead.Id;
        FirstName = lead.FirstName;
        LastName = lead.LastName;
        Email = lead.Email;
    }
    #endregion Constructor
    #region Public Methods

    public LeadEntity ToLeadEntity()
    {
        return new LeadEntity(
            this.Id,
            this.FirstName,
            this.LastName,
            this.Email
        );
    }
    #endregion Public Methods
}