using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore.Storage.Json;

using DSED.REC.Entity;

namespace DSED.REC.WebApp.Models;

public class LeadViewModel
{
    public Guid ExternalId { get; set; }
    
    [Required(ErrorMessage = "First Name is required")]
    [StringLength(CONST.MAX_NAME_LENGTH, MinimumLength = CONST.MIN_NAME_LENGTH, ErrorMessage = CONST.FIRST_NAME_LENGTH_ERROR)]
    public string FirstName { get; set; } = string.Empty;
   
    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(CONST.MAX_NAME_LENGTH, MinimumLength = CONST.MIN_NAME_LENGTH, ErrorMessage = CONST.LAST_NAME_LENGTH_ERROR)]
    public string LastName { get; set; } = string.Empty;
    
    [EmailAddress(ErrorMessage = "Email address is not valid")]
    public string? Email { get; set; } = string.Empty;
}