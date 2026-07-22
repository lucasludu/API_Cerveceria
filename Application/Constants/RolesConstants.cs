namespace Application.Constants
{
    public static class RolesConstants
    {
        public static string Admin => "Admin";
        public static string Docente => "Docente";
        public static string Estudiante => "Estudiante";

        public static List<string> ValidRoles => new List<string>
        {
            Admin,
            Docente,
            Estudiante
        };
    }
}
