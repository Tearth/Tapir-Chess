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
    public class CloseRoomCommand
    {
        public required Guid Id { get; set; }
        public required Guid GameId { get; set; }
    }

    public interface ICloseRoomCommandHandler : ICommandHandler<CloseRoomCommand, Unit>
    {

    }

    public class CloseRoomCommandHandler : ICloseRoomCommandHandler
    {
        private readonly IAggregateRepository<RoomEntity> _roomRepository;

        public CloseRoomCommandHandler(IAggregateRepository<RoomEntity> roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<Unit> Process(CloseRoomCommand command)
        {
            var entity = await _roomRepository.Load(command.Id);

            entity.Close(command.GameId);

            await _roomRepository.Save(entity);
            return Unit.Default;
        }
    }
}
