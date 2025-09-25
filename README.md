# Intelectah - Tech Challenge
⚠️ Atenção - Não utilize docker para rodar a aplicação. Apesar dos arquivos existirem, eles não estão atualizados

## Descrição
Aplicação ASP.NET Core MVC (.NET 8.0) para gestão de concessionárias de veículos.  
Implementa autenticação com Identity, CRUD de entidades principais (Fabricantes, Veículos, Concessionárias e Vendas) e controle de acesso baseado em papéis de usuário (Administrador, Gerente e Vendedor).

## 🚀 Pré-requisitos

- [SDK do .NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Microsoft SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) (LocalDB, Express ou Developer)
- (Opcional) [Redis](https://redis.io/download) para cache

## ⚙️ Configuração do Banco de Dados

O projeto usa SQL Server. A string de conexão padrão está definida no `appsettings.Development.json`:

````
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CarDealershipDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;",
    "Redis": "localhost:6379"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
````

### Criar o Banco de Dados
````
dotnet ef database update --project CarDealershipManager.Infrastructure --startup-project CarDealershipManager.App
````

Isso aplicará as migrations e criará o banco CarDealershipDB.

## Executando o Projeto

1. Restaure as dependências:
````
dotnet restore
````

2. Compile o projeto:
````
dotnet build
````

3. Execute a aplicação
````
dotnet run
````

4. Acesse no navegador:
````
[dotnet run](http://localhost:5206)
````

### Usuários Padrões

 👤 Usuários Padrão (Seed)

Durante a primeira execução, usuários e papéis padrão são criados automaticamente:

Administrador
- Email: admin@cardealership.com
- Senha: Admin@123

Gerente
- Email: gerente@cardealership.com
- Senha: Gerente@123

Vendedor
- Email: vendedor@cardealership.com
- Senha: Vendedor@123

#### Papéis e Permissões

Administrador → Gerencia Fabricantes e Concessionárias
Gerente → Gerencia Veículos e Relatórios
Vendedor → Realiza Vendas

## Recursos Implementados

- Autenticação e autorização com ASP.NET Identity
- CRUD de Fabricantes, Veículos, Concessionárias e Vendas
- Controle de acesso baseado em papéis ([Authorize(Roles = ...)])
- Integração inicial com Redis para cache (não finalizada)
- Documentação básica de API com Swagger (/swagger) (⚠️ Nem todos os endpoints estão expostos no Swagger ainda )

### ⚠️ Observações

- O Redis está configurado, mas não é obrigatório para rodar o sistema localmente.
- Caso não tenha Redis ativo, a aplicação funciona normalmente (apenas sem cache distribuído).
- Algumas funcionalidades avançadas (relatórios gráficos, exportação, cache otimizado) ainda estão em desenvolvimento.
