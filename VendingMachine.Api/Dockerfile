#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["VendingMachine.Api/VendingMachine.Api.csproj", "VendingMachine.Api/"]
COPY ["VendingMachine.Core\VendingMachine.Core.csproj", "VendingMachine.Core/"]
COPY ["VendingMachine.Entities/VendingMachine.Entities.csproj", "VendingMachine.Entities/"]
COPY ["VendingMachine.Infrastructure/VendingMachine.Infrastructure.csproj", "VendingMachine.Infrastructure/"]
COPY ["VendingMachine.Infrastructure.Core/VendingMachine.Infrastructure.Core.csproj", "VendingMachine.Infrastructure.Core/"]

RUN dotnet restore "VendingMachine.Api/VendingMachine.Api.csproj"

COPY . .
WORKDIR "/src/VendingMachine.Api"
RUN dotnet build "VendingMachine.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VendingMachine.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY ./wait-for-it.sh ./wait-for-it.sh
RUN chmod +x ./wait-for-it.sh

ENTRYPOINT ["dotnet", "VendingMachine.Api.dll"]