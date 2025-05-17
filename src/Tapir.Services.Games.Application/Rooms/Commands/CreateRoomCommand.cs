using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

    public class CreateRoomCommandResult
    {
        public Guid Id { get; set; }
    }

    public interface ICreateRoomCommandHandler : ICommandHandler<CreateRoomCommand, CreateRoomCommandResult>
    {

    }

    public class CreateRoomCommandHandler : ICreateRoomCommandHandler
    {
        private readonly IAggregateRepository<RoomEntity> _roomRepository;

        public CreateRoomCommandHandler(IAggregateRepository<RoomEntity> roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<CreateRoomCommandResult> Process(CreateRoomCommand command, ClaimsPrincipal? user)
        {
            if (user.GetId() is not Guid userId)
            {
                throw new UnauthorizedAccessException();
            }

            if (user.GetName() is not string username)
            {
                throw new UnauthorizedAccessException();
            }

            var entity = new RoomEntity(Guid.NewGuid(), userId, username, new TimeControl(command.Time, command.Increment));
            
            await _roomRepository.Save(entity);

            return new CreateRoomCommandResult
            {
                Id = entity.Id
            };
        }
    }
}
