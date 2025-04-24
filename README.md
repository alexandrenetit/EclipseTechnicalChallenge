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
### Prerequisites
- Docker
- Docker Compose
### Running with Docker
1. Clone o repositório
2. Execute `docker-compose up --build`
3. A API estará disponível em `http://localhost:7000`
### API Documentation
Após iniciar a aplicação, o Swagger UI estará disponível em `http://localhost:7000/swagger`

```bash
curl -X 'POST' \
  'http://localhost:7000/api/projects' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "name": "Projeto Exemplo 1",
  "description": "Criação de um projeto de exemplo para criação das tarefas",
  "ownerId": "22222222-2222-2222-2222-222222222222"
}'