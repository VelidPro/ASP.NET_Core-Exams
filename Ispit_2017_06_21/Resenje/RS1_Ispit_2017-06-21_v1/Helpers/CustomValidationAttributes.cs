using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RS1_Ispit_2017_06_21_v1.Helpers
{
    public class FutureDateTime: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime)
            {

                DateTime.TryParse(value.ToString(), out var date);

                if (date <= DateTime.Now)
                    return true;
            }

            return false;
        }
    }
}