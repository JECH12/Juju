using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Business.Dto
{
    public class ResponseServiceDto<T>
    {
        public string Message { get; set; }
        public T Result { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }

    }
}
