using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapir.Core.Commands;
using Tapir.Core.Identity;
using Tapir.Core.Persistence;
using Tapir.Core.Types;
using Tapir.Services.Games.Domain.Rooms.Entities;
using Tapir.Services.Games.Domain.Rooms.ValueObjects;

namespace Tapir.Services.Games.Application.Rooms.Commands
{
    public class CreateRoomCommand
    {
        public required int Time { get; set; }
        public required int Increment { get; set; }
    }

    public interface ICreateRoomCommandHandler : ICommandHandler<CreateRoomCommand, Unit>
    {

    }

    public class CreateRoomCommandHandler : ICreateRoomCommandHandler
    {
        private readonly IAggregateRepository<Room> _roomRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateRoomCommandHandler(IAggregateRepository<Room> roomRepository, IHttpContextAccessor httpContextAccessor)
        {
            _roomRepository = roomRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Process(CreateRoomCommand command)
        {
            if (_httpContextAccessor.HttpContext?.User.GetId() is not string userId)
            {
                throw new UnauthorizedAccessException();
            }

            if (_httpContextAccessor.HttpContext?.User.GetName() is not string username)
            {
                throw new UnauthorizedAccessException();
            }

            var entity = new Room(Guid.NewGuid(), Guid.Parse(userId), username, new TimeControl(command.Time, command.Increment));

            await _roomRepository.Save(entity);
            return Unit.Default;
        }
    }
}
