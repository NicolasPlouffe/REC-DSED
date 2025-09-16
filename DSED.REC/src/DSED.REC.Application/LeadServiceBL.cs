using FluentValidation;
using DSED.REC.Entity;
using DSED.REC.Entity.IDepot;

namespace DSED.REC.Application;

public class LeadServiceBL : IDisposable 
{
    #region Properties
    private readonly ILeadDepot _leadDepot;
    private readonly IValidator<LeadEntity> _validator;
    #endregion
    
    #region CONSTRUCTORS
    public LeadServiceBL(ILeadDepot leadDepot, IValidator<LeadEntity> validator)
    {
        _leadDepot = leadDepot ?? throw new ArgumentNullException(nameof(leadDepot));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }
    #endregion

    #region CRUD
    public async Task<LeadEntity> CreateLead(LeadEntity lead)
    {
        if (lead is null) throw new ArgumentNullException(nameof(lead));
        if (lead.Id == Guid.Empty) lead.Id = Guid.NewGuid();
        
        var validationResult = await _validator.ValidateAsync(lead);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        
        await _leadDepot.AddLeadAsync(lead);
        return lead;
    }

    public async Task<LeadEntity?> GetLeadById(Guid leadId)
    {
        if (leadId == Guid.Empty) throw new ArgumentException(nameof(leadId)); 
        
        return await _leadDepot.GetLeadByIdAsync(leadId);
    }

    public async Task<List<LeadEntity>> GetAllLeadsAsync()
    {
        return await _leadDepot.GetAllLeadsAsync();
    }
    
    public async Task UpdateLead(LeadEntity lead)
    {
        if (lead is null) throw new ArgumentNullException(nameof(lead));
        if (lead.Id == Guid.Empty) throw new ArgumentException("Lead ID cannot be empty for update", nameof(lead));
        
        var existingLead = await _leadDepot.GetLeadByIdAsync(lead.Id);
        if (existingLead is null) throw new ArgumentException("Lead not found", nameof(lead));
        
        var validationResult = await _validator.ValidateAsync(lead);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        
        await _leadDepot.UpdateLeadAsync(lead);
    }
    #endregion CRUD
    
    public void Dispose()
    {
    }
}
