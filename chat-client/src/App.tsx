import { useEffect, useState } from "react";
import EnterChatForm from "./components/EnterChatForm";
import UserList from "./components/UsersList";
import ChatBox from "./components/ChatBox";
import { v4 as uuidv4 } from "uuid";
import * as SignalR from "@microsoft/signalr";
import { EstablishConnectionResponse, Message, User } from "./types";

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
          console.log("[Receive Message]", {
            id,
            content,
            senderId,
            receiverId,
            timestamp,
          });

          const conversationId = buildConversationId(senderId, receiverId);
          const message: Message = {
            id,
            content,
            senderId,
            receiverId,
            timestamp,
          };
          console.log("current conversations", conversations[conversationId]);
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
        console.log(
          "[UserConnect] A NEW User connected to the system",
          connectedUser
        );

        if (currentUser) {
          const newConversations = {
            ...conversations,
            [buildConversationId(currentUser.id, connectedUser.id)]: [],
          };

          console.log("[UserConnect] conversations", newConversations);

          setConversations(newConversations);
        }
        setUsers({ ...users, [connectedUser.id]: connectedUser });
      });

      connection.on(USER_DISCONNECT_EVENT, (disconnectedUserId: string) => {
        console.log(
          "[UserConnect] A User Disconnected from the system",
          users[disconnectedUserId]
        );
        // if (selectedUser?.id === disconnectedUserId) {
        //   setSelectedUser(disconnectedUser);
        // }
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
          console.log("[EstablishConnection] My connectionId", connectionId);
          console.log(
            "[EstablishConnection] Current Users in the system",
            users
          );

          const newSelf: User = { ...myself, id: connectionId };
          const newConversations = Object.fromEntries(
            users.map((user) => [buildConversationId(newSelf.id, user.id), []])
          );
          console.log("[EstablishConnection] conversations", newConversations);

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
      const message: Message = {
        id: uuidv4(),
        content,
        senderId: sender.id,
        receiverId: receiver.id,
        timestamp: new Date().getTime(),
      };
      console.info("Message to send:", message);

      const conversationId = buildConversationId(sender.id, receiver.id);
      console.info({ sender, receiver, conversationId });

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

  function buildConversationId(id1: string, id2: string): string {
    return [id1, id2].sort().join("#");
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
