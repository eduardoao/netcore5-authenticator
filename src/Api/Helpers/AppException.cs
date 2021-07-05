using System;
using System.Globalization;

namespace WebApi.Helpers
{    
    // classe de exceção personalizada para lançar exceções específicas do aplicativo (por exemplo, para validação)
    // que pode ser capturado e tratado dentro do aplicativo
    public class AppException : Exception
    {
        public AppException() : base() {}

        public AppException(string message) : base(message) { }

        public AppException(string message, params object[] args) 
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}