using System;
using AutoMapper;
using BecomingPrepper.Api.Objects;
using BecomingPrepper.Core.PrepGuideUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Logger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BecomingPrepper.Api.Controllers.PrepGuide
{
    [Route("Api/[controller]")]
    [ApiController]
    public class PilotController : ControllerBase
    {
        private readonly IPrepGuide _prepGuide;
        private readonly IMapper _mapper;
        private readonly ILogManager _logManager;
        public PilotController(IPrepGuide prepGuide, IMapper mapper, ILogManager logManager)
        {
            _prepGuide = prepGuide ?? throw new ArgumentNullException(nameof(prepGuide));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
        }

        // GET: api/<PrepGuideController>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("prep-guide")]
        public IActionResult GetPrepGuide()
        {
            try
            {
                var prepGuide = _prepGuide.GetPrepGuide();
                if (prepGuide == null)
                {
                    return NoContent();
                }

                return Ok(prepGuide);
            }
            catch (Exception ex)
            {
                _logManager.LogError(ex);
                return NotFound();
            }
        }

        // POST api/<PrepGuideController>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("prep-guide/tip")]
        public IActionResult Post([FromBody] TipInfo tip)
        {
            if (tip == null) return NotFound();
            try
            {
                var entity = _mapper.Map<TipEntity>(tip);
                _prepGuide.Add(ObjectId.Empty, entity);
                return Ok($"tip {tip.Name} added Successfully");
            }
            catch (Exception ex)
            {
                _logManager.LogError(ex);
                return NotFound();
            }
        }

        // DELETE api/<PrepGuideController>/5
        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("prep-guide/tip/{tipId}")]
        public IActionResult Delete(string tipId)
        {
            if (string.IsNullOrWhiteSpace(tipId)) return NotFound();
            try
            {
                _prepGuide.Delete(ObjectId.Empty, tipId);
                return Ok($"Tip {tipId} successfully removed");
            }
            catch (Exception e)
            {
                _logManager.LogError(e);
                return NotFound();
            }
        }
    }
}
