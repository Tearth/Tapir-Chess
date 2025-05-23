using Tapir.Core.Domain.Exceptions;
using Tapir.Services.Games.Domain.Rooms.Entities;
using Tapir.Services.Games.Domain.Rooms.Events;
using Tapir.Services.Games.Domain.Rooms.ValueObjects;

namespace Tapir.Services.Games.Tests.Unit.Games
{
    public class GameCreationTests
    {
        [Test]
        public void CreateGame_ValidGame_EventEmitted()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userIdWhite = Guid.NewGuid();
            var userIdBlack = Guid.NewGuid();
            var usernameWhite = "test1";
            var usernameBlack = "test2";
            var timeControl = new TimeControl(10, 5);

            // Act
            var entity = new GameEntity(id, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Id, Is.EqualTo(id));
                Assert.That(entity.UserIdWhite, Is.EqualTo(userIdWhite));
                Assert.That(entity.UserIdBlack, Is.EqualTo(userIdBlack));
                Assert.That(entity.UsernameWhite, Is.EqualTo(usernameWhite));
                Assert.That(entity.UsernameBlack, Is.EqualTo(usernameBlack));
                Assert.That(entity.TimeControl, Is.EqualTo(timeControl));
                Assert.That(entity.TimeWhite, Is.EqualTo(timeControl.Time * 60 * 1000));
                Assert.That(entity.TimeBlack, Is.EqualTo(timeControl.Time * 60 * 1000));
                Assert.That(entity.Fen, Is.EqualTo("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<GameCreatedEvent>().Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void CreateGame_EmptyUsernameWhite_ThrowsDomainException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userIdWhite = Guid.NewGuid();
            var userIdBlack = Guid.NewGuid();
            var usernameWhite = "";
            var usernameBlack = "test2";
            var timeControl = new TimeControl(10, 5);

            // Act
            var act = () => new GameEntity(id, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }

        [Test]
        public void CreateGame_EmptyUsernameBlack_ThrowsDomainException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userIdWhite = Guid.NewGuid();
            var userIdBlack = Guid.NewGuid();
            var usernameWhite = "test1";
            var usernameBlack = "";
            var timeControl = new TimeControl(10, 5);

            // Act
            var act = () => new GameEntity(id, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }

        [Test]
        public void CreateGame_TimeControlWithZeroValues_ThrowsDomainException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userIdWhite = Guid.NewGuid();
            var userIdBlack = Guid.NewGuid();
            var usernameWhite = "test1";
            var usernameBlack = "test2";
            var timeControl = new TimeControl(0, 0);

            // Act
            var act = () => new GameEntity(id, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }

        [Test]
        public void CreateGame_TimeControlWithNegativeTime_ThrowsDomainException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userIdWhite = Guid.NewGuid();
            var userIdBlack = Guid.NewGuid();
            var usernameWhite = "test1";
            var usernameBlack = "test2";
            var timeControl = new TimeControl(-1, 5);

            // Act
            var act = () => new GameEntity(id, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }

        [Test]
        public void CreateGame_TimeControlWithNegativeIncrement_ThrowsDomainException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userIdWhite = Guid.NewGuid();
            var userIdBlack = Guid.NewGuid();
            var usernameWhite = "test1";
            var usernameBlack = "test2";
            var timeControl = new TimeControl(10, -1);

            // Act
            var act = () => new GameEntity(id, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }

        [Test]
        public void CreateGame_TimeControlWithTimeTooHigh_ThrowsDomainException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userIdWhite = Guid.NewGuid();
            var userIdBlack = Guid.NewGuid();
            var usernameWhite = "test1";
            var usernameBlack = "test2";
            var timeControl = new TimeControl(int.MaxValue, 5);

            // Act
            var act = () => new GameEntity(id, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }

        [Test]
        public void CreateGame_TimeControlWithIncrementTooHigh_ThrowsDomainException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userIdWhite = Guid.NewGuid();
            var userIdBlack = Guid.NewGuid();
            var usernameWhite = "test1";
            var usernameBlack = "test2";
            var timeControl = new TimeControl(10, int.MaxValue);

            // Act
            var act = () => new GameEntity(id, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}