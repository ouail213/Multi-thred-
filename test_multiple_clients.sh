#!/bin/bash
# Script to test multiple client connections

echo "Starting 3 clients..."

# Client 1
(cd WebApplication2 && echo -e "HELLO\nQUIT" | dotnet run -- client) &
sleep 1

# Client 2
(cd WebApplication2 && echo -e "PING\nQUIT" | dotnet run -- client) &
sleep 1

# Client 3
(cd WebApplication2 && echo -e "TIME\nQUIT" | dotnet run -- client) &

# Wait for all clients to finish
wait

echo "All clients finished"
