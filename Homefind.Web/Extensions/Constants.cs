namespace Homefind.Web.Extensions
{
    public class Constants
    {
        public static readonly int ItemsPerPage = 5;
        public static readonly string SelectAnyStatus = "Any Status";
        public static readonly string SelectAllTypes = "All Types";
        public static readonly string SelectAllCities = "All Cities";
        public static readonly int[] SelectBeds = new int[] { 1, 2, 3, 4, 5 };
        public static readonly int[] SelectBaths = new int[] { 1, 2, 3, 4, 5 };
    }

    public enum MarkFavouriteAction
    {
        Add = 1,
        Remove = 2
    }
}
