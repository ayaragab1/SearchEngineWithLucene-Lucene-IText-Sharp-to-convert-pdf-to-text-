using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SearchEngineWithLucene.Context;
using SearchEngineWithLucene.DbContext;

namespace SearchEngineWithLucene.Requests;

public class GetBookQuery:IRequest<BookVm>
{
    public int Id { get; set; }

    public GetBookQuery(int id)
    {
        Id = id;
    }
}

public class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookVm>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBookQueryHandler(ApplicationDbContext  context , IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<BookVm> Handle(GetBookQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<BookVm>(await _context.Books.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken));  
    }
}
