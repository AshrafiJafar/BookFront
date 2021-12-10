using Api;
using BookFront.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookFront.Controllers
{
    public class BookController : Controller
    {
        private readonly IClient client;

        public BookController(IClient client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexViewModel
            {
                Types = (await client.GetTypesAsync()).ToList(),
                Authors = (await client.GetAuthorsAsync()).ToList(),
                Books = (await client.GetBooksAsync(null,null,null,"")).ToList(),
            };
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new IndexViewModel
            {
                Types = (await client.GetTypesAsync()).ToList(),
                Authors = (await client.GetAuthorsAsync()).ToList(),
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookCreateCommand command)
        {
            await client.CreateBookAsync(command);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> GetSubTypes(Guid typeId)
        {
            var subTypes = await client.GetSubTypesForTypeAsync(typeId);
            return PartialView("_SubTypeOptions", subTypes);
        }


        public async Task<IActionResult> Search(Guid? typeId, Guid? subTypeId, Guid? authorId, string search)
        {
            var model = await client.GetBooksAsync(typeId, subTypeId, authorId, search);
            return PartialView("_TableBody", model);
        }

        public async Task<IActionResult> SearchWithProcedure(Guid? typeId, Guid? subTypeId, Guid? authorId, string search)
        {
            var model = await client.GetBooksWithStoreProcedureAsync(typeId, subTypeId, authorId, search);
            return PartialView("_TableBody", model);
        }


        public async Task<IActionResult> Privacy()
        {

            var clientHandler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
            var httpCclient = new HttpClient(clientHandler);
            var result = await httpCclient.GetAsync(
                "https://api.stackexchange.com/2.3/answers?order=desc&sort=activity&site=stackoverflow");
            var res = await result.Content.ReadAsStringAsync();
            var root = JsonConvert.DeserializeObject<Root>(res);
            return View(root);
        }

      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
