using Microsoft.AspNetCore.Mvc;
using FluentValidation;

using DSED.REC.Application;
using DSED.REC.Common;
using DSED.REC.DataAccesLayer;
using DSED.REC.Entity;
using DSED.REC.Entity.IDepot;

using DSED.REC.WebApp.Models;

namespace DSED.REC.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeadController : ControllerBase
{
    private readonly ILeadDepot _leadDepot;
    private readonly LeadServiceBL _leadServiceBL;
    private readonly LeadMessageProducer _leadMessageProducer;
    
    #region Constructor

    public LeadController(ILeadDepot leadDepot,LeadServiceBL leadServiceBL)
    {
        _leadDepot = leadDepot ?? throw new ArgumentNullException(nameof(leadDepot));
        _leadServiceBL = leadServiceBL ?? throw new ArgumentNullException(nameof(leadServiceBL));
        _leadMessageProducer = new LeadMessageProducer("lead-exchange", "DSED.REC.LeadCRM");
    }
    #endregion

    #region CRUD
    #region Get

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<LeadEntity>>> GetAllLeads()
    {
        try
        {
            var leads = await _leadDepot.GetAllLeadsAsync();
            return Ok(leads);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<LeadEntity>> GetLeadById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Lead ID cannot be empty"); 
        }

        try
        {
            var lead = await _leadDepot.GetLeadByIdAsync(id);
            if (lead == null)
            {
                return NotFound($"Lead with {id} not found"); 
            }
            return Ok(lead);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    #endregion Get
    
    #region Post

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<LeadEntity>> CreateLead(LeadEntity lead)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        _leadMessageProducer.PublishCreate(lead);
        return CreatedAtAction(nameof(GetLeadById), new { id = lead.Id }, lead);
    }
    #endregion Post

    #region Put

    [HttpPut("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateLeadById(Guid id, LeadEntity lead)
    {
        if ( !ModelState.IsValid || id == Guid.Empty) 
        {
            return BadRequest("Lead ID cannot be empty");
        }
        _leadMessageProducer.PublishUpdate(lead);
        return NoContent();
    }
    
    #endregion Put    
    #endregion CRUD
}