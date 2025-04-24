# Task Management System API

Uma API RESTful para gerenciar tarefas e projetos construída com .NET 8, seguindo os princípios de Clean Architecture e Domain-Driven Design.

## Features

- Gerenciamento de projetos (criar, visualizar, excluir)
- Gerenciamento de work items (criar, atualizar, excluir, adicionar comentários)
- Relatórios de performance
- History tracking para todas as mudanças

## Technologies

- .NET 8
- MySQL
- Entity Framework Core
- Docker

## Getting Started

### Pré-requisitos

- Docker
- Docker Compose

### Rodando com o Docker

1. Clone o repositório
2. Execute `docker-compose up --build`
3. A API estará disponível em `http://localhost:7000`

### Documentação da API 

Após iniciar a aplicação, o Swagger UI estará disponível em `http://localhost:7000/swagger`