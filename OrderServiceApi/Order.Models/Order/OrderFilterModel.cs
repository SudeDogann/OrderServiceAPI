using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Models.Order
{
    public class OrderFilterModel
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string SearchText { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<OrderStatus> Statuses { get; set; }
        public string SortBy { get; set; }
    }
}
