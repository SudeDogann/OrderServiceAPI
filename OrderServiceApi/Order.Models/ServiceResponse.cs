using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Order.Models
{
    public class ServiceResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Value { get; set; }
    }
}
