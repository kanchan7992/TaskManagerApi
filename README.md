# 🚀 Task Manager API

Production-ready ASP.NET Core Web API for managing projects and tasks with JWT authentication.

---

## 🔐 Features

- User Registration & Login (JWT Authentication)
- Role-based secure APIs (Authorize)
- Projects CRUD (user-specific data)
- Tasks CRUD (linked to projects)
- Pagination & Filtering
- Input Validation (Data Annotations)
- Global Exception Handling Middleware
- Swagger API Documentation

---

## 🛠 Tech Stack

- C# / .NET 8 / ASP.NET Core Web API
- Entity Framework Core (SQLite)
- JWT Authentication
- Swagger (OpenAPI)
- LINQ

---

## 📁 API Endpoints

### Auth
- POST /api/auth/register
- POST /api/auth/login

### Projects
- GET /api/projects?page=1&pageSize=5
- POST /api/projects
- PUT /api/projects/{id}
- DELETE /api/projects/{id}

### Tasks
- GET /api/tasks?projectId=1&page=1&pageSize=5&status=Todo
- POST /api/tasks
- PUT /api/tasks/{id}
- DELETE /api/tasks/{id}

---

## 🔐 Authentication

Use JWT token in header:

Authorization: Bearer {token}

---

## ▶️ How to Run

```bash
dotnet restore
dotnet run
