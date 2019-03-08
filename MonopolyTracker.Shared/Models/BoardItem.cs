
namespace MonopolyTracker.Shared.Models
{
    using Newtonsoft.Json;

    public class BoardItem
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public int Count { get; set; }

        [JsonIgnore]
        public bool OnBoard => this.Count > 0;

        public bool Equals(Entry obj) => this.Name == obj.Name;

        public bool Equals(BoardItem obj) => this.Name == obj.Name;

        public override string ToString() => $"{Name}_{Count}_{Image}";
    }
}
