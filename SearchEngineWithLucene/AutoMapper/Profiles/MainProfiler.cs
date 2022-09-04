using AutoMapper;
using SearchEngineWithLucene.DbContext;
using SearchEngineWithLucene.Requests;

namespace SearchEngineWithLucene.AutoMapper.Profiles
{
    public class MainProfiler : Profile
    {
        public MainProfiler()
        {
            CreateMap<Book, BookVm>().ReverseMap();
        }
    }
}