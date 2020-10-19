using System;
using System.Web.Mvc;
using BecomingPrepper.Core.TokenService.Interface;
using BecomingPrepper.Logger;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = Microsoft.AspNetCore.Mvc.ControllerBase;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BecomingPrepper.Api.Controllers.Token
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginDataController : ControllerBase
    {
        private readonly ILoginDataService _loginDataRetrievalService;
        private readonly ILogManager _logManager;
        public LoginDataController(ILoginDataService loginDataRetrievalService, ILogManager logManager)
        {
            _loginDataRetrievalService = loginDataRetrievalService ?? throw new ArgumentNullException(nameof(loginDataRetrievalService));
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
        }

        // GET: api/<TokenService>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult FetchAuthorizationToken(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId)) return NotFound();
            try
            {
                var login = _loginDataRetrievalService.FetchLastLoginData(accountId);
                login.LoginStamp = DateTime.Parse(login.LoginStamp.ToLocalTime().ToString("G"));
                return Ok(login);
            }
            catch (Exception e)
            {
                _logManager.LogError(e);
                return NotFound();
            }
        }
    }
}
