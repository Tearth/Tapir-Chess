﻿using MediatR;
using System.ComponentModel.DataAnnotations;
using Tapir.Core.Validation;
using Tapir.Identity.Application.Auth.Commands.ResetPassword;

namespace Tapir.Identity.Application.Auth.Commands
{
    public class ResetPasswordCommand : IRequest<ResetPasswordCommandResult>
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        [EmailAddress(ErrorMessage = ValidationErrorCodes.INVALID_EMAIL)]
        public required string Email { get; set; }
    }
}
