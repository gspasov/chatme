using ErrorOr;

namespace HistoryServer.ServiceErrors;
public static class Errors
{
    public static class User
    {

        public static Error UsernameAlreadyTaken => Error.Validation(
            code: "User.UsernameAlreadyTaken",
            description: "Username is already taken"
        );

        public static Error NotFound => Error.NotFound(
            code: "User.NotFound",
            description: "User not found"
        );
    }

    public static class Conversation
    {
        public static Error NotFound => Error.NotFound(
            code: "Conversation.NotFound",
            description: "Conversation not found"
        );
    }
}
