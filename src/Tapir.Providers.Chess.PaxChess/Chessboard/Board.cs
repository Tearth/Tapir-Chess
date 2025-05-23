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

        public MakeMoveResult MakeMove(string move)
        {
            if (move.Length < 4 || move.Length > 5)
            {
                return new MakeMoveResult
                {
                    Valid = false
                };
            }

            var from = FieldToPosition(move.Substring(0, 2));
            var to = FieldToPosition(move.Substring(2, 2));
            var promotion = SymbolToPieceType(move.Length == 5 ? move.Substring(3, 1) : null);
            var result = _game.Move(new EngineMove(from, to, promotion));

            if (result != MoveState.Ok)
            {
                return new MakeMoveResult
                {
                    Valid = false
                };
            }
            
            return new MakeMoveResult
            {
                Valid = true,
                MoveShort = _game.State.CurrentMove!.PgnMove,
                Fen = Fen.MapList(_game.State)
            };
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
