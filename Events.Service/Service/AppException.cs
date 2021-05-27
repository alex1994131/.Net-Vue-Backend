using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace UserManagment.services
{
    [Serializable]
    public class AppException : Exception
    {
        public AppException()
        {
        }

        public AppException(string message) : base(message) { }
        public AppException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args)) { }

        public AppException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected AppException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}