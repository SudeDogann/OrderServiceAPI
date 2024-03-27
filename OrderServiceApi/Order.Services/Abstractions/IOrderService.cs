using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Order.Models;
using Order.Models.Order;

namespace Order.Services.Abstractions
{
    public interface IOrderService
    {
        Task<ServiceResponse<int>> SaveOrders(List<Models.Order.Order> orders);
        Task<ServiceResponse<List<Models.Order.Order>>> GetOrders(OrderFilterModel filter);
    }
}
