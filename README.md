# Task Management System API

A RESTful API for managing tasks and projects built with .NET 8, following Clean Architecture and Domain-Driven Design principles.

## Features

- Project management (create, view, delete)
- Work item management (create, update, delete, add comments)
- Performance reporting
- History tracking for all changes

## Technologies

- .NET 8
- PostgreSQL
- Entity Framework Core
- Docker

## Getting Started

### Prerequisites

- Docker
- Docker Compose

### Running with Docker

1. Clone the repository
2. Run `docker-compose up --build`
3. The API will be available at `http://localhost:8080`

### API Documentation

After starting the application, Swagger UI will be available at `http://localhost:8080/swagger