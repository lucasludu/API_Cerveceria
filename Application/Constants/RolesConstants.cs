namespace Application.Constants
{
    public static class RolesConstants
    {
        public static string Admin => "Admin";
        public static string Brewery => "Brewery";
        public static string Wholesaler => "Wholesaler";
        public static string Client => "Client";

        public static List<string> ValidRoles => new List<string>
        {
            Admin,
            Brewery,
            Wholesaler,
            Client
        };
    }
}
