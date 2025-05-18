namespace Tapir.Core.Chess
{
    public interface IBoardFactory
    {
        IBoard CreateFromStartPosition();
        IBoard CreateFromFen(string fen);
        IBoard CreateFromPgn(string pgn);
    }
}
