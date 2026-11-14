# ---- Build stage: publish the Blazor app with the .NET SDK ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore packages first so this step is cached.
COPY Pschool.Shared/Pschool.Shared.csproj Pschool.Shared/
COPY Pschool.Client/Pschool.Client.csproj Pschool.Client/
RUN dotnet restore Pschool.Client/Pschool.Client.csproj

COPY Pschool.Shared/ Pschool.Shared/
COPY Pschool.Client/ Pschool.Client/
RUN dotnet publish Pschool.Client/Pschool.Client.csproj -c Release -o /app/publish --no-restore

# ---- Run stage: serve the published files with nginx ----
FROM nginx:1.27-alpine

COPY Pschool.Client/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

# Startup script that writes the API address; strip Windows line endings so it runs on Linux.
COPY Pschool.Client/write-config.sh /docker-entrypoint.d/40-write-config.sh
RUN sed -i 's/\r$//' /docker-entrypoint.d/40-write-config.sh \
    && chmod +x /docker-entrypoint.d/40-write-config.sh

# Default API address; docker compose can override it.
ENV API_BASE_URL=http://localhost:5080/

EXPOSE 80