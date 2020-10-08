using System;
using System.Collections.Generic;
using BecomingPrepper.Data;
using BecomingPrepper.Data.Enums;
using BecomingPrepper.Logger;
using Microsoft.AspNetCore.Mvc;

namespace BecomingPrepper.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductSelectorController : ControllerBase
    {
        private readonly ILogManager _logManager;

        public ProductSelectorController(ILogManager logManager)
        {
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
        }

        [HttpGet]
        public string Get()
        {
            return "Becoming Prepper Service";
        }

        [HttpGet]
        [Route("/[controller]/category/{category}")]
        public IActionResult GetProducts(int category)
        {
            _logManager.LogInformation($"Fetching Products for Category {category} to display on UI");
            switch (category)
            {
                case 1:
                    return Ok(GetEnumDescriptions<Grain>());
                case 2:
                    return Ok(GetEnumDescriptions<CannedOrDriedMeat>());
                case 3:
                    return Ok(GetEnumDescriptions<FatAndOil>());
                case 4:
                    return Ok(GetEnumDescriptions<Bean>());
                case 5:
                    return Ok(GetEnumDescriptions<Dairy>());
                case 6:
                    return Ok(GetEnumDescriptions<Sugar>());
                case 7:
                    return Ok(GetEnumDescriptions<CookingEssentials>());
                case 8:
                    return Ok(GetEnumDescriptions<DriedFruitAndVegetable>());
                case 9:
                    return Ok(GetEnumDescriptions<CannedFruitAndVegetable>());
                case 10:
                    return Ok(GetEnumDescriptions<Water>());
                default:
                    return BadRequest("Invalid Category");
            }
        }

        private List<string> GetEnumDescriptions<T>() where T : struct
        {
            var enumValues = Enum.GetValues(typeof(T));
            var valueDescriptions = new List<string>();
            foreach (T value in enumValues)
            {
                valueDescriptions.Add(value.GetDescription());
            }

            return valueDescriptions;
        }
    }
}
