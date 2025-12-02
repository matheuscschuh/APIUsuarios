# API de Gerenciamento de Usuários

  

## Descrição

Este projeto é uma API de Gerenciamento de Usuários desenvolvida com ASP.NET Core usando Minimal APIs. A ideia foi criar uma aplicação simples, rápida e organizada, onde é possível cadastrar, buscar, atualizar 
e remover usuários do sistema. Tudo foi estruturado seguindo princípios de Clean Architecture, para deixar o código mais limpo, separado por camadas e fácil de manter no futuro.

Durante o desenvolvimento, foram utilizados padrões de projeto, validações com FluentValidation e persistência de dados com Entity Framework Core, garantindo que os dados dos usuários sejam armazenados de 
forma segura e que todas as entradas passem por validações antes de serem salvas. O objetivo foi criar uma API que funcione bem, seja estável e preparada para crescer conforme as necessidades da aplicação.

No geral, o projeto entrega uma solução completa para gerenciamento de usuários, seguindo boas práticas de desenvolvimento e trazendo uma estrutura moderna e eficiente para um backend profissional.

  

## Tecnologias Utilizadas

- .NET 8.0

- Entity Framework Core

- SQLite

- FluentValidation

- Outras...

  

## Padrões de Projeto Implementados

- Repository Pattern

- Service Pattern

- DTO Pattern

- Dependency Injection

  

## Como Executar o Projeto
  

  ### Pré-requisitos

 Plataforma e Linguagem
  -  .NET 8.0 ou superior

  -   C# 12.0

  -   ASP.NETLinks para um site externo. Core com Minimal APIs

  Banco de Dados
  -   Entity Framework Core (versão 8.0+)

  -   SQLite (para desenvolvimento e entrega)

  -   Code First com Migrations

  Bibliotecas Externas
  -   FluentValidation.AspNetCore (versão 11.3+)

  -   Outras bibliotecas necessárias para implementação

  

  ### Passos

  1. Clone o repositório

  2. Execute as migrations

  3. Execute a aplicação utilizando dotnet run

  

  

## Estrutura do Projeto

APIUsuarios/
│
├── Application/
│   │
│   ├── DTOs/
│   │   ├── UsuarioCreateDto.cs
│   │   ├── UsuarioReadDto.cs
│   │   └── UsuarioUpdateDto.cs
│   │
│   ├── Interfaces/
│   │   ├── IUsuarioRepository.cs
│   │   └── IUsuarioService.cs
│   │
│   ├── Services/
│   │   └── UsuarioService.cs
│   │
│   └── Validators/
│       ├── UsuarioCreateDtoValidator.cs
│       └── UsuarioUpdateDtoValidator.cs
│
├── Domain/
│   └── Entities/
│       └── Usuario.cs
│
├── Infrastructure/
│   │
│   ├── Persistence/
│   │   └── AppDbContext.cs
│   │
│   └── Repositories/
│       └── UsuarioRepository.cs
│
├── Migrations/
│   ├── 20251202222719_Inicial.cs
│   ├── 20251202222719_Inicial.Designer.cs
│   └── AppDbContextModelSnapshot.cs
│
├── Properties/
│
├── obj/
│
├── bin/
│
├── APIUsuarios.csproj
├── APIUsuarios.http
├── APIUsuarios.sln
├── appsettings.json
├── appsettings.Development.json
└── Program.cs



  

## Autor

Matheus de Carvalho Schuh
