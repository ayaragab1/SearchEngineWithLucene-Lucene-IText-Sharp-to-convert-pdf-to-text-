using Microsoft.EntityFrameworkCore;
using SearchEngineWithLucene.DbContext;

namespace SearchEngineWithLucene.Context;
public class ApplicationDbContext: Microsoft.EntityFrameworkCore.DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Book> Books { get; set;  }
}