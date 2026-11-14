# Pschool

A parent and student management app for a school. Staff can view parents and their children, add, edit and delete both, and filter students by parent.

Built as two separate projects, as the test requires:

- **Pschool.API**: ASP.NET Core Web API with controllers, EF Core and PostgreSQL
- **Pschool.Client**: Blazor WebAssembly frontend

## Quick start (Docker)

The only requirement is [Docker Desktop](https://www.docker.com/products/docker-desktop/).

```bash
git clone https://github.com/alaricXshiran/Pschool.git
cd Pschool
docker compose up --build
```

Then open:

| What                    | Address                       |
| ----------------------- | ----------------------------- |
| App                     | http://localhost:8080         |
| API test page (Swagger) | http://localhost:5080/swagger |

The database is created automatically and filled with sample parents and students on the first start.

To stop: `Ctrl + C`, or `docker compose down`. To also wipe the data: `docker compose down -v`.

### Settings (optional)

Copy `.env.example` to `.env` to change the database password or the ports. Without it, sensible defaults are used.

## Features

**Parents**

- Paged list with search by name, username or email
- Siblings count per parent
- View details, including the parent's children
- Add, edit and delete, with a confirmation step
- A parent who still has students cannot be deleted

**Students**

- Paged list with search by student or parent name
- Filter by parent from a dropdown
- Add and edit, choosing the parent from a dropdown
- Delete with a confirmation step

**General**

- Validation in the forms and in the API, using the same rules
- Light and dark mode, remembered between visits
- Loading, empty and error messages

## Tech stack

- .NET 10, ASP.NET Core Web API, Blazor WebAssembly
- Entity Framework Core 10 with PostgreSQL 17 (Npgsql)
- Bootstrap 5 and Bootstrap Icons
- Docker and Docker Compose, with nginx serving the frontend

## Project structure

```
Pschool/
├── Pschool.API/        Web API: controllers, EF Core models, migrations, sample data
├── Pschool.Client/     Blazor WebAssembly app: pages, components, API services
├── Pschool.Shared/     Request and response classes (DTOs) used by both projects
├── docker-compose.yml  Database, API and client together
└── .env.example        Optional settings for Docker
```

## API endpoints

| Method | Address                                           | Purpose                                            |
| ------ | ------------------------------------------------- | -------------------------------------------------- |
| GET    | `/api/parents?page=&pageSize=&search=`            | Page of parents                                    |
| GET    | `/api/parents/lookup`                             | All parent names, for dropdowns                    |
| GET    | `/api/parents/{id}`                               | One parent                                         |
| POST   | `/api/parents`                                    | Add a parent                                       |
| PUT    | `/api/parents/{id}`                               | Edit a parent                                      |
| DELETE | `/api/parents/{id}`                               | Delete a parent (refused while they have students) |
| GET    | `/api/students?parentId=&page=&pageSize=&search=` | Page of students, optionally for one parent        |
| GET    | `/api/students/{id}`                              | One student                                        |
| POST   | `/api/students`                                   | Add a student                                      |
| PUT    | `/api/students/{id}`                              | Edit a student                                     |
| DELETE | `/api/students/{id}`                              | Delete a student                                   |

## Design decisions

- **Every student has a parent.** The foreign key is required, and deleting a parent is restricted at the database level, so students can never be left without one. The API answers 409 with a clear message instead of failing.
- **DTOs instead of entities.** The API never exposes its database classes. Requests contain only the fields a client may set, so the Id always comes from the database.
- **A shared project for DTOs.** The API and the Blazor forms use the same classes, so validation rules are written once and checked in both places.
- **Paging and search in the API.** Only one page of rows is ever sent, however large the tables grow.
- **A separate lookup endpoint** returns every parent's Id and name for dropdowns, rather than reusing the paged list.
- **Configurable startup.** Migrations, sample data and Swagger are off by default and switched on per environment, so the Docker setup runs in Production mode.
- **Siblings count is calculated**, not stored, so it can never go out of date.

## Running without Docker (for development)

Requires the .NET 10 SDK and Docker for the database.

```bash
docker compose up -d db
dotnet tool restore
dotnet run --project Pschool.API      # http://localhost:5292/swagger
dotnet run --project Pschool.Client   # http://localhost:5180
```

In Development mode the API applies migrations and adds sample data on startup.

## Possible improvements

- Admin login, so only authorised staff can add, edit and delete
- Automated tests for the API endpoints
