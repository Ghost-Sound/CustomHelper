﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper.ValidationHelper.Interfaces
{
    public interface IValidationRule
    {
        bool IsValid(object value);
    }
}
