namespace SummerSchool
{
    public class Article
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public DateTime Created_at { get; set; }

        public DateTime Updated_at { get; set; } = DateTime.Now;
        public int Author_id { get; set; }
        public string Thumbnail { get; set; }   
        public string Descr { get; set; }   

    
    }
}
