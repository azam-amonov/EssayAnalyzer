using System.Data;
using System.Reflection.Metadata;
using EssayAnalyzer.Api.Models.Foundation.Users;
using EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;
using Microsoft.Extensions.Options;

namespace EssayAnalyzer.Api.Services.Foundation.Users;

public partial class UserService
{
    private static void ValidateUserOnAdd(User user)
    {
        ValidateUserIsNotNull(user);
        
        Validate((Rule: IsInvalid(user.Id), Parameter: nameof(User.Id)),
            (Rule: IsInvalid(user.FirstName), Parameter: nameof(User.FirstName)),
            (Rule: IsInvalid(user.LastName), Parameter: nameof(User.LastName)),
            (Rule: IsInvalid(user.EmailAddress), Parameter: nameof(User.EmailAddress)));
    }

    private static void ValidateUserOnModify(User user)
    {
        ValidateUserIsNotNull(user);
        
        Validate((Rule: IsInvalid(user.Id), Parameter: nameof(User.Id)),
            (Rule: IsInvalid(user.FirstName), Parameter: nameof(User.FirstName)),
            (Rule: IsInvalid(user.LastName), Parameter: nameof(User.LastName)),
            (Rule: IsInvalid(user.EmailAddress), Parameter: nameof(User.EmailAddress)));
    }

    private static void ValidateUserIsNotNull(User user)
    {
        if (user is null)
        {
            throw new NullUserException();
        }
    }

    private static void ValidateUserId(Guid id) =>
        Validate((Rule: IsInvalid(id), Parameter: nameof(User.Id)));

    private static void ValidateUserIsExists(User user, Guid id)
    {
        if (user is null)
        {
            throw new NotFoundUserException(id);
        }
    }

    private static dynamic IsInvalid(Guid id) => new
    {
        Condition = id == Guid.Empty,
        Message = "Id is required"
    };

    private static dynamic IsInvalid(string text) => new
    {
        Condition = string.IsNullOrWhiteSpace(text),
        Message = "Text is required"
    };

    private static void Validate(params (dynamic Rule, string Parameter)[] validations)
    {
        var invalidUserException = new InvalidUserException();

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidUserException.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }
        }
        
        invalidUserException.ThrowIfContainsErrors();
    }
}