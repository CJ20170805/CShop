using CShop.Application.DTOs;
using CShop.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        public CartController(ICartService service)
        {
            _service = service;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                        ?? throw new UnauthorizedAccessException("User ID not found in token");

            return Guid.Parse(userIdClaim);
        }

        [HttpGet]
        public async Task<ActionResult> GetByUserId()
        {
            var userId = GetUserId();
            var cart = await _service.GetByUserIdAsync(userId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<ActionResult<CartDto>> AddItem([FromBody] CartItemDto itemDto)
        {
            var userId = GetUserId();
            var cart = await _service.AddItemAsync(userId, itemDto.ProductId, itemDto.Quantity);
            return Ok(cart);
        }

        [HttpPut("items")]
        public async Task<ActionResult<CartDto>> UpdateItem([FromBody] CartItemDto itemDto)
        {
            var userId = GetUserId();
            var cart = await _service.UpdateItemAsync(userId, itemDto.ProductId, itemDto.Quantity);
            return Ok(cart);
        }

        [HttpDelete("items/{productId}")]
        public async Task<ActionResult<CartDto>> RemoveItem(Guid productId)
        {
            var userId = GetUserId();
            var cart = await _service.RemoveItemAsync(userId, productId);
            return Ok(cart);
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            await _service.ClearCartAsync(userId);
            return NoContent();
        }

    }
}
