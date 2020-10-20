using System;
using System.Web.Http;
using BecomingPrepper.Api.Authentication;
using BecomingPrepper.Api.Objects;
using BecomingPrepper.Core.TokenService.Interface;
using BecomingPrepper.Core.UserUtility;
using BecomingPrepper.Data.Entities.Logins;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BecomingPrepper.Api.Controllers.User
{
    [Route("Api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginUtility _login;
        private readonly ILoginDataService _loginDataService;
        private readonly ILogManager _logger;
        private readonly ITokenManager _tokenManager;
        public LoginController(ILoginUtility login, ILoginDataService loginDataService, ILogManager logger, ITokenManager tokenManager)
        {
            _login = login ?? throw new ArgumentNullException(nameof(login));
            _loginDataService = loginDataService ?? throw new ArgumentNullException(nameof(loginDataService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
        }

        // POST api/<UserController>
        [HttpPost]
        [AllowAnonymous]
        [ThrottleFilter(nameof(Login), 100, 60)]
        public IActionResult Login([FromBody] Credentials credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Username) || string.IsNullOrWhiteSpace(credentials.Password)) return NotFound();
            try
            {
                var result = _login.Authenticate(credentials.Username, credentials.Password);
                if (!result) return Unauthorized();

                var token = _tokenManager.Generate(_login.AccountId, _login.Email);
                _tokenManager.CreateCookie("Email", _login.Email, Response, false, false);
                _tokenManager.CreateCookie("AccountId", _login.AccountId, Response, false, false);
                var login = _loginDataService.FetchLastLoginData(_login.AccountId);
                if (login == null)
                {
                    var loginData = new Login()
                    {
                        AccessToken = token,
                        AccountId = _login.AccountId,
                        LoginStamp = DateTime.Now
                    };
                    _loginDataService.CreateLoginData(loginData);
                }

                if (login?.LoginStamp <= DateTime.Now.AddHours(-8))
                {
                    _loginDataService.RefreshToken(_login.AccountId, token);
                }
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return NotFound();
            }
        }
    }
}
