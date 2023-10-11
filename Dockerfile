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
#RUN dotnet tool install --global dotnet-ef

ENTRYPOINT ["dotnet", "SolarWatch.dll"]