using System;
using System.Web.Http;
using BecomingPrepper.Api.Objects;
using BecomingPrepper.Core.UserUtility;
using BecomingPrepper.Logger;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BecomingPrepper.Api.Controllers.User
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _login;
        private readonly ILogManager _logger;
        public LoginController(ILogin login, ILogManager logger)
        {
            _login = login ?? throw new ArgumentNullException(nameof(login));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // POST api/<UserController>
        [HttpPost]
        public IActionResult Post([FromBody] Credentials credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Username) || string.IsNullOrWhiteSpace(credentials.Password)) return NotFound();
            try
            {
                var result = _login.Authenticate(credentials.Username, credentials.Password);
                if (result)
                {
                    return Ok("AccountId: " + _login.AccountId);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return NotFound();
            }
        }
    }
}
