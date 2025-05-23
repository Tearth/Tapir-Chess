namespace Tapir.Core.Chess
{
    public interface IBoard
    {
        MakeMoveResult MakeMove(string move);
    }
}
