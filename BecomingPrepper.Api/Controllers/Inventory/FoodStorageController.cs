using System;
using AutoMapper;
using BecomingPrepper.Api.Objects;
using BecomingPrepper.Auth;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BecomingPrepper.Api.Controllers.Inventory
{
    [Route("Api/[controller]")]
    [ApiController]
    public class FoodStorageController : ControllerBase
    {
        private readonly IInventoryUtility _inventoryUtility;
        private readonly IMapper _mapper;
        private readonly ILogManager _logManager;
        public FoodStorageController(IInventoryUtility inventoryUtility, IMapper mapper, ILogManager logManager)
        {
            _inventoryUtility = inventoryUtility ?? throw new ArgumentNullException(nameof(inventoryUtility));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
        }

        [HttpGet]
        [ThrottleFilter(nameof(GetFoodStorageInventory), 100, 60)]
        [AuthorizePrepper(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Inventory/{accountId}")]
        public IActionResult GetFoodStorageInventory(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId)) return NotFound();
            try
            {
                var inventory = _inventoryUtility.GetInventory(accountId);
                if (inventory == null)
                {
                    return NoContent();
                }

                return Ok(inventory);
            }
            catch (Exception ex)
            {
                _logManager.LogError(ex);
                return NotFound();
            }
        }

        [HttpGet]
        [ThrottleFilter(nameof(GetInventoryItem), 100, 60)]
        [AuthorizePrepper(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Inventory/{accountId}/item/{itemId}")]
        public IActionResult GetInventoryItem(string accountId, string itemId)
        {
            if (string.IsNullOrWhiteSpace(accountId) || string.IsNullOrWhiteSpace(itemId)) return NotFound();
            try
            {
                var item = _inventoryUtility.GetInventoryItem(accountId, itemId);
                if (item == null)
                {
                    return NoContent();
                }

                return Ok(item);
            }
            catch (Exception e)
            {
                _logManager.LogError(e);
                return NotFound();
            }
        }

        [HttpPost]
        [ThrottleFilter(nameof(AddInventory), 100, 60)]
        [AuthorizePrepper(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Inventory")]
        public IActionResult AddInventory([FromBody] FoodStorageInventoryInfo foodStorageInventory)
        {
            if (foodStorageInventory == null) return NotFound();
            try
            {
                var entity = _mapper.Map<FoodStorageEntity>(foodStorageInventory);
                _inventoryUtility.AddInventory(entity);
                return Ok("Inventory Created Successfully");
            }
            catch (Exception e)
            {
                _logManager.LogError(e);
                return NotFound();
            }
        }

        [HttpPost]
        [ThrottleFilter(nameof(AddInventoryItem), 100, 60)]
        [AuthorizePrepper(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Inventory/{accountId}/item")]
        public IActionResult AddInventoryItem(string accountId, [FromBody] InventoryInfo inventoryInfo)
        {
            if (string.IsNullOrWhiteSpace(accountId) || inventoryInfo == null) return NotFound();
            try
            {
                var entity = _mapper.Map<InventoryEntity>(inventoryInfo);
                _inventoryUtility.AddInventoryItem(accountId, entity);
                return Ok("Inventory Item added Successfully");
            }
            catch (Exception e)
            {
                _logManager.LogError(e);
                return NotFound();
            }
        }

        [HttpPut]
        [ThrottleFilter(nameof(UpdateInventoryItem), 100, 60)]
        [AuthorizePrepper(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Inventory/{accountId}/item")]
        public IActionResult UpdateInventoryItem(string accountId, [FromBody] InventoryInfo inventoryInfo)
        {
            if (string.IsNullOrWhiteSpace(accountId) || inventoryInfo == null) return NotFound();

            try
            {
                var entity = _mapper.Map<InventoryEntity>(inventoryInfo);
                _inventoryUtility.UpdateInventoryItem(accountId, entity);
                return Ok($"Item: {inventoryInfo.ItemId} updated successfully");
            }
            catch (Exception e)
            {
                _logManager.LogError(e);
                return NotFound();
            }
        }

        [HttpDelete]
        [ThrottleFilter(nameof(DeleteInventory), 100, 60)]
        [AuthorizePrepper(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Inventory/{accountId}")]
        public IActionResult DeleteInventory(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId)) return NotFound();
            try
            {
                _inventoryUtility.DeleteInventory(accountId);
                return Ok("Inventory was deleted");
            }
            catch (Exception e)
            {
                _logManager.LogError(e);
                return NotFound();
            }
        }

        [HttpDelete]
        [ThrottleFilter(nameof(DeleteInventoryItem), 100, 60)]
        [AuthorizePrepper(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Inventory/{accountId}/item/{itemId}")]
        public IActionResult DeleteInventoryItem(string accountId, string itemId)
        {
            if (string.IsNullOrWhiteSpace(accountId) || string.IsNullOrWhiteSpace(itemId)) return NotFound();
            try
            {
                _inventoryUtility.DeleteInventoryItem(accountId, itemId);
                return Ok("Inventory Item was deleted");
            }
            catch (Exception e)
            {
                _logManager.LogError(e);
                return NotFound();
            }
        }
    }
}
