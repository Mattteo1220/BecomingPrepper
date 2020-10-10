﻿using System;
using BecomingPrepper.Core.ProgressTrackerProcessor;
using BecomingPrepper.Data.Enums;
using BecomingPrepper.Logger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BecomingPrepper.Api.Controllers.ProgressTracker
{
    [Route("Api/[controller]")]
    [ApiController]
    public class ProgressTrackerController : ControllerBase
    {
        private IProgressTracker _progressTracker;
        private ILogManager _logManager;
        public ProgressTrackerController(IProgressTracker progressTracker, ILogManager logManager)
        {
            _progressTracker = progressTracker ?? throw new ArgumentNullException(nameof(progressTracker));
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
        }

        // GET api/<ProgressTrackerController>/5
        [HttpGet("{accountId}/{objective}/{familySize}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get(string accountId, Objective objective, int familySize)
        {
            if (string.IsNullOrWhiteSpace(accountId)) return NotFound();
            if (familySize <= 0) return BadRequest("FamilySize must be greater than or equal to 1");

            try
            {
                return Ok(_progressTracker.GetProgress(accountId, objective, familySize));
            }
            catch (Exception e)
            {
                _logManager.LogError(e);
                return NotFound();
            }
        }
    }
}