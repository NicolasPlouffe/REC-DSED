using DSED.REC.Entity;
namespace DSED.REC.DataAccesLayer;

public class LeadDTO
{
    #region Properties
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    #endregion
    
    #region CONSTRUCTOR

    public LeadDTO()
    {
        ;
    }

    public LeadDTO(LeadEntity lead)
    {
        this.Id = lead.Id;
        this.FirstName = lead.FirstName;
        this.LastName = lead.LastName;
        this.Email = lead.Email;
    }
    #endregion CONSTRUCTOR

    #region public methods

    public LeadEntity ToEntity()
    {
        LeadEntity lead = new LeadEntity(
            this.Id,
            this.FirstName,
            this.LastName,
            this.Email
            );

        return lead;
    }
    #endregion public methods

    #region private methods

    private static DSED.REC.Entity.Email CreateEmailSafely(string? emailValue)
    {
        if (string.IsNullOrEmpty(emailValue))
        {
            return null;
        }

        try
        {
            return DSED.REC.Entity.Email.Create(emailValue);
        }
        catch (Exception ex)
        {
            throw new InvalidCastException($"Invalid email format: {emailValue}", ex);
        }
    }

    #endregion
    
    #region override
    #endregion
}