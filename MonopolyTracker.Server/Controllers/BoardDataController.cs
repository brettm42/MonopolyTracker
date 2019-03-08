
namespace MonopolyTracker.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.FileProviders;
    using MonopolyTracker.Server.Services.BoardService;
    using MonopolyTracker.Shared.Models;
    using static MonopolyTracker.Shared.Constants;

    [Route("api/[controller]")]
    public class BoardDataController : Controller
    {
        //private readonly IFileProvider fileProvider;
        private readonly IBoardService boardService;

        //public BoardDataController(IBoardService service, IFileProvider fileProvider)
        public BoardDataController(IBoardService service)
        {
            this.boardService = service;    

            //this.fileProvider = fileProvider;
            //this.boardService.SetServiceDirectory(
                //this.fileProvider.GetFileInfo(@"./Resources/BoardService"));
        }

        [HttpGet("[action]")]
        public IEnumerable<BoardItem> GetBoardItems() => this.boardService.GetEntries();

        [HttpGet("[action]")]
        public Result AddTicket() => 
            this.boardService.AddItem(
                new Entry
                {
                    Name = $"test{DateTime.Now.Second}",
                    Contents = $"ImgData{DateTime.Now.Day}.png",
                });

        [HttpGet("[action]")]
        public Result AddTicketImage() =>
            this.boardService.AddItemImage(ResourcesPath + "/test.jpg");

        [HttpGet("[action]")]
        [Route("api/[controller]/[action]/{id}")]
        public Result RemoveTicket(int id) => this.boardService.RemoveItem(id);

        [HttpGet("[action]")]
        public Result CacheBoard() => this.boardService.CacheBoard();
    }
}
