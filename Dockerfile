# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["AprobadorIdeas.csproj", "./"]
RUN dotnet restore "AprobadorIdeas.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "AprobadorIdeas.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port Render uses (Render usually sets PORT env var, but ASP.NET defaults to 8080 in newer versions)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AprobadorIdeas.dll"]
