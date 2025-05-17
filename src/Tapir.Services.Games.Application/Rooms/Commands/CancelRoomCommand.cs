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
    public class CancelRoomCommand
    {
        public required Guid Id { get; set; }
    }

    public interface ICancelRoomCommandHandler : ICommandHandler<CancelRoomCommand, Unit>
    {

    }

    public class CancelRoomCommandHandler : ICancelRoomCommandHandler
    {
        private readonly IAggregateRepository<RoomEntity> _roomRepository;

        public CancelRoomCommandHandler(IAggregateRepository<RoomEntity> roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<Unit> Process(CancelRoomCommand command, ClaimsPrincipal? user)
        {
            var entity = await _roomRepository.Load(command.Id);

            if (user != null)
            {
                if (entity.UserId != user.GetId())
                {
                    throw new UnauthorizedAccessException();
                }
            }

            entity.Cancel();

            await _roomRepository.Save(entity);
            return Unit.Default;
        }
    }
}
