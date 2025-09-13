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
        this.Id = lead.ID;
        this.FirstName = lead.FirstName;
        this.LastName = lead.LastName;
        this.Email = lead.Email;
    }
    #endregion CONSTRUCTOR

    #region public methods
    #endregion public methods

    #region private methods
    #endregion
    
    #region override
    #endregion
}