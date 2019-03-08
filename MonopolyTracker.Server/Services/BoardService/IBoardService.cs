
namespace MonopolyTracker.Server.Services.BoardService
{
    using System.Collections.Generic;
    using MonopolyTracker.Shared.Models;

    public interface IBoardService
    {
        Board GetCurrentBoard();

        IEnumerable<BoardItem> GetEntries();

        Result AddItem(Entry entry);

        Result AddItemImage(string filePath);

        Result RemoveItem(BoardItem entry);

        Result RemoveItem(int idx);

        Result CacheBoard();
    }
}