using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Order.Models;
using Order.Models.Order;
using Order.Services.Abstractions;

namespace Order.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly OrderContext.OrderContext _orderContext;

        public OrderService(OrderContext.OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        async Task<ServiceResponse<List<Models.Order.Order>>> IOrderService.GetOrders(OrderFilterModel filter)
        {
            List<Models.Order.Order> orders = new List<Models.Order.Order>();

            try
            {
                orders = await _orderContext.Orders
                .AsQueryable()
                .Where(x => (x.CustomerName.Contains(filter.SearchText) || x.StoreName.Contains(filter.SearchText))
                        && ((filter.StartDate.HasValue && x.CreatedOn >= filter.StartDate.Value) || (!filter.StartDate.HasValue))
                        && ((filter.EndDate.HasValue && x.CreatedOn <= filter.EndDate.Value) || (!filter.EndDate.HasValue))
                        && filter.Statuses.Contains((Models.Order.OrderStatus)x.Status))
                .Skip(filter.PageNumber)
                .Take(filter.PageSize)
                .Select(x => new Models.Order.Order
                {
                    Id = x.Id,
                    BrandId = x.BrandId,
                    CreatedOn = x.CreatedOn,
                    CustomerName = x.CustomerName,
                    Price = x.Price,
                    Status = (Models.Order.OrderStatus)x.Status,
                    StoreName = x.StoreName
                })
                .ToListAsync();

                switch (filter.SortBy)
                {
                    case "Id":
                        orders = orders.OrderBy(x => x.Id).ToList();
                        break;
                    case "BrandId":
                        orders = orders.OrderBy(x => x.BrandId).ToList();
                        break;
                    case "Price":
                        orders = orders.OrderBy(x => x.Price).ToList();
                        break;
                    case "StoreName":
                        orders = orders.OrderBy(x => x.StoreName).ToList();
                        break;
                    case "CustomerName":
                        orders = orders.OrderBy(x => x.CustomerName).ToList();
                        break;
                    case "CreatedOn":
                        orders = orders.OrderBy(x => x.CreatedOn).ToList();
                        break;
                    case "Status":
                        orders = orders.OrderBy(x => x.Status).ToList();
                        break;   
                    default:
                        return new ServiceResponse<List<Models.Order.Order>>
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Gönderilen sıralama alanı ismi filtreleme alanları ile eşleşmiyor."
                        };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Models.Order.Order>>
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Message = $"İşlem hatalı. Mesaj: {ex.Message}"
                };
            }
            

            if (orders.Count == 0)
            {
                return new ServiceResponse<List<Models.Order.Order>>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Aranan kriterlere uygun sipariş bulunamadı"
                };
            }

            return new ServiceResponse<List<Models.Order.Order>>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Value = orders
            };
        }

        async Task<ServiceResponse<int>> IOrderService.SaveOrders(List<Models.Order.Order> orders)
        {
            int recordCount = 0;
            if (orders == null || orders.Count == 0)
            {
                return new ServiceResponse<int>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Sipariş listesi boş olamaz"
                };
            }

            var entityList = orders.Where(x=> x.BrandId != 0).Select(x => new Entity.Order
            {
                BrandId = x.BrandId,
                CreatedOn = x.CreatedOn,
                CustomerName = x.CustomerName,
                Price = x.Price,
                Status = (Entity.OrderStatus)x.Status,
                StoreName = x.StoreName
            });

            _orderContext.Orders.AddRange(entityList);

            try
            {
                recordCount = await _orderContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ServiceResponse<int>
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Message = $"İşlem hatalı. Mesaj: {ex.Message}",
                    Value = recordCount
                };
            }

            if (recordCount > 0)
            {
                return new ServiceResponse<int>
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = $"İşlem başarılı. Kaydedilen sipariş sayısı: {recordCount}",
                    Value = recordCount
                };
            }

            return new ServiceResponse<int>
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Message = "Kaydedilemedi",
                Value = recordCount
            };
        }
    }
}
