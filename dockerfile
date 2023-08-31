# Use the official .NET Core SDK image as the base image for building
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory in the container
WORKDIR /src

# Copy the application project files for restoring dependencies
COPY ["Notion.Application/Notion.Application.csproj", "Notion.Application/"]
COPY ["Notion.Infrastructure/Notion.Infrastructure.csproj", "Notion.Infrastructure/"]
COPY ["Notion.Domain/Notion.Domain.csproj", "Notion.Domain/"]
COPY ["Notion.WebApi/Notion.WebApi.csproj", "Notion.WebApi/"]
COPY ["Notion.MigrationTool", "Notion.MigrationTool/"]
COPY ["Notion.ExceptionHandler/Notion.ExceptionHandler.csproj", "Notion.ExceptionHandler/"]

# Restore dependencies
RUN dotnet restore "Notion.WebApi/Notion.WebApi.csproj"

# Copy the rest of the application code into the container
COPY . .

# Build the application
WORKDIR "/src/Notion.WebApi"
RUN dotnet build "Notion.WebApi.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Notion.WebApi.csproj" -c Release -o /app/publish

# Use a lightweight runtime image as the final base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app

# Copy the published files from the previous stage into the final stage
COPY --from=publish /app/publish .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Notion.WebApi.dll"]
