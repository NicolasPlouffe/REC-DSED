using FluentValidation;
using DSED.REC.Entity;
using DSED.REC.Entity.IDepot;

namespace DSED.REC.Application;

public class LeadServiceBL : IDisposable
{
    #region Properties
    private readonly ILeadDepot  _leadDepot;
    private readonly IValidator<LeadEntity> _validator;
    #endregion
    
    #region CONSTRUCTORS

    public LeadServiceBL(ILeadDepot leadDepot, IValidator<LeadEntity> validator)
    {
        if(leadDepot is null) throw new ArgumentNullException(nameof(leadDepot));
        if(validator is null) throw new ArgumentNullException(nameof(validator));
        
        _leadDepot = leadDepot;
        _validator = validator;
    }
    #endregion

    #region CRUD

    public async Task<LeadEntity> CreateLead(LeadEntity p_lead)
    {
        if (p_lead is null) throw new ArgumentNullException(nameof(p_lead));
        if (p_lead.Id == Guid.Empty) p_lead.Id = Guid.NewGuid();
        
        var validationResult = await _validator.ValidateAsync(p_lead);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        
        await _leadDepot.AddLeadAsync(p_lead);
        return p_lead;
    }

    public async Task<LeadEntity?> GetLeadById(Guid p_leadId)
    {
        if (p_leadId != Guid.Empty) throw new ArgumentException(nameof(p_leadId));
        
        return await _leadDepot.GetLeadByIdAsync(p_leadId);
    }

    public async Task UpdateLead(LeadEntity p_lead)
    {
        if (p_lead is null) throw new ArgumentNullException(nameof(p_lead));
        if (p_lead.Id == Guid.Empty) p_lead.Id = Guid.NewGuid();
        
        var existingLead = await _leadDepot.GetLeadByIdAsync(p_lead.Id);
        if (existingLead is null) throw new ArgumentException(nameof(p_lead));
        
        await _validator.ValidateAsync(p_lead);
        await _leadDepot.UpdateLeadAsync(p_lead);
    }
    
    #endregion CRUD
    
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}