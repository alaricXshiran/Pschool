# ---- Build stage: compile the API with the full .NET SDK ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy only the project files first and restore packages.
# Docker caches this step, so packages are downloaded again only when a .csproj changes.
COPY Pschool.Shared/Pschool.Shared.csproj Pschool.Shared/
COPY Pschool.API/Pschool.API.csproj Pschool.API/
RUN dotnet restore Pschool.API/Pschool.API.csproj

# Copy the source code and publish a release build.
COPY Pschool.Shared/ Pschool.Shared/
COPY Pschool.API/ Pschool.API/
RUN dotnet publish Pschool.API/Pschool.API.csproj -c Release -o /app/publish --no-restore

# ---- Run stage: only the compiled app on the small runtime image ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Listen on port 8080 inside the container.
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Run as the image's built-in non-root user.
USER $APP_UID

ENTRYPOINT ["dotnet", "Pschool.API.dll"]