# LogisticIntegration - Docker & Database Setup

## Prerequisites

- Docker Desktop installed and running
- .NET 8.0 SDK or later
- SQL Server Management Studio (optional, for GUI access)

## Environment Configuration

### 1. Create `.env` file from template

```bash
cp .env.example .env
```

Edit `.env` to customize your SQL Server password:

```env
SA_PASSWORD=YourSecurePassword@2026!
```

> ⚠️ **Important**: The `.env` file is excluded from Git. Never commit passwords to version control.

## Running with Docker Compose

### Start SQL Server Container

```bash
docker compose up -d
```

This will:
- Pull the SQL Server 2022 image from Microsoft Container Registry
- Create a container named `logistic_sql_server`
- Expose SQL Server on port `1433`
- Create a named volume `sqlvolume` for data persistence
- Run health checks every 10 seconds

### Verify SQL Server is Running

```bash
docker compose ps
```

Expected output:
```
NAME                  STATUS
logistic_sql_server   Up X seconds (healthy)
```

### Connect to SQL Server

**Connection String:**
```
Server=localhost,1433;Database=LogisticIntegrationDb;User Id=sa;Password=SysAdmin@2026!;TrustServerCertificate=True;
```

**Using SSMS (SQL Server Management Studio):**
- Server Name: `localhost,1433`
- Authentication: SQL Server Authentication
- Login: `sa`
- Password: Value from `.env` file

**Using sqlcmd (CLI):**
```bash
sqlcmd -S localhost,1433 -U sa -P SysAdmin@2026!
```

## Running the Application

### 1. Build the Solution

```bash
dotnet build
```

### 2. Apply Entity Framework Migrations

```bash
cd LogisticIntegration.Api
dotnet ef database update
```

> Note: Ensure the database is created by EF Core using migrations.

### 3. Run the API

```bash
dotnet run
```

The API will start on:
- HTTP: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger`

## Docker Compose Configuration Details

### Services

**sqlserver**
- Image: `mcr.microsoft.com/mssql/server:2022-latest`
- Container: `logistic_sql_server`
- Port: `1433:1433`
- Volume: `sqlvolume:/var/opt/mssql` (persistent data storage)
- Restart Policy: `unless-stopped`
- Health Check: Enabled with 10-second interval

### Environment Variables

| Variable | Value | Description |
|----------|-------|---|
| `ACCEPT_EULA` | Y | Accept SQL Server EULA |
| `SA_PASSWORD` | (from .env) | System Administrator password |
| `MSSQL_PID` | Developer | Use Developer Edition |
| `MSSQL_TCP_PORT` | 1433 | SQL Server listening port |

### Networks

- **logistic_network**: Bridge network for all services
  - Allows services to communicate by container name

### Volumes

- **sqlvolume**: Local named volume for SQL Server data persistence
  - Data survives container restarts
  - Located at `/var/opt/mssql` inside container

## Stopping and Cleanup

### Stop Containers

```bash
docker compose down
```

### Remove Volumes (Deletes Data!)

```bash
docker compose down -v
```

### View Logs

```bash
docker compose logs -f sqlserver
```

## Troubleshooting

### Container Won't Start

```bash
docker compose logs sqlserver
```

Look for errors related to:
- Insufficient disk space
- Port 1433 already in use
- Invalid SA_PASSWORD format

### Health Check Failing

Wait 30-40 seconds after container starts. SQL Server takes time to initialize.

```bash
docker compose ps
docker compose logs sqlserver
```

### Connection Refused

1. Verify container is running: `docker compose ps`
2. Verify port is exposed: `netstat -an | findstr 1433` (Windows)
3. Check firewall settings
4. Ensure correct password in connection string

### Database Connection Errors in Application

Verify:
- Connection string in `appsettings.json` matches container configuration
- Container is healthy: `docker compose ps`
- SA password matches between `.env` and `appsettings.json`

## Architecture

The solution follows Clean Architecture with layers:

- **Domain**: Pure business entities (no dependencies)
- **Application**: CQRS commands/queries with MediatR
- **Infrastructure**: EF Core, Database context, Repositories
- **Api**: REST controllers with dependency injection

All layers integrate seamlessly with the SQL Server database running in Docker.
