using DSED.REC.Entity;
using DSED.REC.Entity.IDepot;
using Microsoft.EntityFrameworkCore;

namespace DSED.REC.DataAccesLayer;

public class LeadDepotDepot : ILeadDepot
{
    private readonly ApplicationDbContext _context;

    public LeadDepotDepot(ApplicationDbContext context) 
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

    public async Task<List<LeadEntity>> GetAllLeadsAsync()
    {
        if (_context is null)  throw new ArgumentNullException(nameof(_context));
        
        var dtos = await _context.LeadsDtos.ToListAsync();
        var leads = dtos.Select(dto => dto.ToEntity()).ToList();

        return leads;
    }

    public async Task UpdateLeadAsync(LeadEntity lead)
    {
        if (_context is null)  throw new ArgumentNullException(nameof(_context));
        if (lead is null) throw new ArgumentNullException(nameof(lead));
        
        var existingDto = await _context.LeadsDtos.FindAsync(lead.Id);
        if (existingDto is null) throw new ArgumentNullException(nameof(existingDto));

        existingDto.FirstName = lead.FirstName;
        existingDto.LastName = lead.LastName;
        existingDto.Email = lead.Email;

        await _context.SaveChangesAsync();
    }
}