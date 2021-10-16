using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Web.Utilities.Exceptions
{
    public class WebException : Exception
    {
        public WebException()
        {
        }

        public WebException(string message) : base(message)
        {
        }

        public WebException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WebException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}