#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Core.API/Core.API.csproj", "src/Core.API/"]
COPY ["src/Core.Models/Core.Models.csproj", "src/Core.Models/"]
COPY ["src/Core.Common/Core.Common.csproj", "src/Core.Common/"]
COPY ["src/Core.IContract/Core.IContract.csproj", "src/Core.IContract/"]
COPY ["src/Core.Repository/Core.Repository.csproj", "src/Core.Repository/"]
COPY ["src/Core.IRepository/Core.IRepository.csproj", "src/Core.IRepository/"]
COPY ["src/Core.Contract/Core.Contract.csproj", "src/Core.Contract/"]
COPY ["src/Core.AutoMapper/Core.Mapper.csproj", "src/Core.AutoMapper/"]
RUN dotnet restore "src/Core.API/Core.API.csproj"
COPY . .
WORKDIR "/src/src/Core.API"
RUN dotnet build "Core.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Core.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Core.API.dll"]

#docker build -t k8swebapi .
#docker run -d -p 8001:80 --net mybridge --name k8swebapi k8swebapi