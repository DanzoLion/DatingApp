using Microsoft.AspNetCore.Mvc;                                                                                   // as we inherit from ControllerBase

namespace API.Controllers
{

    [ApiController]                                                                                                             // attributes specified here
    [Route ("api/[controller]")]                                                                                            // to get to controller, users specify api/controller 
    public class BaseApiController : ControllerBase                                                         // always inherit from ControllerBase
    {
        
    }
}