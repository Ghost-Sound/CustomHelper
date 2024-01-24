using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper.Exception
{
    [Serializable]
    public class CustomException : System.Exception
    {
        public object ObjectValue { get; private set; }

        public CustomException()
        {
        }

        public CustomException(string message)
            : base(message)
        {
        }

        public CustomException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        public CustomException(string message, object objectValue)
            : base(message)
        {
            ObjectValue = objectValue;
        }

        public CustomException(string message, object objectValue, System.Exception innerException)
            : base(message, innerException)
        {
            ObjectValue = objectValue;
        }

        public override string ToString()
        {
            string baseString = base.ToString();
            if (ObjectValue != null)
            {
                return $"{baseString}\nObject Type: {ObjectValue.GetType().FullName}\nObject Value: {ObjectValue.ToString()}";
            }
            return baseString;
        }

    }
}
