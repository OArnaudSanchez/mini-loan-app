FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Fundo.Applications.Domain/Fundo.Applications.Domain.csproj Fundo.Applications.Domain/
COPY Fundo.Applications.Application/Fundo.Applications.Application.csproj Fundo.Applications.Application/
COPY Fundo.Applications.Infrastructure/Fundo.Applications.Infrastructure.csproj Fundo.Applications.Infrastructure/
COPY Fundo.Applications.WebApi/Fundo.Applications.WebApi.csproj Fundo.Applications.WebApi/

RUN dotnet restore Fundo.Applications.WebApi/Fundo.Applications.WebApi.csproj

COPY . .

WORKDIR /src/Fundo.Applications.WebApi
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app

RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Fundo.Applications.WebApi.dll"]