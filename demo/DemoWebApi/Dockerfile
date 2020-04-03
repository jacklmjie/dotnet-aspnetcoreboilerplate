FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DemoWebApi.csproj", ""]
RUN dotnet restore "./DemoWebApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "DemoWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoWebApi.dll"]

#docker build -t webapi .
#docker run -d -p 8001:80 --name webapi01 webapi