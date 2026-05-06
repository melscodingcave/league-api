# 🎱 league-api

A RESTful API for managing a billiards league — players, matches, standings, and win/loss records.

Built as a portfolio project to demonstrate C#/.NET backend engineering, REST API design, Entity Framework, PostgreSQL integration, Swagger documentation, and containerized deployment.

---

## 🛠 Tech Stack

- **Language:** C# / .NET 8
- **Framework:** ASP.NET Core Web API
- **ORM:** Entity Framework Core
- **Database:** PostgreSQL
- **Documentation:** Swagger / OpenAPI
- **Containerization:** Docker + Docker Compose
- **Testing:** Covered by [`break-and-verify`](https://github.com/melscodingcave/break-and-verify) — a companion SpecFlow/BDD test suite

---

## 📐 Domain Model

| Entity | Description |
|---|---|
| `Player` | A registered league participant |
| `Match` | A recorded game between two players |
| `Standing` | Computed win/loss/streak record per player |

---

## 🚀 Getting Started

### Run locally with Docker
```bash
docker-compose up --build
```
API will be available at `http://localhost:5000`
Swagger UI at `http://localhost:5000/swagger`

### Run locally without Docker
```bash
dotnet restore
dotnet run --project LeagueApi
```
Update the connection string in `appsettings.Development.json` to point to your local PostgreSQL instance.

---

## 📡 API Endpoints (Planned)

| Method | Route | Description |
|---|---|---|
| GET | `/api/players` | List all players |
| POST | `/api/players` | Register a new player |
| GET | `/api/players/{id}/standing` | Get a player's record |
| POST | `/api/matches` | Record a match result |
| GET | `/api/matches` | List all matches |
| GET | `/api/standings` | Full league leaderboard |

---

## 🤖 AI-Assisted Development

See [`AI-NOTES.md`](./AI-NOTES.md) for a transparent log of how AI tooling was used in this project — what was prompted, what was rejected, and what was changed.

---

## 🔗 Related Projects

This API is intentionally tested by a companion BDD suite:
- **[`break-and-verify`](https://github.com/melscodingcave/break-and-verify)** — SpecFlow + Gherkin test suite targeting this API