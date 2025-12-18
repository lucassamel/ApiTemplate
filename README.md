# Gerenciamento de Estoque

## 🚀 Pré-requisitos

Antes de rodar a aplicação você precisará ter:

- [.NET 9 SDK](https://dotnet.microsoft.com/)

ApiTemplate é um **um projeto API em .NET Core** com arquitetura limpa (Clean Architecture) para gerenciar estoque:

- 🧱 Domínio separado (Domain)
- 🎯 Application (casos de uso / DTOs / handlers)
- 🏗️ Infraestrutura com Entity Framework Core
- 🐳 Docker & Docker Compose
- 📦 Migrations com EF Core
---

## Fluxograma do Projeto

<img width="551" height="341" alt="image" src="https://github.com/user-attachments/assets/070e7125-acce-4a04-90cf-5c2edca87ec1" />

## 🛠️ Executando localmente

Clone o repositório:

```bash
git clone https://github.com/lucassamel/ApiTemplate.git
cd ApiTemplate
````

```
docker compose up --build
```
[localhost:8081](https://localhost:8081/swagger/index.html)
> Certifique-se de ter o [Docker](https://docs.docker.com/engine/install/) instalado e em execução em sua máquina.
