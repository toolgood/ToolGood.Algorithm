﻿using System;

namespace ToolGood.Algorithm.UnitConversion
{
    internal sealed class UnitNotSupportedException : NotSupportedException
    {
        internal UnitNotSupportedException(string unit) : base(String.Format("The Unit '{0}' is not supported by this converter", unit))
        {
        }
    }
}