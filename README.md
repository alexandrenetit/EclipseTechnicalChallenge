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
1. Clone o repositório e entre no diretório criado
2. Execute `docker-compose up -d --build` a partir do diretório ./TaskManagement
3. A API estará disponível em `http://localhost:7000`
### API Documentation
Após iniciar a aplicação, o Swagger UI estará disponível em `http://localhost:7000/swagger`

### Exemplo de chamada para criar um projeto

Após a criação do projeto poderá criar as tarefas.
Obs: não esqueça de pegar o Id do projeto retornado após a chamada do método POST abaixo.  

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
```

### CRUD das tarefas

Para criar/atualizar/adiconar comentários e etc das tarefas utilize os endpoints expostos no Swagger preenchendo corretamente as informações dos parâmetros nas urls e nos body's.


## Fase 2: Refinamento

Como parte do processo de evolução do sistema de gerenciamento de tarefas, considero alinhar com o PO as próximas implementações com as reais necessidades dos usuários. Baseado na análise do escopo atual e nas tendências de mercado para ferramentas de produtividade, gostaria de propor as seguintes questões estratégicas para refinamento do produto:

### Implementação de notificações: 
Implementar um sistema de notificações para alertar usuários sobre tarefas próximas do vencimento ou alterações em tarefas compartilhadas. Quais canais seriam prioritários (email, push, in-app)?

### Hierarquia de tarefas: 
Verificar se existe a necessidade de permitir que tarefas possam ter subtarefas, criando uma estrutura hierárquica que permita melhor organização de projetos complexos.

### Integrações com ferramentas externas: 
Verificar quais ferramentas externas (Slack, Microsoft Teams, GitHub, etc.) seriam prioritárias para integração, possibilitando um fluxo de trabalho mais conectado.

### Personalização de fluxos de trabalho: 
Investigar a possibilidade de permitir que equipes possam personalizar seus próprios fluxos de trabalho com estados personalizados além dos padrões (pendente, em andamento, concluída).

## Fase 3: Final
Após a implementação da API de gerenciamento de tarefas, identifiquei pontos importantes para evolução do projeto que contribuiriam significativamente para sua robustez, escalabilidade e experiência do usuário. Apresento a seguir as principais oportunidades de melhoria técnica que poderiam ser consideradas em iterações futuras:

### Oportunidades de Melhoria
### Arquitetura e Infraestrutura

#### Implementação de arquitetura de microsserviços: 
Refatorar a aplicação monolítica em microsserviços independentes (gerenciamento de projetos, tarefas, relatórios) para facilitar a manutenção e permitir escalabilidade individualizada de componentes conforme a demanda.

#### Migração para Azure Kubernetes Service (AKS): 
Implementar a solução no AKS para orquestração de containers, aproveitando recursos como autoscaling, integração nativa com Azure DevOps para CI/CD e políticas de segurança avançadas com Azure Policy para Kubernetes.

#### Implementação de cache distribuído: 
Utilizar Azure Cache for Redis para armazenar dados frequentemente acessados, reduzindo a carga no banco de dados principal e melhorando o tempo de resposta da API.

#### Padrões de design e código: 
Aplicar o padrão CQRS (Command Query Responsibility Segregation) para separar operações de leitura e escrita, otimizando o desempenho e facilitando a manutenção.

#### Monitoramento e observabilidade: 
Integrar Azure Application Insights e Azure Monitor para telemetria completa, monitoramento de performance e comportamento da aplicação em produção.

#### Automação de testes: 
Expandir a suíte de testes para incluir testes de integração e end-to-end automatizados, utilizando ferramentas como Cypress ou Postman/Newman com Azure DevOps Pipelines para CI/CD.

