using pax.chess;
using Tapir.Core.Chess;

namespace Tapir.Providers.Chess.PaxChess.Chessboard
{
    public class Board : IBoard
    {
        private readonly Game _game;

        public Board()
        {
            _game = new Game();
        }

        public void LoadFromFen(string fen)
        {
            _game.LoadFen(fen);
        }

        public void LoadFromPgn(string pgn)
        {
            _game.LoadPgn(pgn);
        }

        public bool Move(string longNotation)
        {
            if (longNotation.Length < 4 || longNotation.Length > 5)
            {
                return false;
            }

            var from = FieldToPosition(longNotation.Substring(0, 2));
            var to = FieldToPosition(longNotation.Substring(2, 2));
            var promotion = SymbolToPieceType(longNotation.Length == 5 ? longNotation.Substring(3, 1) : null);
            var move = new EngineMove(from, to, promotion);

            return _game.Move(move) == MoveState.Ok;
        }

        private Position FieldToPosition(string field)
        {
            return new Position(
                (byte)(field[0] - 'a'),
                (byte)(field[1] - '1')
            );
        }

        private PieceType? SymbolToPieceType(string? symbol)
        {
            switch (symbol?.ToLower())
            {
                case "n": return PieceType.Knight;
                case "b": return PieceType.Bishop;
                case "r": return PieceType.Rook;
                case "q": return PieceType.Queen;
            }

            return null;
        }
    }
}
