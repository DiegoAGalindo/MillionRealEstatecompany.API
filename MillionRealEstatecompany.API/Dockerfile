# Use the official .NET 8.0 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["MillionRealEstatecompany.API.csproj", "."]
RUN dotnet restore "MillionRealEstatecompany.API.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src"

# Build the application
RUN dotnet build "MillionRealEstatecompany.API.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "MillionRealEstatecompany.API.csproj" -c Release -o /app/publish

# Use the official .NET 8.0 ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 80
EXPOSE 80

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Docker

# Set the entry point
ENTRYPOINT ["dotnet", "MillionRealEstatecompany.API.dll"]