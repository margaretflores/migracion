using System;

namespace appWcfService
{
    public class GlobalApplicationException : Exception
    {
        public int Status { get; set; }

        public int Code { get; set; }

        public String DeveloperMessage { get; set; }

        public GlobalApplicationException(int status, int code, string message, string developerMessage) : base(message)
        {
            this.Status = status;
            this.Code = code;
            this.DeveloperMessage = developerMessage;
        }
    }
}