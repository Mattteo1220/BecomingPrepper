using System;
using BecomingPrepper.Api.Objects;
using BecomingPrepper.Core.UserUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BecomingPrepper.Api.Controllers.User
{
    [Route("Api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceAccount _serviceAccount;
        private readonly ILogManager _logger;
        public UserController(IServiceAccount serviceAccount, ILogManager logger)
        {
            _serviceAccount = serviceAccount ?? throw new ArgumentNullException(nameof(serviceAccount));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //Api get Account Details
        [HttpGet]
        [ThrottleFilter(nameof(GetAccountDetails), 100, 60)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("account-management/{accountId}")]
        public IActionResult GetAccountDetails(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId)) return NotFound();
            try
            {
                UserEntity user = _serviceAccount.GetAccountDetails(accountId);
                if (user != null)
                {
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return NotFound();
            }

            return NoContent();
        }

        // Patch Update Email
        [HttpPatch]
        [ThrottleFilter(nameof(PatchEmail), 100, 60)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("account-Management/{accountId}/email")]
        public IActionResult PatchEmail(string accountId, [FromBody] ECommunication ecomm)
        {
            if (string.IsNullOrWhiteSpace(accountId) || ecomm == null) return NotFound();
            try
            {
                _serviceAccount.UpdateEmail(accountId, ecomm.NewEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return NotFound();
            }

            return Ok("Email Successfully updated.");
        }

        // PATCH Update FamilySize
        [HttpPatch]
        [ThrottleFilter(nameof(PatchFamilySize), 100, 60)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("account-management/{accountId}/familySize")]
        public IActionResult PatchFamilySize(string accountId, [FromBody] Family family)
        {
            if (string.IsNullOrWhiteSpace(accountId)) return NotFound();
            if (family.Size <= 0) return BadRequest("Family size not supported. Must be greater than or equal to 1");

            try
            {
                _serviceAccount.UpdateFamilySize(accountId, family.Size);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return NotFound();
            }

            return Ok("Family size updated successfully");
        }

        // PATCH Update FamilySize
        [HttpPatch]
        [ThrottleFilter(nameof(PatchObjective), 100, 60)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("account-management/{accountId}/objective")]
        public IActionResult PatchObjective(string accountId, [FromBody] Scheme scheme)
        {
            if (string.IsNullOrWhiteSpace(accountId)) return NotFound();

            try
            {
                _serviceAccount.UpdateObjective(accountId, scheme.Objective);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return NotFound();
            }

            return Ok("Objective updated successfully");
        }

        // PATCH Update FamilySize
        [HttpPatch]
        [ThrottleFilter(nameof(ChangePassword), 100, 60)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("account-management/{accountId}/credentials")]
        public IActionResult ChangePassword(string accountId, [FromBody] Objects.Authentication authentication)
        {
            if (string.IsNullOrWhiteSpace(accountId)) return NotFound();
            if (authentication == null) return NotFound();

            try
            {
                _serviceAccount.UpdatePassword(accountId, authentication.Password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return NotFound();
            }

            return Ok("Password updated successfully");
        }
    }
}
