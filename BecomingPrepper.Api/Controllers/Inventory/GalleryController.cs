using System;
using System.Threading.Tasks;
using BecomingPrepper.Auth;
using BecomingPrepper.Core.ImageResourceHelper;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using BecomingPrepper.Security.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BecomingPrepper.Api.Controllers.Inventory
{
    [Route("Api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly IGridFSHelper _gridFsHelper;
        private readonly ILogManager _logManager;
        public GalleryController(IGridFSHelper gridFsHelper, ILogManager logManager)
        {
            _gridFsHelper = gridFsHelper ?? throw new ArgumentNullException(nameof(gridFsHelper));
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
        }

        // GET api/<GalleryController>/5
        [HttpGet]
        [ThrottleFilter(Endpoint.GetImage)]
        [AuthorizePrepper(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route(("Image/{itemId}"))]
        public async Task<IActionResult> GetImage(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId)) return BadRequest();
            try
            {
                var chunkEntity = await Task.Run(() => _gridFsHelper.GetChunks(itemId));
                return chunkEntity != null ? Ok(chunkEntity) : throw new ArgumentException($"ItemId: {itemId} not Found");
            }
            catch (Exception ex)
            {
                _logManager.LogError(ex);
                return NoContent();
            }
        }
    }
}
