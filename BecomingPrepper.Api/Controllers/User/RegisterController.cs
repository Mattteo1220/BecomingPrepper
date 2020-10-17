using System;
using AutoMapper;
using BecomingPrepper.Api.Objects;
using BecomingPrepper.Core.UserUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BecomingPrepper.Api.Controllers.User
{
    [Route("Api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegister _registration;
        private readonly IMapper _mapper;
        private readonly ILogManager _logger;
        public RegisterController(IRegister registration, IMapper mapper, ILogManager logger)
        {
            _registration = registration ?? throw new ArgumentNullException(nameof(registration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // POST api/<RegisterController>
        [HttpPost]
        [ThrottleFilter(nameof(Register), 100, 60)]
        public IActionResult Register([FromBody] UserRegistrationInfo user)
        {
            if (user == null) return NotFound();

            var userEntity = _mapper.Map<UserEntity>(user);
            try
            {
                _registration.Register(userEntity);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound("Username already used");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return NotFound("An Error Occurred. Please try again");
            }

        }
    }
}
