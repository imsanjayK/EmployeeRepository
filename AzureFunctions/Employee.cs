

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions
{
    class Count
    {
        public int total { get; set; }
    }
    public class Employee
    {
        public string id { get; set; }
        public string name { get; set; }
        public string salary { get; set; }
        public string age { get; set; }
        public string profile_image { get; set; }
    }

    class Error
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public Guid errorId { get; set; }
        public DateTime timeStamp { get; set; }
        Error(int statusCode, string message)
        {
            this.statusCode = statusCode;
            this.message = message;
            timeStamp = DateTime.UtcNow;
            errorId = Guid.NewGuid();
        }
    }
}
