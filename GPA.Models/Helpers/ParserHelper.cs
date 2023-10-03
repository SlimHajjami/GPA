using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPA.Models
{
    public static class ParseHelper
    {

        public static int parseToInt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            int entier;
            return int.TryParse(value, out entier) ? entier : 0;
        }
    }
}
