import React, { useState } from "react";
import { User } from "../types";
import { v4 as uuidv4 } from "uuid";

type EnterChatRoomProps = {
  connectSignalR: (user: User) => Promise<void>;
};

export default function EnterChatForm({ connectSignalR }: EnterChatRoomProps) {
  const [input, setInput] = useState("");

  function handleInputChange(e: React.ChangeEvent<HTMLInputElement>): void {
    const proposedUsername = e.target.value;
    if (
      proposedUsername === "" ||
      proposedUsername.match(/^[a-zA-Z0-9_-]{1,16}$/)
    ) {
      setInput(e.target.value);
    }
  }

  function handleSubmit(e: React.FormEvent): void {
    e.preventDefault();

    if (input.trim() === "") {
      alert("Please fill in your username before entering!");
      return;
    } else if (input.trim().length < 3) {
      alert("Username should be at least 3 characters long!");
      return;
    }

    const myself = { id: uuidv4(), username: input, isOnline: true };

    connectSignalR(myself);
  }

  return (
    <div className="flex justify-center items-center min-h-screen">
      <div className="w-full max-w-sm p-6 bg-white rounded-lg shadow-md">
        <h1 className="text-2xl font-bold mb-4">Chat</h1>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <label
              htmlFor="username"
              className="block text-gray-700 font-bold mb-2"
            >
              Username
            </label>
            <input
              type="username"
              id="username"
              name="username"
              className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring focus:border-blue-300"
              placeholder="Enter your username"
              value={input}
              onChange={handleInputChange}
            ></input>
          </div>
          <button
            type="submit"
            className="w-full py-2 bg-blue-500 text-white font-bold rounded-lg hover:bg-blue-600 focus:outline-none focus:ring focus:border-blue-300"
          >
            Let's talk
          </button>
        </form>
      </div>
    </div>
  );
}
