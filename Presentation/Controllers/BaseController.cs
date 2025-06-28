using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tournament.Api.Core;

namespace Tournament.Api.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {

        protected IActionResult HandleResponse<T>(Response<T> response)
        {
            switch (response.Status)
            {
                case ResultEnum.Success:
                    return Ok(response);
                case ResultEnum.Validation:
                    return BadRequest(response);
                case ResultEnum.Error:
                    return StatusCode(500, response);
                default:
                    return NoContent();
            }
        }
    }
}
