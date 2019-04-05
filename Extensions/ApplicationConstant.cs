using System;
namespace Auction.Extensions
{
    public class ApplicationConstant
    {
        /// <summary>
        /// new DateTime(1900, 1, 1)
        /// </summary>
        public static readonly DateTime DateTimeMinValue = new DateTime(1900, 1, 1);
        /// <summary>
        /// DateTime.MaxValue; = public static readonly DateTime MaxValue;
        /// </summary>
        public static readonly DateTime DateTimeMaxValue = DateTime.MaxValue;

        /// <summary>
        /// -1
        /// </summary>
        public const int intDefaultValue = -1;
        /// <summary>
        /// int.MaxValue; = public const Int32 MaxValue = 2147483647;
        /// </summary>
        public const int intMaxValue = int.MaxValue;

        /// <summary>
        /// -1
        /// </summary>
        public const long longDefaultValue = -1;
        /// <summary>
        /// long.MaxValue; = public const Int64 MaxValue = 9223372036854775807;
        /// </summary>
        public const long longMaxValue = long.MaxValue;


        /// <summary>
        /// -1
        /// </summary>
        public const decimal decimalDefaultValue = -1;
        /// <summary>
        /// decimal.MaxValue; = public const Decimal MaxValue = 79228162514264337593543950335M;
        /// </summary>
        public const decimal decimalMaxValue = decimal.MaxValue;

        /// <summary>
        /// -1
        /// </summary>
        public const double doubleDefaultValue = -1;
        /// <summary>
        /// double.MaxValue; = public const Double MaxValue = 1.7976931348623157E+308;
        /// </summary>
        public const double doubleMaxValue = double.MaxValue;


        /// <summary>
        /// -1
        /// </summary>
        public const float floatDefaultValue = -1;
        /// <summary>
        /// float.MaxValue; = public const Single MaxValue = 3.40282347E+38F;
        /// </summary>
        public const float floatMaxValue = float.MaxValue;

        /// <summary>
        /// false
        /// </summary>
        public const bool boolDefaultValue = false;
    }
}