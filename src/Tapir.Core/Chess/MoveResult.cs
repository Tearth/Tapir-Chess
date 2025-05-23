namespace Tapir.Core.Chess
{
    public class MakeMoveResult
    {
        public bool Valid { get; set; }
        public string MoveShort { get; set; }
        public string Fen { get; set; }
    }
}
