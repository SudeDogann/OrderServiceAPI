using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Models.Order;
using Order.Services.Abstractions;

namespace OrderServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrder([FromBody] List<Order.Models.Order.Order> orders) 
        {
            var response = await _orderService.SaveOrders(orders);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(response.Message);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        [HttpPost("Query")]
        public async Task<IActionResult> GetOrders([FromBody] OrderFilterModel filter) 
        {
            var response = await _orderService.GetOrders(filter);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(response.Value);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }
    }
}
