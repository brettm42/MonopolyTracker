
namespace MonopolyTracker.Server.Services.BoardService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using MonopolyTracker.Shared.Models;

    public class Board
    {
        const int EntryNameMaxLength = 5;
        private readonly List<BoardItem> entries = new List<BoardItem>();

        public Board(string[] entries)
        {
            this.entries = 
                entries
                    .Select(i => new BoardItem { Name = i, Count = 1 })
                    .ToList();
        }

        public Board (IEnumerable<BoardItem> entries)
        {
            this.entries = entries.ToList();
        }

        public IEnumerable<BoardItem> GetEntries() => this.entries;

        public Result AddItem(BoardItem entry)
        {
            if (entry is null || string.IsNullOrWhiteSpace(entry.Name))
            {
                return new Result { Successful = false, Message = $"Tried to add {entry} to board" };
            }

            if (entry.Name.Length > EntryNameMaxLength)
            {
                return new Result { Successful = false, Message = $"Tried to add {entry} to board but name exceeded {EntryNameMaxLength} limit" };
            }

            var existingItem = this.entries.FirstOrDefault(i => i.Equals(entry));
            if (existingItem != null)
            {
                existingItem.Count++;

                return new Result { Successful = true, Message = $"Incremented {entry}" };
            }

            this.entries.Add(entry);

            return new Result { Successful = true, Message = $"Added {entry}" };
        }

        public Result RemoveItem(BoardItem entry)
        {
            if (entry is null)
            {
                return new Result { Successful = false, Message = $"Tried to remove {entry} from the board" };
            }

            var existingItem = this.entries.FirstOrDefault(i => i.Equals(entry));
            if (existingItem != null)
            {
                existingItem.Count--;

                if (existingItem.Count < 0)
                {
                    existingItem.Count = 0;
                }

                return new Result { Successful = true, Message = $"Decremented {entry}" };
            }

            return new Result { Successful = false, Message = $"{entry} does not exist on board" };
        }

        public Result RemoveItem(int idx)
        {
            if (idx < 0)
            {
                return new Result { Successful = false, Message = $"Tried to remove item at {idx} from the board" };
            }

            var existingItem = this.entries.ElementAtOrDefault(idx);
            if (existingItem != null)
            {
                existingItem.Count--;

                if (existingItem.Count < 0)
                {
                    existingItem.Count = 0;
                }

                return new Result { Successful = true, Message = $"Decremented {existingItem} at {idx}" };
            }

            return new Result { Successful = false, Message = $"Item at {idx} does not exist on board" };
        }

        public Result CacheBoard(string serviceDirectory)
        {
            if (string.IsNullOrWhiteSpace(serviceDirectory))
            {
                return new Result { Successful = false, Message = $"Cache path was null" };
            }

            try
            {
                var directory = Path.GetFullPath(serviceDirectory);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var cacheFile = Path.Combine(directory, "cache", $"_board_{DateTime.Now.ToOADate()}.json");
                var json = JsonConvert.SerializeObject(this.entries.ToArray(), Formatting.Indented);

                File.WriteAllText(cacheFile, json);

                return File.Exists(cacheFile)
                    ? new Result { Successful = true, Message = $"Cached to {cacheFile}" }
                    : new Result { Successful = false, Message = $"Couldn't cache to {cacheFile}" };
            }
            catch (Exception ex)
            {
                return new Result { Successful = false, Message = ex.ToString() };
            }
        }

        public static (Board Board, Result Result) LoadCachedBoard(string serviceDirectory)
        {
            if (string.IsNullOrWhiteSpace(serviceDirectory))
            {
                return (default, new Result { Successful = false, Message = "Cache path was null" });
            }

            try
            {
                var directory = Path.GetFullPath(serviceDirectory);
                var cacheDirectory = Path.Combine(directory, "cache");
                if (!Directory.Exists(cacheDirectory))
                {
                    return (default, new Result { Successful = false, Message = $"Cache directory missing at {cacheDirectory}" });
                }

                var cacheFile = Directory.GetFiles(cacheDirectory).LastOrDefault(i => i.Contains("_board_"));
                if (string.IsNullOrWhiteSpace(cacheFile))
                {
                    return (default, new Result { Successful = false, Message = $"Cache at {cacheDirectory} contained no cached boards" });
                }

                return (JsonConvert.DeserializeObject<Board>(cacheFile), new Result { Successful = true, Message = "Cache loaded" });
            }
            catch (Exception ex)
            {
                return (default, new Result { Successful = false, Message = ex.ToString() });
            }
        }
    }
}
