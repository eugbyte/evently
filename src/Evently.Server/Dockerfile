# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
# https://learn.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=windows&pivots=dotnet-9-0#create-the-dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
RUN apt-get update
RUN apt-get install curl --yes
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs
RUN npm install -g pnpm

WORKDIR /App

## The docker build command and the build context is fixed at ./Evently.slnx

COPY . .

RUN ls

# Build and publish a release
RUN dotnet publish ./src/Evently.Server/Evently.Server.csproj -c Release -o publish -p:UseAppHost=false

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS run
WORKDIR /App/server

COPY --from=build /App/publish .

# Sanity check that react files has been transpiled to wwwroot folder
Run ls
RUN ls wwwroot 

EXPOSE 8080
EXPOSE 8081

ARG PORT=8080
RUN echo $PORT

# cannot separate --urls http https://docs.docker.com/reference/dockerfile/#variable-substitution
ENTRYPOINT ["dotnet", "Evently.Server.dll", "--urls https://*:$PORT"]