using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Homefind.Core.Constants
{
    public class Constants
    {
        public static readonly int FirstPage = 1;
        public static readonly int ItemsPerPage = 5;
        public static readonly string SelectAnyStatus = "Any Status";
        public static readonly string SelectAllTypes = "All Types";
        public static readonly string SelectAllCities = "All Cities";
        public static readonly int[] SelectBeds = new int[] { 1, 2, 3, 4, 5 };
        public static readonly int[] SelectBaths = new int[] { 1, 2, 3, 4, 5 };
    }

    public enum SortOptions
    {
        [Description("Price low to high")] LowestPrice,
        [Description("Price high to low")] HighestPrice,
        [Description("Newest properties")] Newest,
        [Description("Oldest properties")] Oldest
    }

    public enum ListingType
    {
        [Description("For Rent")]
        Rental = 1,

        [Description("For Sale")]
        Selling = 2,

        [Description("Any Status")]
        All = 3
    }

    public enum NotificationType
    {
        Success,
        Error,
        None
    }

    public enum ToggleFavouritesAction
    {
        Add,
        Remove
    }

    public static class EnumHelper
    {
        public static string GetDescription(this Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }
    }
}
