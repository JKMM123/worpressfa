# SME KPI Dashboard

A full-stack dashboard for small and medium e-commerce business owners to track sales, expenses, products, customers, and KPI reports.

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | Vue 3 + Quasar Framework + TypeScript |
| Backend | ASP.NET Core 9 Web API |
| Database | PostgreSQL |
| Auth | JWT access tokens + refresh tokens |
| Package manager | Bun (frontend) |

## Project Structure

```
sme-kpi-dashboard/
├── client/   # Vue 3 + Quasar frontend (port 9000)
└── server/   # ASP.NET Core 9 backend (port 5000)
```

## Prerequisites

- [Bun](https://bun.sh) >= 1.0
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL](https://www.postgresql.org/download/) >= 14

## Setup

### 1. Database

```bash
psql -U postgres -c "CREATE DATABASE sme_kpi_db;"
```

### 2. Backend

Update the connection string in `server/appsettings.Development.json`:

```json
"DefaultConnection": "Host=localhost;Database=sme_kpi_db;Username=postgres;Password=YOUR_PASSWORD"
```

> **Security:** Move the connection string and JWT secret to environment variables before deploying. Never commit real credentials to Git.

```bash
cd server
dotnet ef database update   # applies migrations
dotnet run                  # starts on http://localhost:5000
```

### 3. Frontend

```bash
cd client
bun install
bun run dev   # starts on http://localhost:9000
```

## API Endpoints

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| POST | `/api/auth/register` | None | Create account |
| POST | `/api/auth/login` | None | Sign in |
| POST | `/api/auth/refresh` | None | Rotate tokens |
| POST | `/api/auth/logout` | Bearer | Revoke refresh token |

## Architecture

### Backend (3-layer)

- **Controllers** — HTTP request/response only, delegate to services
- **Services** — Business logic, call repositories
- **Repositories** — Database access via EF Core, no business logic

### Frontend

- **stores/** — Pinia stores (auth state kept in memory, refresh token in localStorage)
- **services/api.ts** — Axios instance with automatic token refresh on 401
- **router/** — Vue Router with auth guards
- **layouts/** — `MainLayout` (sidebar + topbar) and `AuthLayout` (centered card)

## Phase 2 (planned)

Sales tracking, expense management, product catalog, customer records, and KPI charts.
