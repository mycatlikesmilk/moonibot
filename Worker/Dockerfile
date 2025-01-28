# Stage 1: building and publish
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./Application ./Application
COPY ./Domain ./Domain
COPY ./Infrastructure ./Infrastructure
COPY ./Worker ./Worker

ENV ASPNETCORE_ENVIRONMENT=Production
ENV TZ=Europe/Moscow

RUN dotnet restore ./Worker/Worker.csproj
RUN dotnet publish ./Worker/Worker.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Worker.dll"]