FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["dockertest.csproj", ""]
RUN dotnet restore "./dockertest.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "dockertest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dockertest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dockertest.dll"]

#docker build -t my/api:dev .
#docker run -d -p 8001:80 --net mybridge --name api my/api:dev