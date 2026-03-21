
# 🚀 Guia de Configuração - Nucleos Server

---

## 📋 Pré-requisitos

### 1. .NET SDK
O projeto foi desenvolvido com **.NET 10.0**. Verifique sua versão:

```bash
dotnet --version
Se não tiver ou for inferior à 10.0, instale a versão correta no site oficial.

2. Banco de Dados (Supabase)

Acesse Supabase Dashboard
Selecione o projeto Nucleos
Em Project Settings > Database copie a senha
🔧 Configurar User Secrets (Senha do Supabase)

O projeto utiliza User Secrets para armazenar a senha do banco de dados de forma segura (não vai para o Git).

bash
cd src/1.\ Presentation/Nucleos.API

# Inicializar User Secrets (se necessário)
dotnet user-secrets init

# Adicionar a senha do Supabase (SUBSTITUA pela sua senha)
dotnet user-secrets set "Supabase:Password" "SUA_SENHA_AQUI"

# Verificar se foi salvo
dotnet user-secrets list
📦 Restaurar Dependências e Compilar

Volte para a raiz do back-end:

bash
cd ~/nucleos-server/back-end

# Restaurar pacotes NuGet (equivalente ao npm install)
dotnet restore

# Compilar a solução
dotnet build
🏃 Executar a API

bash
cd src/1.\ Presentation/Nucleos.API
dotnet run --urls="http://localhost:5000"
Verifique no terminal se a conexão com o Supabase foi estabelecida.

🌐 Acessar a API

Endpoint	URL
Home	http://localhost:5000/
Health Check	http://localhost:5000/api/health
Swagger	http://localhost:5000/swagger
📁 Estrutura do Projeto

text
back-end/
├── src/
│   ├── 1. Presentation/Nucleos.API/      → Controllers, Middleware, Filters
│   ├── 2. Application/Nucleos.Application/ → CQRS, Commands, Queries
│   ├── 3. Domain/Nucleos.Domain/         → Entities, Enums, ValueObjects
│   └── 4. Infrastructure/Nucleos.Infrastructure/ → Persistence, Identity, Services
├── docs/                                 → Documentação da API e arquitetura
├── docker/                               → Configuração Docker
├── k8s/                                  → Kubernetes manifests
├── scripts/                              → Scripts de deploy e seed
└── tests/                                → Testes unitários e funcionais
📌 Consulte o arquivo estructure.md para detalhes completos da estrutura.

📝 Comandos Úteis

Comando	Descrição
dotnet restore	Restaura pacotes NuGet
dotnet build	Compila a solução
dotnet run	Roda a API
dotnet user-secrets list	Lista secrets configuradas
dotnet user-secrets remove "Supabase:Password"	Remove uma secret
dotnet user-secrets clear	Remove todas as secrets
🌿 Git Flow - Como Trabalhar

1. Clonar e Configurar

bash
# Clonar o repositório
git clone https://github.com/seu-usuario/nucleos-server.git
cd nucleos-server

# Ir para a branch develop
git checkout develop

# Garantir que está atualizada
git pull origin develop
2. Criar uma Feature

bash
# Certifique-se que está na develop
git checkout develop
git pull origin develop

# Criar branch da feature
git checkout -b feature/nome-da-feature

# Exemplos:
git checkout -b feature/auth-login
git checkout -b feature/crud-nucleos
git checkout -b feature/gestao-blocos
3. Desenvolver e Commitar

bash
# Verificar arquivos modificados
git status

# Adicionar arquivos específicos
git add src/1.\ Presentation/Nucleos.API/Controllers/v1/AuthController.cs
git add src/2.\ Application/Nucleos.Application/Features/Auth/Commands/LoginCommand.cs

# Ou adicionar tudo
git add .

# Commitar com mensagem descritiva
git commit -m "feat: implementa endpoint de login"
Padrões de commit:

Tipo	Descrição
feat	Nova funcionalidade
fix	Correção de bug
docs	Documentação
refactor	Refatoração
test	Testes
chore	Tarefas de manutenção
4. Enviar para o GitHub

bash
# Enviar branch para o GitHub
git push origin feature/nome-da-feature

# Exemplo:
git push origin feature/auth-login
5. Após Pull Request Aprovado

bash
# Mudar para develop
git checkout develop

# Atualizar com as mudanças do PR
git pull origin develop
⚠️ Lembretes Importantes

NUNCA commitar diretamente na main ou develop
SEMPRE criar feature/* para novas funcionalidades
SEMPRE abrir Pull Request para develop
SEMPRE revisar código antes do merge
SEMPRE manter a develop atualizada