import { Message, User } from "../types";
import { buildConversationId } from "../utls";

type UserListProps = {
  users: User[];
  selectedUserId?: string;
  handleUserSelect: (user: User) => void;
  conversations: {
    [id: string]: Message[];
  };
  currentUser: User;
};

export default function UserList({
  users,
  selectedUserId,
  handleUserSelect,
  conversations,
  currentUser,
}: UserListProps) {
  function getLastMessageContent(userId: string): string {
    const conversationId = buildConversationId(userId, currentUser.id);
    const messages = conversations[conversationId];
    if (messages.length === 0) return "";

    const lastMessage = conversations[conversationId].reduce(
      (acc, currentMessage) => {
        return currentMessage.timestamp > acc.timestamp ? currentMessage : acc;
      }
    );

    return lastMessage.senderId === currentUser.id
      ? `You: ${lastMessage.content}`
      : lastMessage.content;
  }

  return (
    <div className="p-2 h-screen max-w-xl border-r border-gray-400 bg-neutral-50">
      <h1 className="text-2xl font-bold p-2">Chats</h1>
      <ul>
        {users.length === 0 ? (
          <p>No Connected Users :(</p>
        ) : (
          users.map((user) => (
            <li
              key={user.id}
              className={`cursor-pointer rounded-xl mb-1 font-bold ${
                selectedUserId === user.id ? "bg-gray-200" : ""
              }`}
              onClick={() => handleUserSelect(user)}
            >
              <div className="p-3 rounded-xl hover:bg-gray-200">
                <span>{user.username}</span>{" "}
                <div className="flex items-center justify-between">
                  {user.isOnline ? (
                    <div className="flex items-center">
                      <div className="w-2 h-2 rounded-full mr-2 bg-green-700"></div>
                      <span className="text-green-700 font-semibold text-sm">
                        Online
                      </span>
                    </div>
                  ) : (
                    <div className="flex items-center">
                      <div className="w-2 h-2 rounded-full mr-2 bg-red-700"></div>
                      <span className="text-red-700 font-semibold text-sm">
                        Offline
                      </span>
                    </div>
                  )}
                  <div className="text-sm w-40 text-right font-normal truncate">
                    {getLastMessageContent(user.id)}
                  </div>
                </div>
              </div>
            </li>
          ))
        )}
      </ul>
    </div>
  );
}
