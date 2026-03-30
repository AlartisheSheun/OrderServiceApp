# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file
COPY ["OrderAPI.csproj", "./"]

# Restore dependencies
RUN dotnet restore "OrderAPI.csproj"

# Copy source code
COPY . .

# Build application
RUN dotnet build "OrderAPI.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "OrderAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published application
COPY --from=publish /app/publish .

# Expose port (Render uses 10000)
EXPOSE 10000

# Set environment
ENV ASPNETCORE_URLS=http://+:10000
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
  CMD dotnet --info || exit 1

# Run application
ENTRYPOINT ["dotnet", "OrderAPI.dll"]
