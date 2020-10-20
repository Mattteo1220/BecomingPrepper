using System;
using System.Collections.Generic;
using BecomingPrepper.Auth;
using BecomingPrepper.Data;
using BecomingPrepper.Data.Enums;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = Microsoft.AspNetCore.Mvc.ControllerBase;

namespace BecomingPrepper.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class ProductSelectorController : ControllerBase
    {
        private readonly ILogManager _logManager;

        public ProductSelectorController(ILogManager logManager)
        {
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
        }


        [HttpGet]
        [Route("Welcome")]
        [ThrottleFilter(nameof(Welcome), 100, 60)]
        public string Welcome()
        {
            return @"Is a Web Application with an Api Service that will gradually aid ordinary citizens preparedness for emergency situations and doomsday type events, ultimately, BecomingPrepper. 
                    To aid in both the short and long term of emergency preparedness. BecomingPrepper will allow citizens to stock up on every day items such as food, water, first Aid, Ammo and track their products. 
                    Preppers will be able to Update, Add, and Delete products from their inventory as well as track their progress against a selected objective created during account registration. 
                    These objectives range from two weeks to twenty years. As their inventory grows, so too will their confidence in being prepared for the unknown.
                    Not only is this a great tracking tool for inventory needs but it is also a way to learn to live off the environment around them.
                    There are ten categories for which preppers should stock up.Some of them are Water, Grains, Sugars, Canned Fruits and Vegetables and Dried Meats.
                    With each product, a cooresponding image will be returned from the Mongo Database.These images are designed in a way to not only give the prepper a physical sense of what each product is, but teaches the prepper about where in the environment they can encounter these products.i.e. olive oil is natural to temperate climates such as the Mediterranean as well as areas in South America and Australia.
                    The image returned would be able to educate the prepper about where to find the product, seasons to harvest, preparation and long term preservation.";
        }


        [HttpGet]
        [AuthorizePrepper(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ThrottleFilter(nameof(GetProducts), 100, 60)]
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
