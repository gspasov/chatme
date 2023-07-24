export type User = {
  id: string;
  username: string;
  isOnline: boolean;
};

export type UserData = {
  [id: string]: User;
};

export type Message = {
  id: string;
  senderId: string;
  receiverId: string;
  content: string;
  timestamp: number;
};

export type EstablishConnectionResponse = {
  connectionId: string;
  users: User[];
};
