using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper.EFcore.Models
{
    public abstract class BaseChange
    {
        public abstract DateTime? CreationDate { get; set; }
        public abstract DateTime? LastModifiedDate { get; set; }
    }
}
