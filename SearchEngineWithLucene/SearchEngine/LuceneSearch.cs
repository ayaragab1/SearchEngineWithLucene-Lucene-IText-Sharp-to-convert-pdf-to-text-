using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using SearchEngineWithLucene.Requests;
using Directory = System.IO.Directory;
using TextField = Lucene.Net.Documents.TextField;

namespace SearchEngineWithLucene.SearchEngine;

public static class LuceneSearch
{
    public static string indexFileLocation = "Index";
    public const LuceneVersion Version = LuceneVersion.LUCENE_48;

    public static Lucene.Net.Store.Directory dir =FSDirectory.Open(new DirectoryInfo(indexFileLocation));
    public static Lucene.Net.Analysis.Analyzer analyzer =new StandardAnalyzer(Version);
    public static IndexWriter IndexWriter { get; set;  }

    public static void initialize(IEnumerable<BookVm> books)
    {
        if (!Directory.Exists("Index"))
        {
            IndexWriterConfig config = new(Version, analyzer);
            IndexWriter = new(dir, config);

            foreach (var book in books)
            {
                var text = pdfText(@"D:\Aya\VHM\SearchEngineWithLucene\SearchEngineWithLucene\wwwroot\uploads\" + book.FileUrl);


                var doc = new Document
                {
                    new StringField("Id", book.Id.ToString(), Field.Store.YES),
                    new TextField("Title", book.Title, Field.Store.YES),
                    new TextField("Summary", book.Summary, Field.Store.NO ),
                    new TextField("File", text, Field.Store.YES),
                };

                IndexWriter.AddDocument(doc);
            }

            IndexWriter.Commit();
            IndexWriter.Dispose();
        }
     
    }

    public static void AddIndexer(BookVm book)
    {
        IndexWriterConfig config = new(Version, analyzer);
        IndexWriter = new(dir, config);

        var text = pdfText(@"D:\Aya\VHM\SearchEngineWithLucene\SearchEngineWithLucene\wwwroot\uploads\" + book.FileUrl);


        var doc = new Document
        {
            new StringField("Id", book.Id.ToString(), Field.Store.YES),
            new TextField("Title", book.Title, Field.Store.YES),
            new TextField("Summary", book.Summary, Field.Store.NO),
            new TextField("File", text, Field.Store.YES),

        };

        IndexWriter.AddDocument(doc);
        IndexWriter.Commit();
        IndexWriter.Dispose();


    }

    public static void ChangeInIndex(int id, BookVm book)
    {
        IndexWriterConfig config = new(Version, analyzer);
        IndexWriter = new(dir, config);
        var text = pdfText(@"D:\Aya\VHM\SearchEngineWithLucene\SearchEngineWithLucene\wwwroot\uploads\" + book.FileUrl);

        var doc = new Document
        {
            new StringField("Id", id.ToString(), Field.Store.YES),
            new TextField("Title", book.Title, Field.Store.YES),
            new TextField("Summary", book.Summary, Field.Store.NO),
            new TextField("File", text, Field.Store.YES),

        };

        IndexWriter.UpdateDocument( new Term("Id" , id.ToString()),  doc);
        IndexWriter.Commit();
        IndexWriter.Dispose();
    }


    public static void DeleteFromIndex(int id)
    {

        IndexWriterConfig config = new(Version, analyzer);
        IndexWriter = new(dir, config);
        IndexWriter.DeleteDocuments(new Term("Id", id.ToString()));

        IndexWriter.Commit();
        IndexWriter.Dispose();
    }

    public static IEnumerable<BookVm> Search(string searchTerm)
    {
        IndexWriterConfig config = new(Version, analyzer);
        IndexWriter = new(dir, config);
        var directoryReader = DirectoryReader.Open(dir);

        var indexSearcher = new IndexSearcher(directoryReader);
        string[] fields = { "Id", "Title", "Summary","File"};
        var queryParser = new MultiFieldQueryParser(Version, fields, analyzer);
        var query = queryParser.Parse(searchTerm);
        var hits = indexSearcher.Search(query, 1000).ScoreDocs;
        var books = new List<BookVm>();

        foreach (var hit in hits)
        {
            var document = indexSearcher.Doc(hit.Doc);
            books.Add(new BookVm
            {
                Id =int.Parse(document.Get("Id")),
                Title = document.Get("Title"),
                Summary = document.Get("Summary"),
                FileUrl = document.Get("File")


            });
        }
        IndexWriter.Commit();
        IndexWriter.Dispose();
        return books;



    }
    public static string pdfText(string path)
    {
        string text = string.Empty;

        if (File.Exists(path))
        {
            PdfReader reader = new PdfReader(path);
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                text += PdfTextExtractor.GetTextFromPage(reader, page);
            }
            reader.Close();
        }
        return text;

    }
}