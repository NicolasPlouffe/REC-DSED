using Microsoft.AspNetCore.Mvc;

using DSED.REC.Application;
using DSED.REC.DataAccesLayer;
using DSED.REC.Entity.IDepot;

using DSED.REC.WebApp.Models;

namespace DSED.REC.WebApp.Controllers;

public class LeadController : Controller
{
    private readonly LeadServiceBL _leadServiceBL;
    private readonly ILogger<LeadController> _logger;

    #region Constructor

    public LeadController(LeadServiceBL leadServiceBL, ILogger<LeadController> logger)
    {
        _leadServiceBL = leadServiceBL ?? throw new ArgumentNullException(nameof(leadServiceBL));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));;
    }
    #endregion

    #region CRUD
    #region Get
    
    #endregion Get
    
    #region Post
    
    #endregion Post

    #region Put
    
    #endregion Put
    #endregion CRUD
}