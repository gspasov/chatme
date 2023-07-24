# ChatMe

Simple chat application developed using [SignalR](https://dotnet.microsoft.com/en-us/apps/aspnet/signalr).

## Components

- ChatServer
  - This server is responsible for the Chat communication with the Client Application. Keeps messages in memory and persists the message history to the HistoryServer every 5 minutes.
- HistoryServer
  - Responsible for keeping the Users in system and all the message history
- Client
  - Simple chat UI setup via React.


## How to run

1. First make sure you have the database running:
```bash
docker compose up
```
2. Run the migrations in the HistoryServer:
```bash
$> cd chat-backend/HistoryServer
$HistoryServer> dotnet ef database update
```
3. Install dependencies and run the HistoryServer:
```bash
$HistoryServer> dotnet restore
$HistoryServer> dotnet run
```
4. Install dependencies and run the ChatServer:
```bash
$> cd ../ChatServer
$ChatServer> dotnet restore
$ChatServer> dotnet run
```
5. Install Client dependencies and run Client Application:
```bash
$> cd ../../chat-client
$chat-client> npm install
$chat-client> npm start
```

## HistoryServer API

To access the Swagger API docs go to `https://localhost:7030/swagger/index.html`