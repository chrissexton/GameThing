#!/bin/sh
dotnet restore src/Server
dotnet build src/Server
dotnet test src/GameTest
