
namespace MonopolyTracker.Shared.Models
{
    public class Result
    {
        public bool Successful { get; set; }

        public string Message { get; set; }

        public override string ToString() => 
            $"Result was{(this.Successful ? string.Empty : " not")} successful, {this.Message}";
    }
}
