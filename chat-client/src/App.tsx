import { useEffect, useState } from "react";
import EnterChatForm from "./components/EnterChatForm";
import UserList from "./components/UsersList";
import ChatBox from "./components/ChatBox";
import { v4 as uuidv4 } from "uuid";
import * as SignalR from "@microsoft/signalr";
import { EstablishConnectionResponse, Message, User } from "./types";
import { buildConversationId } from "./utls";

const CHAT_HUB_URL = "https://localhost:7154/chat-hub";
const RECEIVE_MESSAGE_EVENT = "ReceiveMessage";
const USER_CONNECT_EVENT = "UserConnect";
const USER_DISCONNECT_EVENT = "UserDisconnect";
const ESTABLISH_CONNECTION_EVENT = "EstablishConnection";

export default function App() {
  const [users, setUsers] = useState<{ [id: string]: User }>({});
  const [currentUser, setCurrentUser] = useState<User | undefined>();
  const [selectedUser, setSelectedUser] = useState<User | undefined>();
  const [conversations, setConversations] = useState<{
    [id: string]: Message[];
  }>({});
  const [connection, setConnection] = useState<
    SignalR.HubConnection | undefined
  >();

  useEffect(() => {
    if (connection && currentUser) {
      connection.on(
        RECEIVE_MESSAGE_EVENT,
        ({ id, content, senderId, receiverId, timestamp }) => {
          const conversationId = buildConversationId(senderId, receiverId);
          const message: Message = {
            id,
            content,
            senderId,
            receiverId,
            timestamp,
          };

          setConversations({
            ...conversations,
            [conversationId]: [
              ...(conversations[conversationId] ?? []),
              message,
            ],
          });
        }
      );

      connection.on(USER_CONNECT_EVENT, (connectedUser: User) => {
        if (currentUser) {
          const newConversations = {
            ...conversations,
            [buildConversationId(currentUser.id, connectedUser.id)]: [],
          };

          setConversations(newConversations);
        }
        setUsers({ ...users, [connectedUser.id]: connectedUser });
      });

      connection.on(USER_DISCONNECT_EVENT, (disconnectedUserId: string) => {
        setUsers({
          ...users,
          [disconnectedUserId]: {
            ...users[disconnectedUserId],
            isOnline: false,
          },
        });
      });
    }
  }, [connection, currentUser, selectedUser, conversations, users]);

  async function connectSignalR(myself: User): Promise<void> {
    try {
      const connection = new SignalR.HubConnectionBuilder()
        .withUrl(CHAT_HUB_URL, {
          skipNegotiation: true,
          transport: SignalR.HttpTransportType.WebSockets,
        })
        .configureLogging(SignalR.LogLevel.Information)
        .build();

      connection.on(
        ESTABLISH_CONNECTION_EVENT,
        ({ connectionId, users }: EstablishConnectionResponse) => {
          const newSelf: User = { ...myself, id: connectionId };
          const newConversations = Object.fromEntries(
            users.map((user) => [buildConversationId(newSelf.id, user.id), []])
          );

          setUsers(Object.fromEntries(users.map((user) => [user.id, user])));
          setCurrentUser(newSelf);
          setConversations(newConversations);
        }
      );

      connection.onclose((error) => {
        console.warn("Connection Closed", error);
        setConversations({});
        setUsers({});
        setCurrentUser(undefined);
      });

      await connection.start();
      await connection.invoke("Connect", myself.username);
      setConnection(connection);
    } catch (error) {
      console.error("Error starting SignalR connection:", error);
    }
  }

  function sendMessage(sender: User, receiver: User, content: string): void {
    if (connection) {
      const conversationId = buildConversationId(sender.id, receiver.id);
      const message: Message = {
        id: uuidv4(),
        content,
        senderId: sender.id,
        receiverId: receiver.id,
        timestamp: new Date().getTime(),
      };

      setConversations({
        ...conversations,
        [conversationId]: [...conversations[conversationId], message],
      });

      connection
        .invoke("SendMessage", message)
        .catch((error) => console.error("Error sending message:", error));
    }
  }

  function getSelectedUserMessages(conversationId?: string): Message[] {
    return conversationId ? conversations[conversationId] || [] : [];
  }

  function getConversationId(): string {
    return currentUser && selectedUser
      ? buildConversationId(currentUser.id, selectedUser.id)
      : "";
  }

  function handleUserSelect(user: User): void {
    setSelectedUser(user);
  }

  return (
    <div>
      {currentUser === undefined ? (
        <EnterChatForm connectSignalR={connectSignalR} />
      ) : (
        <div className="flex">
          <UserList
            users={Object.values(users)}
            selectedUserId={selectedUser?.id}
            handleUserSelect={handleUserSelect}
            conversations={conversations}
            currentUser={currentUser}
          />
          <ChatBox
            messages={getSelectedUserMessages(getConversationId())}
            currentUser={currentUser}
            selectedUser={selectedUser}
            sendMessage={sendMessage}
          />
        </div>
      )}
    </div>
  );
}
