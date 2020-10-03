using System;
using BecomingPrepper.Core.PrepGuideUtility.Interfaces;
using BecomingPrepper.Logger;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BecomingPrepper.Api.Controllers.PrepGuide
{
    [Route("[controller]")]
    [ApiController]
    public class PilotController : ControllerBase
    {
        private readonly IPrepGuide _prepGuide;
        private readonly ILogManager _logManager;
        public PilotController(IPrepGuide prepGuide, ILogManager logManager)
        {
            _prepGuide = prepGuide ?? throw new ArgumentNullException(nameof(prepGuide));
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
        }

        // GET: api/<PrepGuideController>
        [HttpGet]
        [Route("/[controller]/Prep-Guide")]
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

        // GET api/<PrepGuideController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PrepGuideController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PrepGuideController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PrepGuideController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
