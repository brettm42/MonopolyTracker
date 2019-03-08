
namespace MonopolyTracker.Server.Services.BoardService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using MonopolyTracker.Shared.Data;
    using MonopolyTracker.Shared.Models;
    using static MonopolyTracker.Shared.Constants;

    public class BoardService : IBoardService
    {
        private readonly string ServiceDirectory;
        private readonly Board currentBoard;

        private static readonly string[] TestImages =
            {
                "Freezing.png",
                "Bracing.png",
                "Chilly.png",
                "Cool.png",
                "Mild.png",
                "Warm.png",
                "Balmy.png",
                "Hot.png",
                "Sweltering.png",
                "Scorching.png"
            };

        public BoardService(string servicePath = ResourcesPath)
        {
            this.ServiceDirectory = servicePath;

            var (board, result) = Board.LoadCachedBoard(this.ServiceDirectory);
            if (result.Successful)
            {
                this.currentBoard = board;
            }

            this.currentBoard = this.LoadCurrentBoard();
        }

        public Board GetCurrentBoard() => this.LoadCurrentBoard();

        public Result AddItem(Entry entry)
        {
            return this.currentBoard.AddItem(
                new BoardItem 
                { 
                    Name = entry.Name, 
                    Count = 1,
                    Image = entry.Contents, 
                });
        }

        public Result AddItemImage(string filePath)
        {
            var test = 
                ImageClient.GetImageText(
                    new Entry
                    {
                        Name = $"Test_{DateTime.Now.Day}",
                        Contents = filePath,
                    });

            Console.WriteLine(test);

            return new Result { Successful = test.Result.Successful, Message = test.Text + test.Result.Message };
        }

        public Result RemoveItem(BoardItem entry) => this.currentBoard.RemoveItem(entry);

        public Result RemoveItem(int idx) => this.currentBoard.RemoveItem(idx);

        public IEnumerable<BoardItem> GetEntries() => this.GetCurrentBoard().GetEntries();

        public Result CacheBoard() => this.currentBoard.CacheBoard(this.ServiceDirectory);

        private Board LoadCurrentBoard()
        {
            if (this.currentBoard is null)
            {
                var rng = new Random();

                return new Board(
                    Enumerable
                    .Range(1, 5)
                    .Select(index =>
                        new BoardItem
                        {
                            Name = $"test{index}",
                            Count = rng.Next(0, 5),
                            Image = TestImages[rng.Next(TestImages.Length)]
                        }));
            }

            return this.currentBoard;
        }
    }
}
