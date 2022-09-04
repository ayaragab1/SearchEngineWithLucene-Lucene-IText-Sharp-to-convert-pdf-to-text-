using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SearchEngineWithLucene.DbContext;
using SearchEngineWithLucene.Requests;
using SearchEngineWithLucene.SearchEngine;

namespace SearchEngineWithLucene.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISender _sender;

        public HomeController(ISender sender)
        {
            _sender = sender;
        }
        [HttpGet("~/")]
        [HttpGet("Book/GetBook")]
        public async Task<IActionResult> GetBook()
        {
            var model = await _sender.Send(new GetBooksQuery());
            LuceneSearch.initialize(model);
            return View(model);
        }
        [HttpGet("Book/{id}/Set")]
        public async Task<IActionResult> SetBook(int id)
        {
            
            var model = id == 0 ? new BookVm() : await _sender.Send(new GetBookQuery(id));
            return View(model); 
        }

        [HttpPost("Book/{id}/Set")]
        public async Task<IActionResult> SetBook(int id , BookVm vm)
        {
            var model =  await _sender.Send(new SetBookCommand(vm));
            if (id != 0)
            {
                LuceneSearch.ChangeInIndex(id, vm);
            }
            else
            {
                LuceneSearch.AddIndexer(vm); 
            }


            return RedirectToAction(nameof(GetBook)); 
        }


        [HttpGet("Book/{id}/Delete")]
        public async Task<IActionResult> DeleteBook(int id)
        { 
            await _sender.Send(new DeleteBookCommand(id));
            LuceneSearch.DeleteFromIndex(id); 
            return RedirectToAction(nameof(GetBook));
        }

        [HttpGet("Book/Search")]
        public IActionResult Search (string word)
        {    var res =LuceneSearch.Search(word);
            return View(res);
        }

    }
}
