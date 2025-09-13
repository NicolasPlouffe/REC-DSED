namespace DSED.REC.Entity;

public class LeadEntity
{
    #region Properties
    
    public Guid ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    #endregion
    
    #region CONSTRUCTOR

    public LeadEntity()
    {
        ;
    }

    public LeadEntity(Guid id, string firstName, string lastName, string email)
    {
        ID = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
    #endregion CONSTRUCTOR

    #region public methods
    #endregion public methods

    #region private methods
    #endregion
    
    #region override

    public override string ToString()
    {
        return $"Lead ID: {ID}" +
               $"First name : {FirstName}" +
               $"Last name : {LastName}" +
               $"Email : {Email}";
    }

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != typeof(LeadEntity))
        {
            return false;
        }
        return Equals((LeadEntity)obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    #endregion
}