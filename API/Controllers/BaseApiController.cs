using API.Helpers;
using Microsoft.AspNetCore.Mvc;                                                                                   // as we inherit from ControllerBase

namespace API.Controllers
{

    [ServiceFilter(typeof(LogUserActivity))]                                       // ServiceFilter is now available to all controllers and all actions will make use of this controller
    [ApiController]                                                                                                             // attributes specified here
    [Route ("api/[controller]")]                                                                                            // to get to controller, users specify api/controller 
    public class BaseApiController : ControllerBase                                                         // always inherit from ControllerBase
    {
        
    }
}