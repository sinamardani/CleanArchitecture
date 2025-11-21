using System.ComponentModel.DataAnnotations;

namespace Domain.Commons.Enums;

public enum CrudStatus
{
    [Display(Name = nameof(Failed))]
    Failed = 0,
    [Display(Name = nameof(Succeeded))]
    Succeeded = 1,
    [Display(Name = nameof(DatabaseException))]
    DatabaseException = -1,
    [Display(Name = nameof(InputNotValid))]
    InputNotValid = -2,
    [Display(Name = nameof(NotAccess))]
    NotAccess = -3,
    [Display(Name = nameof(NotFound))]
    NotFound = -4,
    [Display(Name = nameof(IdentityError))]
    IdentityError = -5,
    [Display(Name = nameof(DeletedError))]
    DeletedError = -6,
    [Display(Name = nameof(UserNotFound))]
    UserNotFound = -7,
    [Display(Name = nameof(ServerError))]
    ServerError = -8,
    [Display(Name = nameof(ExpiryOtpCode))]
    ExpiryOtpCode = -9,
    [Display(Name = nameof(NotAuthorize))]
    NotAuthorize = -10,
}