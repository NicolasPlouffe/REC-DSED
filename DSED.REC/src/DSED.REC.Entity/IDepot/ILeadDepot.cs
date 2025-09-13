using DSED.REC.Entity;
namespace DSED.REC.Entity.IDepot;

public interface ILeadDepot
{
    Task AddLeadAsync(LeadEntity lead);
    Task <LeadEntity?>GetLeadByIdAsync(Guid leadId);
    Task UpdateLeadAsync(LeadEntity lead);
}