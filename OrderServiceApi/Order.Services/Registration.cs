using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Order.Services
{
    public static class Registration
    {
        public static void AddPersistenceRegistration(this IServiceCollection services)
        {
            services.AddDbContext<OrderContext.OrderContext>(opt => opt.UseSqlServer(@"Server=SUDE\SQLEXPRESS;Database=OrderDb;Trusted_Connection=True;"));
        }
    }
}
