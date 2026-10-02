# Task C: threaded TCP server and client

From the solution folder, run the server in one terminal:

```sh
dotnet run --project WebApplication2 -- server
```

In another terminal in the same folder, run a client:

```sh
dotnet run --project WebApplication2 -- client
```

Repeat the client command in more terminals to connect several clients at once.
Send HELLO from each client to see its separate server thread ID.

Commands: PING returns PONG; HELLO shows the thread ID; TIME shows the server's
local time; QUIT disconnects. Other text is echoed back.
Press Ctrl+C in the server terminal to stop the server and all client threads.

Main chooses the mode. RunServer accepts connections and creates one Thread
per client. HandleClient reads requests and sends responses. RunClient sends
keyboard input and prints replies. Using closes resources; AutoFlush sends
messages immediately.

This is a console TCP program using 127.0.0.1:5000 on your computer. Use terminals,
not a browser. Requires the .NET 8 runtime and an SDK that can build .NET 8.
