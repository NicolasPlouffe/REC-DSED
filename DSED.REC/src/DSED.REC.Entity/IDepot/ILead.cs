using DSED.REC.Entity;
namespace DSED.REC.Entity.IDepot;

public interface ILead
{
    Task AddLeadAsync(LeadEntity lead);
    Task <LeadEntity?>GetLeadByIdAsync(Guid leadId);
    Task UpdateLeadAsync(LeadEntity lead);
}