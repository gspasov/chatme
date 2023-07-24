import { useState } from "react";
import MessageBubble from "./MessageBubble";
import { Message, User } from "../types";

type ChatBoxProps = {
  currentUser: User;
  selectedUser?: User;
  messages: Message[];
  sendMessage: (sender: User, receiver: User, content: string) => void;
};

export default function ChatBox({
  selectedUser,
  currentUser,
  messages,
  sendMessage,
}: ChatBoxProps) {
  const [newMessage, setNewMessage] = useState("");

  const handleMessageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setNewMessage(e.target.value);
  };

  const handleSendMessage = (sender: User, receiver: User, content: string) => {
    if (content.trim() !== "") {
      sendMessage(sender, receiver!!, content);
      setNewMessage("");
    }
  };

  const sendBtnOfflineStyles = "bg-gray-500";
  const sendBtnOnlineStyles = "bg-blue-500 hover:bg-blue-600";

  return (
    <div className="bg-white p-4 h-screen w-3/4 flex flex-col">
      {selectedUser ? (
        <>
          <h1 className="text-2xl font-bold mb-4">
            Chat with {`User ${selectedUser.username}`}
          </h1>
          <div className="overflow-y-scroll flex-grow px-2">
            {messages.map((message) => (
              <MessageBubble
                key={message.id}
                isSender={message.senderId === currentUser?.id}
                content={message.content}
              />
            ))}
          </div>
          <form
            className="flex mt-4"
            onSubmit={(e) => {
              e.preventDefault();
              handleSendMessage(currentUser, selectedUser, newMessage);
            }}
          >
            <input
              type="text"
              value={newMessage}
              onChange={handleMessageChange}
              className="w-full px-3 py-2 border rounded-lg mr-2 focus:outline-none focus:ring focus:border-blue-300"
              placeholder="Type your message..."
            />
            <button
              type="submit"
              className={"px-4 py-2 text-white font-bold rounded-lg focus:outline-none focus:ring focus:border-blue-300 ".concat(
                selectedUser.isOnline
                  ? sendBtnOnlineStyles
                  : sendBtnOfflineStyles
              )}
              disabled={!selectedUser.isOnline}
            >
              Send
            </button>
          </form>
        </>
      ) : (
        <div className="flex items-center justify-center h-screen">
          <div className="text-gray-600 text-center">
            <h2 className="text-2xl font-bold mb-4">
              No Conversation selected
            </h2>
            <p className="text-lg">
              Click on a name to start a conversation with your friend!
            </p>
          </div>
        </div>
      )}
    </div>
  );
}
