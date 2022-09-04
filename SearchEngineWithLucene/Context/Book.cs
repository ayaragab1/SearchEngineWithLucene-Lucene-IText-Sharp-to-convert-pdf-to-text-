namespace SearchEngineWithLucene.DbContext;

public class Book
{
    public int Id { get; set; }
    public string  Title{ get; set; }

    public string Summary { get; set; }
    public string FileUrl { get; set; }
}