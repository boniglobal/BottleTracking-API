using System.Net;

namespace Core.Models
{
    public class ResponseModels
    {
        public class Messaging
        {
            public static Response GetResponse(bool success, int? code, string message = null, object data = null)
            {
                return new Response
                {
                    Success = success,
                    Status = new Status
                    {
                        Code = code ?? (int)HttpStatusCode.OK,
                        Message = message
                    },
                    Data = data
                };
            }
        }

        public class Response
        {
            public bool Success { get; set; }
            public Status Status { get; set; }
            public object Data { get; set; }
            public string Timestamp { get; set; }
        }

        public class Status
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }
    }
}
