namespace SummerSchool  
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descr { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public string Instructor { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public string Thumbnail { get; set; }
    }
}
