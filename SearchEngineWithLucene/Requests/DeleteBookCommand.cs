using MediatR;
using SearchEngineWithLucene.Context;

namespace SearchEngineWithLucene.Requests;

public class DeleteBookCommand:IRequest<bool>
{
    public int Id { get; set; }

    public DeleteBookCommand(int id)
    {
        Id = id;
    }
}

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand ,bool>
{
    private readonly ApplicationDbContext _context;

    public DeleteBookCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FindAsync(request.Id);

        if (book==null)
            return false; 


        _context.Books.Remove(book);
        await _context.SaveChangesAsync(cancellationToken); 
        return true;
    }
}
