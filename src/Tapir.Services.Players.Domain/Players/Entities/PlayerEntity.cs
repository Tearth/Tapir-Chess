using System.Text.RegularExpressions;
using Tapir.Core.Domain;
using Tapir.Core.Domain.Exceptions;
using Tapir.Core.Validation;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.Players.Domain.Players.Entities
{
    public class PlayerEntity : AggregateRoot
    {
        public DateTime CreatedAt { get; private set; }
        public string? Username { get; private set; }
        public string? Email { get; private set; }
        public string? Country { get; private set; }
        public string? AboutMe { get; private set; }

        public PlayerEntity()
        {

        }

        public PlayerEntity(Guid id)
        {
            var @event = new PlayerCreatedEvent(id, DateTime.UtcNow);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public void SetUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new DomainException("Username is required.", "EmptyCountry");
            }

            var @event = new PlayerUsernameUpdatedEvent(Id, username);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new DomainException("EMail is required.", "EmptyEmail");
            }

            if (!EmailValidator.IsValid(email))
            {
                throw new DomainException("Email is not valid.", "InvalidEmail");
            }

            var @event = new PlayerEmailUpdatedEvent(Id, email);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public void SetCountry(string country)
        {
            if (string.IsNullOrEmpty(country))
            {
                throw new DomainException("Country is required.", "EmptyCountry");
            }

            var @event = new PlayerCountryUpdatedEvent(Id, country);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public void SetAboutMe(string aboutMe)
        {
            if (string.IsNullOrEmpty(aboutMe))
            {
                throw new DomainException("About Me is required.", "EmptyAboutMe");
            }

            var @event = new PlayerAboutMeUpdatedEvent(Id, aboutMe);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public override void ApplyEvent(DomainEvent @event)
        {
            switch (@event)
            {
                case PlayerCreatedEvent playerCreatedEvent: ExecuteEvent(playerCreatedEvent); break;
                case PlayerUsernameUpdatedEvent usernameUpdatedEvent: ExecuteEvent(usernameUpdatedEvent); break;
                case PlayerEmailUpdatedEvent emailUpdatedEvent: ExecuteEvent(emailUpdatedEvent); break;
                case PlayerCountryUpdatedEvent countryUpdatedEvent: ExecuteEvent(countryUpdatedEvent); break;
                case PlayerAboutMeUpdatedEvent aboutMeUpdatedEvent: ExecuteEvent(aboutMeUpdatedEvent); break;
            }

            base.ApplyEvent(@event);
        }

        private void ExecuteEvent(PlayerCreatedEvent @event)
        {
            Id = @event.AggregateId;
            CreatedAt = @event.CreatedAt;
        }

        private void ExecuteEvent(PlayerUsernameUpdatedEvent @event)
        {
            Username = @event.Username;
        }

        private void ExecuteEvent(PlayerEmailUpdatedEvent @event)
        {
            Email = @event.Email;
        }

        private void ExecuteEvent(PlayerCountryUpdatedEvent @event)
        {
            Country = @event.Country;
        }

        private void ExecuteEvent(PlayerAboutMeUpdatedEvent @event)
        {
            AboutMe = @event.AboutMe;
        }
    }
}
