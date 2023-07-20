using System.Net;

namespace Common.ViewModels
{
    public class Response<T>
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public HttpStatusCode HttpStatus { get; set; }
        public T Data { get; set; }
    }
}
