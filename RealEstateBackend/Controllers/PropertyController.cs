using Microsoft.AspNetCore.Mvc;
using RealEstateBackend.Models;

namespace RealEstateBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private static readonly List<Property> Properties = new()
        {
            new Property { Id = 1, Title = "Дом у озера", Description = "Просторный двухэтажный дом", Price = 12500000, ImageUrl = "/images/house1.jpg", Location = "Коттеджный поселок 'Озерный'" },
            new Property { Id = 2, Title = "Лесной дом", Description = "Уютный одноэтажный дом", Price = 8200000, ImageUrl = "/images/house2.jpg", Location = "Коттеджный поселок 'Лесная поляна'" }
        };

        [HttpGet]
        public IActionResult GetProperties()
        {
            return Ok(Properties);
        }

        [HttpGet("{id}")]
        public IActionResult GetProperty(int id)
        {
            var property = Properties.FirstOrDefault(p => p.Id == id);
            return property == null ? NotFound() : Ok(property);
        }
    }
}