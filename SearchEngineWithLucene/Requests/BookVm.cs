namespace SearchEngineWithLucene.Requests;

public class BookVm
{
    public int Id { get; set; }
    public string Title { get; set; }

    public string Summary { get; set; }
    public string FileUrl { get; set; }
    public IFormFile File { get; set; }
}