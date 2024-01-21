using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper.Exception
{
    [Serializable]
    public class DataNotModifiedException : CustomException
    {
        public string AdditionalMessage { get; set; }

        public DataNotModifiedException() : base("The data have not been changed.")
        {
            AdditionalMessage = string.Empty;
        }

        public DataNotModifiedException(string additionalMessage) : base("The data have not been changed.")
        {
            AdditionalMessage = additionalMessage;
        }

        public DataNotModifiedException(string message, string additionalMessage) : base(message)
        {
            AdditionalMessage = additionalMessage;
        }

        public DataNotModifiedException(string message, System.Exception innerException) : base(message, innerException)
        {
            AdditionalMessage = string.Empty;
        }

        public DataNotModifiedException(string message, string additionalMessage, System.Exception innerException) : base(message, innerException)
        {
            AdditionalMessage = additionalMessage;
        }
    }
}
