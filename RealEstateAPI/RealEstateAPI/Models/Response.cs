﻿namespace RealEstateAPI.Models
{
    public class Response
    {
        public string Message { get; set; } =  string.Empty;
        public int Code { get; set; } = 0;
        public dynamic Data { get; set; }
        public string Error { get; set; } = string.Empty;

        public Response()
        {
                
        }

        public Response(string message, int code, dynamic data,string error)
        {
            this.Message = message;
            this.Error = error;
            this.Data = data;
            this.Code = code;
        }

    }
}
