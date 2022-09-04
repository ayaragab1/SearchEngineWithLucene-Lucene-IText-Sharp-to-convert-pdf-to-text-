using AutoMapper;
using MediatR;
using SearchEngineWithLucene.Context;
using SearchEngineWithLucene.DbContext;
using SearchEngineWithLucene.helpers;

namespace SearchEngineWithLucene.Requests;

public class SetBookCommand:IRequest<int>
{
    public BookVm Book { get; set; }

    public SetBookCommand(BookVm book)
    {
        Book = book;
    }
}

public class SetBookCommandHandler : IRequestHandler<SetBookCommand, int>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SetBookCommandHandler(ApplicationDbContext context , IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> Handle(SetBookCommand request, CancellationToken cancellationToken)
    {

        if (request.Book.File != null)
        {
            if (!string.IsNullOrWhiteSpace(request.Book.FileUrl) && FileManager.Exist(request.Book.FileUrl,"uploads"))
            {
                FileManager.Delete(request.Book.FileUrl, "uploads");
            }
            request.Book.FileUrl = await FileManager.Upload(request.Book.File, "uploads", true);
        }
        var model = _mapper.Map<Book>(request.Book); 
        if (request.Book.Id == 0)
        {
            _context.Books.Add(model); 
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            _context.Books.Update(model);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return request.Book.Id; 
    }
}