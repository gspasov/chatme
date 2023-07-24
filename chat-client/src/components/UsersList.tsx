import { User } from "../types";

type UserListProps = {
  users: User[];
  selectedUserId?: string;
  handleUserSelect: (user: User) => void;
};

export default function UserList({
  users,
  selectedUserId,
  handleUserSelect,
}: UserListProps) {
  return (
    <div className="bg-gray-200 p-4 h-screen w-1/4">
      <h1 className="text-2xl font-bold mb-4">User List</h1>
      <ul>
        {users.length === 0 ? (
          <p>No Connected Users :(</p>
        ) : (
          users.map((user) => (
            <li
              key={user.id}
              className={`cursor-pointer mb-1 ${
                selectedUserId === user.id ? "font-bold bg-gray-300" : ""
              }`}
              onClick={() => handleUserSelect(user)}
            >
              <div className="p-2 rounded-sm hover:bg-gray-300">
                <span>{user.username}</span>{" "}
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
              </div>
            </li>
          ))
        )}
      </ul>
    </div>
  );
}
