# Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
ENV ASPNETCORE_URLS=http://+:80
WORKDIR /SolarWatch

# Copy csproj and restore as distinct layers
COPY SolarWatch/SolarWatch.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /SolarWatch
COPY --from=build-env /SolarWatch/out .

# Add EF Core Tools
RUN dotnet tool install --global dotnet-ef

# Database Migrations (Apply migrations during container startup)
# Create a shell script to run migrations and the application
RUN echo "#!/bin/sh" > entrypoint.sh
RUN echo "dotnet ef database update --context SolarWatchContext" >> entrypoint.sh
RUN chmod +x entrypoint.sh

# Use the shell script as the CMD
CMD ["./entrypoint.sh"]


ENTRYPOINT ["dotnet", "SolarWatch.dll"]