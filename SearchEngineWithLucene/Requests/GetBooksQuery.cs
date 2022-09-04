using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SearchEngineWithLucene.Context;
using SearchEngineWithLucene.DbContext;

namespace SearchEngineWithLucene.Requests;

public class GetBooksQuery : IRequest<List<BookVm>>
{
}

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, List<BookVm>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBooksQueryHandler(ApplicationDbContext context , IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<BookVm>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        return  _mapper.Map<List<BookVm>>(await _context.Books.ToListAsync(cancellationToken)); 
    }
}
