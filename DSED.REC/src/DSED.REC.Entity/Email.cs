using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DSED.REC.Entity;

public sealed record Email
{
    [EmailAddress]
    public string EmailAdress { get; set; }
    private Email() { EmailAdress = string.Empty; }
    private Email(string p_emailAdress) => EmailAdress = p_emailAdress;
    
    public static Email Create(string emailAdress)
    {
        if (string.IsNullOrWhiteSpace(emailAdress)) throw new ArgumentNullException(nameof(emailAdress));
        
        var emailChecker = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
        if (!emailChecker.IsValid(emailAdress)) throw new ArgumentException("Email adress is not valid form", nameof(emailAdress));
        
        return new Email(emailAdress.ToLowerInvariant());
    }
    
    public static implicit operator string(Email email) => email.EmailAdress;
}