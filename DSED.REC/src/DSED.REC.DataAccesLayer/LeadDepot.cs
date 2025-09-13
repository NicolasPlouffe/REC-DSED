using DSED.REC.Entity;
using DSED.REC.Entity.IDepot;
using Microsoft.EntityFrameworkCore;

namespace DSED.REC.DataAccesLayer;

public class LeadDepot : ILead
{
    private readonly ApplicationDbContext _context;

    public LeadDepot(ApplicationDbContext context) 
        => _context = context ?? throw new ArgumentNullException(nameof(context));
    
        
    public async Task AddLeadAsync(LeadEntity lead)
    {
        if (_context is null)  throw new ArgumentNullException(nameof(_context));
        if (lead is null) throw new ArgumentNullException(nameof(lead));
        
        var dto = new LeadDTO(lead);
        await _context.AddAsync(dto);
        await _context.SaveChangesAsync();
        
        lead.Id = dto.Id;
    }

    public async Task<LeadEntity?> GetLeadByIdAsync(Guid leadId)
    {
        if (_context is null)  throw new ArgumentNullException(nameof(_context));
        
        var dto = await _context.LeadsDtos.FirstOrDefaultAsync(d => d.Id == leadId);
        return dto?.ToEntity();
    }

    public Task UpdateLeadAsync(LeadEntity lead)
    {
        if (_context is null)  throw new ArgumentNullException(nameof(_context));
        if (lead is null) throw new ArgumentNullException(nameof(lead));
    }
}