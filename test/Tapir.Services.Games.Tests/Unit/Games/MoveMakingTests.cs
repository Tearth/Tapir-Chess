using Tapir.Core.Domain.Exceptions;
using Tapir.Services.Games.Domain.Rooms.Entities;
using Tapir.Services.Games.Domain.Rooms.Events;
using Tapir.Services.Games.Domain.Rooms.ValueObjects;

namespace Tapir.Services.Games.Tests.Unit.Rooms
{
    public class MoveMakingTests
    {
        [Test]
        public void MakeMove_ValidOneMove_EventEmitted()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userIdWhite = Guid.NewGuid();
            var userIdBlack = Guid.NewGuid();
            var usernameWhite = "test1";
            var usernameBlack = "test2";
            var timeControl = new TimeControl(10, 5);
            var entity = new GameEntity(id, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            var moveFirst = "e2e4";
            var moveShortFirst = "e4";
            var madeAtFirst = DateTime.UtcNow.AddSeconds(10);
            var fenFirst = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1";

            // Act
            entity.MakeMove(moveFirst, moveShortFirst, madeAtFirst, fenFirst);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Status, Is.EqualTo(GameStatus.InProgress));
                Assert.That(entity.Pgn, Is.EqualTo("1. e4"));
                Assert.That(entity.Fen, Is.EqualTo("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1"));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(3));
                Assert.That(entity.GetUncommittedEvents<GameStartedEvent>().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<MoveMadeEvent>().Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void MakeMove_ValidTwoMoves_EventEmitted()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userIdWhite = Guid.NewGuid();
            var userIdBlack = Guid.NewGuid();
            var usernameWhite = "test1";
            var usernameBlack = "test2";
            var timeControl = new TimeControl(10, 5);
            var entity = new GameEntity(id, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            var moveFirst = "e2e4";
            var moveShortFirst = "e4";
            var madeAtFirst = DateTime.UtcNow.AddSeconds(10);
            var fenFirst = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1";

            var moveSecond = "e7e5";
            var moveShortSecond = "e5";
            var madeAtSecond = DateTime.UtcNow.AddSeconds(20);
            var fenSecond = "rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1";

            // Act
            entity.MakeMove(moveFirst, moveShortFirst, madeAtFirst, fenFirst);
            entity.MakeMove(moveSecond, moveShortSecond, madeAtSecond, fenSecond);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Status, Is.EqualTo(GameStatus.InProgress));
                Assert.That(entity.Pgn, Is.EqualTo("1. e4 e5"));
                Assert.That(entity.Fen, Is.EqualTo("rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1"));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(4));
                Assert.That(entity.GetUncommittedEvents<GameStartedEvent>().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<MoveMadeEvent>().Count, Is.EqualTo(2));
            });
        }

        [Test]
        public void MakeMove_ValidThreeMoves_EventEmitted()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userIdWhite = Guid.NewGuid();
            var userIdBlack = Guid.NewGuid();
            var usernameWhite = "test1";
            var usernameBlack = "test2";
            var timeControl = new TimeControl(10, 5);
            var entity = new GameEntity(id, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            var moveFirst = "e2e4";
            var moveShortFirst = "e4";
            var madeAtFirst = DateTime.UtcNow.AddSeconds(10);
            var fenFirst = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1";

            var moveSecond = "e7e5";
            var moveShortSecond = "e5";
            var madeAtSecond = DateTime.UtcNow.AddSeconds(20);
            var fenSecond = "rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1";

            var moveThird = "b1c3";
            var moveShortThird = "Nc3";
            var madeAtThird = DateTime.UtcNow.AddSeconds(30);
            var fenThird = "rnbqkbnr/pppp1ppp/8/4p3/4P3/2N5/PPPP1PPP/R1BQKBNR w KQkq - 0 1";

            // Act
            entity.MakeMove(moveFirst, moveShortFirst, madeAtFirst, fenFirst);
            entity.MakeMove(moveSecond, moveShortSecond, madeAtSecond, fenSecond);
            entity.MakeMove(moveThird, moveShortThird, madeAtThird, fenThird);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Status, Is.EqualTo(GameStatus.InProgress));
                Assert.That(entity.Pgn, Is.EqualTo("1. e4 e5 2. Nc3"));
                Assert.That(entity.Fen, Is.EqualTo("rnbqkbnr/pppp1ppp/8/4p3/4P3/2N5/PPPP1PPP/R1BQKBNR w KQkq - 0 1"));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(5));
                Assert.That(entity.GetUncommittedEvents<GameStartedEvent>().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<MoveMadeEvent>().Count, Is.EqualTo(3));
            });
        }

        [Test]
        public void MakeMove_ValidFourMoves_EventEmitted()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userIdWhite = Guid.NewGuid();
            var userIdBlack = Guid.NewGuid();
            var usernameWhite = "test1";
            var usernameBlack = "test2";
            var timeControl = new TimeControl(10, 5);
            var entity = new GameEntity(id, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            var moveFirst = "e2e4";
            var moveShortFirst = "e4";
            var madeAtFirst = DateTime.UtcNow.AddSeconds(10);
            var fenFirst = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1";

            var moveSecond = "e7e5";
            var moveShortSecond = "e5";
            var madeAtSecond = DateTime.UtcNow.AddSeconds(20);
            var fenSecond = "rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1";

            var moveThird = "b1c3";
            var moveShortThird = "Nc3";
            var madeAtThird = DateTime.UtcNow.AddSeconds(30);
            var fenThird = "rnbqkbnr/pppp1ppp/8/4p3/4P3/2N5/PPPP1PPP/R1BQKBNR w KQkq - 0 1";

            var moveFourth = "g8f7";
            var moveShortFourth = "Nf7";
            var madeAtFourth = DateTime.UtcNow.AddSeconds(40);
            var fenFourth = "rnbqkb1r/pppp1ppp/5n2/4p3/4P3/2N5/PPPP1PPP/R1BQKBNR w KQkq - 0 1";

            // Act
            entity.MakeMove(moveFirst, moveShortFirst, madeAtFirst, fenFirst);
            entity.MakeMove(moveSecond, moveShortSecond, madeAtSecond, fenSecond);
            entity.MakeMove(moveThird, moveShortThird, madeAtThird, fenThird);
            entity.MakeMove(moveFourth, moveShortFourth, madeAtFourth, fenFourth);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Status, Is.EqualTo(GameStatus.InProgress));
                Assert.That(entity.Pgn, Is.EqualTo("1. e4 e5 2. Nc3 Nf7"));
                Assert.That(entity.Fen, Is.EqualTo("rnbqkb1r/pppp1ppp/5n2/4p3/4P3/2N5/PPPP1PPP/R1BQKBNR w KQkq - 0 1"));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(6));
                Assert.That(entity.GetUncommittedEvents<GameStartedEvent>().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<MoveMadeEvent>().Count, Is.EqualTo(4));
            });
        }

        [Test]
        public void MakeMove_EmptyMove_ThrowsDomainException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userIdWhite = Guid.NewGuid();
            var userIdBlack = Guid.NewGuid();
            var usernameWhite = "test1";
            var usernameBlack = "test2";
            var timeControl = new TimeControl(10, 5);
            var entity = new GameEntity(id, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            var moveFirst = "";
            var moveShortFirst = "e4";
            var madeAtFirst = DateTime.UtcNow.AddSeconds(10);
            var fenFirst = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1";

            // Act
            var act = () => entity.MakeMove(moveFirst, moveShortFirst, madeAtFirst, fenFirst);

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }

        [Test]
        public void MakeMove_EmptyMoveShort_ThrowsDomainException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userIdWhite = Guid.NewGuid();
            var userIdBlack = Guid.NewGuid();
            var usernameWhite = "test1";
            var usernameBlack = "test2";
            var timeControl = new TimeControl(10, 5);
            var entity = new GameEntity(id, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            var moveFirst = "e2e4";
            var moveShortFirst = "";
            var madeAtFirst = DateTime.UtcNow.AddSeconds(10);
            var fenFirst = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1";

            // Act
            var act = () => entity.MakeMove(moveFirst, moveShortFirst, madeAtFirst, fenFirst);

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }

        [Test]
        public void MakeMove_EmptyFen_ThrowsDomainException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userIdWhite = Guid.NewGuid();
            var userIdBlack = Guid.NewGuid();
            var usernameWhite = "test1";
            var usernameBlack = "test2";
            var timeControl = new TimeControl(10, 5);
            var entity = new GameEntity(id, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            var moveFirst = "e2e4";
            var moveShortFirst = "e4";
            var madeAtFirst = DateTime.UtcNow.AddSeconds(10);
            var fenFirst = "";

            // Act
            var act = () => entity.MakeMove(moveFirst, moveShortFirst, madeAtFirst, fenFirst);

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}