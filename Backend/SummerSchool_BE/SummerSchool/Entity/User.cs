namespace SummerSchool
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public int Gender { get; set; }
        public DateTime dob { get; set; }   
        public int Object { get; set; }
        public string? Jwt { get; set; }
    }
}
