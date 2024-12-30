FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5076

ENV ASPNETCORE_URLS=http://+:5076

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["ProductMicroservice.API/ProductMicroservice.API.csproj", "ProductMicroservice.API/"]
RUN dotnet restore "ProductMicroservice.API/ProductMicroservice.API.csproj"
COPY . .
WORKDIR "/src/ProductMicroservice.API"
RUN dotnet build "ProductMicroservice.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "ProductMicroservice.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductMicroservice.API.dll"]
