using pax.chess;
using Tapir.Core.Chess;

namespace Tapir.Providers.Chess.PaxChess.Chessboard
{
    public class BoardFactory : IBoardFactory
    {
        public IBoard CreateFromStartPosition()
        {
            var board = new Board();
            board.LoadFromFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            return board;
        }

        public IBoard CreateFromFen(string fen)
        {
            var board = new Board();

            if (!string.IsNullOrEmpty(fen))
            {
                board.LoadFromFen(fen);
            }

            return board;
        }

        public IBoard CreateFromPgn(string pgn)
        {
            var board = new Board();

            if (!string.IsNullOrEmpty(pgn))
            {
                board.LoadFromPgn(pgn);
            }

            return board;
        }
    }
}
