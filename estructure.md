# 📁 Estrutura do Projeto - Nucleos Server

nucleos-server
.
├── back-end
│ ├── Nucleos.sln
│ ├── docker
│ │ ├── Dockerfile
│ │ └── docker-compose.yml
│ ├── docs
│ │ ├── api
│ │ │ └── swagger.json
│ │ ├── architecture
│ │ │ └── decisions.md
│ │ └── database
│ │ └── diagram.png
│ ├── k8s
│ │ ├── configmap.yaml
│ │ ├── deployment.yaml
│ │ ├── secrets.yaml
│ │ └── service.yaml
│ ├── scripts
│ │ ├── backup.sh
│ │ ├── deploy.sh
│ │ └── seed-data.sql
│ ├── src
│ │ ├── 1. Presentation
│ │ │ ├── Nucleos.API
│ │ │ │ ├── Controllers
│ │ │ │ │ ├── HealthController.cs
│ │ │ │ │ ├── HomeController.cs
│ │ │ │ │ └── v1
│ │ │ │ │ ├── AdminController.cs
│ │ │ │ │ ├── AuthController.cs
│ │ │ │ │ ├── BlocosController.cs
│ │ │ │ │ ├── CalculosController.cs
│ │ │ │ │ ├── CalendarioController.cs
│ │ │ │ │ ├── GamificacaoController.cs
│ │ │ │ │ ├── HabitosController.cs
│ │ │ │ │ ├── InsightsController.cs
│ │ │ │ │ ├── ItensListaController.cs
│ │ │ │ │ ├── ListasController.cs
│ │ │ │ │ ├── NucleosController.cs
│ │ │ │ │ ├── PlansController.cs
│ │ │ │ │ ├── TarefasController.cs
│ │ │ │ │ ├── TimersController.cs
│ │ │ │ │ └── VersionController.cs
│ │ │ │ ├── Extensions
│ │ │ │ │ └── ServiceExtensions.cs
│ │ │ │ ├── Filters
│ │ │ │ │ ├── ApiKeyAuthFilter.cs
│ │ │ │ │ ├── PermissionFilter.cs
│ │ │ │ │ └── ValidationFilter.cs
│ │ │ │ ├── Middleware
│ │ │ │ │ ├── ExceptionMiddleware.cs
│ │ │ │ │ ├── JwtMiddleware.cs
│ │ │ │ │ ├── RateLimitingMiddleware.cs
│ │ │ │ │ ├── RequestLoggingMiddleware.cs
│ │ │ │ │ └── TenantMiddleware.cs
│ │ │ │ ├── Nucleos.API.csproj
│ │ │ │ ├── Program.cs
│ │ │ │ ├── Startup.cs
│ │ │ │ └── appsettings.json
│ │ │ └── Nucleos.Grpc
│ │ │ ├── Protos
│ │ │ │ └── nucleos.proto
│ │ │ └── Services
│ │ ├── 2. Application
│ │ │ ├── Nucleos.Application
│ │ │ │ ├── Common
│ │ │ │ │ ├── Behaviours
│ │ │ │ │ │ ├── AuthorizationBehaviour.cs
│ │ │ │ │ │ ├── LoggingBehaviour.cs
│ │ │ │ │ │ ├── PerformanceBehaviour.cs
│ │ │ │ │ │ ├── TransactionBehaviour.cs
│ │ │ │ │ │ └── ValidationBehaviour.cs
│ │ │ │ │ ├── Exceptions
│ │ │ │ │ │ ├── BusinessRuleException.cs
│ │ │ │ │ │ ├── ForbiddenException.cs
│ │ │ │ │ │ ├── NotFoundException.cs
│ │ │ │ │ │ ├── UnauthorizedException.cs
│ │ │ │ │ │ └── ValidationException.cs
│ │ │ │ │ ├── Interfaces
│ │ │ │ │ │ ├── IApplicationDbContext.cs
│ │ │ │ │ │ ├── ICacheService.cs
│ │ │ │ │ │ ├── ICurrentUserService.cs
│ │ │ │ │ │ ├── IDateTime.cs
│ │ │ │ │ │ ├── IEmailService.cs
│ │ │ │ │ │ ├── IFileStorageService.cs
│ │ │ │ │ │ ├── INotificationService.cs
│ │ │ │ │ │ ├── IQueueService.cs
│ │ │ │ │ │ └── IUnitOfWork.cs
│ │ │ │ │ ├── Mappings
│ │ │ │ │ │ ├── IMapFrom.cs
│ │ │ │ │ │ └── MappingProfile.cs
│ │ │ │ │ └── Resources
│ │ │ │ │ ├── SharedResource.resx
│ │ │ │ │ └── ValidationMessages.resx
│ │ │ │ ├── DependencyInjection.cs
│ │ │ │ ├── Features
│ │ │ │ │ ├── Admin
│ │ │ │ │ │ ├── Commands
│ │ │ │ │ │ │ ├── CreatePlanCommand.cs
│ │ │ │ │ │ │ └── ManageSubscriptionCommand.cs
│ │ │ │ │ │ ├── DTOs
│ │ │ │ │ │ │ ├── AdminUserDto.cs
│ │ │ │ │ │ │ └── DashboardStatsDto.cs
│ │ │ │ │ │ ├── Queries
│ │ │ │ │ │ │ ├── GetDashboardStatsQuery.cs
│ │ │ │ │ │ │ └── GetUsersQuery.cs
│ │ │ │ │ │ └── Validators
│ │ │ │ │ │ ├── CreatePlanValidator.cs
│ │ │ │ │ │ └── ManageSubscriptionValidator.cs
│ │ │ │ │ ├── Auth
│ │ │ │ │ │ ├── Commands
│ │ │ │ │ │ │ ├── ChangePasswordCommand.cs
│ │ │ │ │ │ │ ├── ForgotPasswordCommand.cs
│ │ │ │ │ │ │ ├── LoginCommand.cs
│ │ │ │ │ │ │ ├── RefreshTokenCommand.cs
│ │ │ │ │ │ │ ├── RegisterCommand.cs
│ │ │ │ │ │ │ ├── ResetPasswordCommand.cs
│ │ │ │ │ │ │ └── VerifyEmailCommand.cs
│ │ │ │ │ │ ├── DTOs
│ │ │ │ │ │ │ ├── AuthResponseDto.cs
│ │ │ │ │ │ │ ├── TokenDto.cs
│ │ │ │ │ │ │ └── UserDto.cs
│ │ │ │ │ │ ├── Queries
│ │ │ │ │ │ │ └── GetCurrentUserQuery.cs
│ │ │ │ │ │ └── Validators
│ │ │ │ │ │ └── LoginValidator.cs
│ │ │ │ │ ├── Blocos
│ │ │ │ │ │ ├── Commands
│ │ │ │ │ │ │ ├── CreateBlocoCommand.cs
│ │ │ │ │ │ │ ├── DeleteBlocoCommand.cs
│ │ │ │ │ │ │ ├── ReorderBlocosCommand.cs
│ │ │ │ │ │ │ └── UpdateBlocoCommand.cs
│ │ │ │ │ │ ├── DTOs
│ │ │ │ │ │ │ ├── BlocoDto.cs
│ │ │ │ │ │ │ └── BlocoListDto.cs
│ │ │ │ │ │ ├── Queries
│ │ │ │ │ │ │ ├── GetBlocoByIdQuery.cs
│ │ │ │ │ │ │ └── GetBlocosByNucleoQuery.cs
│ │ │ │ │ │ └── Validators
│ │ │ │ │ │ └── CreateBlocoValidator.cs
│ │ │ │ │ ├── Calculos
│ │ │ │ │ │ ├── Commands
│ │ │ │ │ │ │ ├── ConfigurarCalculoCommand.cs
│ │ │ │ │ │ │ └── ExecutarCalculoCommand.cs
│ │ │ │ │ │ ├── DTOs
│ │ │ │ │ │ │ ├── CalculoConfigDto.cs
│ │ │ │ │ │ │ └── CalculoResultadoDto.cs
│ │ │ │ │ │ ├── Queries
│ │ │ │ │ │ │ ├── GetCalculoConfigQuery.cs
│ │ │ │ │ │ │ └── GetCalculoResultadoQuery.cs
│ │ │ │ │ │ ├── Services
│ │ │ │ │ │ │ ├── CalculoEngine.cs
│ │ │ │ │ │ │ └── ICalculoEngine.cs
│ │ │ │ │ │ └── Validators
│ │ │ │ │ │ ├── ConfigurarCalculoValidator.cs
│ │ │ │ │ │ └── ExecutarCalculoValidator.cs
│ │ │ │ │ ├── Gamificacao
│ │ │ │ │ │ ├── Commands
│ │ │ │ │ │ │ ├── AdicionarXPCommand.cs
│ │ │ │ │ │ │ ├── AtualizarStreakCommand.cs
│ │ │ │ │ │ │ └── DesbloquearConquistaCommand.cs
│ │ │ │ │ │ ├── DTOs
│ │ │ │ │ │ │ ├── ConquistaDto.cs
│ │ │ │ │ │ │ ├── LevelDto.cs
│ │ │ │ │ │ │ └── StreakDto.cs
│ │ │ │ │ │ ├── Queries
│ │ │ │ │ │ │ ├── GetUserConquistasQuery.cs
│ │ │ │ │ │ │ ├── GetUserLevelQuery.cs
│ │ │ │ │ │ │ └── GetUserStreaksQuery.cs
│ │ │ │ │ │ ├── Services
│ │ │ │ │ │ │ ├── GamificationEngine.cs
│ │ │ │ │ │ │ └── IGamificationEngine.cs
│ │ │ │ │ │ └── Validators
│ │ │ │ │ │ ├── AdicionarXPValidator.cs
│ │ │ │ │ │ └── AtualizarStreakValidator.cs
│ │ │ │ │ ├── Habitos
│ │ │ │ │ │ ├── Commands
│ │ │ │ │ │ │ ├── CreateHabitoCommand.cs
│ │ │ │ │ │ │ ├── DeleteHabitoCommand.cs
│ │ │ │ │ │ │ ├── RegistrarHabitoCommand.cs
│ │ │ │ │ │ │ └── UpdateHabitoCommand.cs
│ │ │ │ │ │ ├── DTOs
│ │ │ │ │ │ │ ├── HabitoDto.cs
│ │ │ │ │ │ │ └── HabitoProgressoDto.cs
│ │ │ │ │ │ ├── Queries
│ │ │ │ │ │ │ ├── GetHabitoProgressoQuery.cs
│ │ │ │ │ │ │ └── GetHabitosByBlocoQuery.cs
│ │ │ │ │ │ └── Validators
│ │ │ │ │ │ └── CreateHabitoValidator.cs
│ │ │ │ │ ├── IA
│ │ │ │ │ │ ├── Commands
│ │ │ │ │ │ │ ├── AplicarInsightCommand.cs
│ │ │ │ │ │ │ ├── EnviarMensagemCommand.cs
│ │ │ │ │ │ │ └── GerarInsightCommand.cs
│ │ │ │ │ │ ├── DTOs
│ │ │ │ │ │ │ ├── InsightDto.cs
│ │ │ │ │ │ │ └── MensagemDto.cs
│ │ │ │ │ │ ├── Queries
│ │ │ │ │ │ │ ├── GetContextoQuery.cs
│ │ │ │ │ │ │ └── GetInsightsQuery.cs
│ │ │ │ │ │ ├── Services
│ │ │ │ │ │ │ ├── AIService.cs
│ │ │ │ │ │ │ └── IAIService.cs
│ │ │ │ │ │ └── Validators
│ │ │ │ │ │ ├── EnviarMensagemValidator.cs
│ │ │ │ │ │ └── GerarInsightValidator.cs
│ │ │ │ │ ├── ItensLista
│ │ │ │ │ │ ├── Commands
│ │ │ │ │ │ │ ├── BulkUpdateItemsCommand.cs
│ │ │ │ │ │ │ ├── CreateItemCommand.cs
│ │ │ │ │ │ │ ├── DeleteItemCommand.cs
│ │ │ │ │ │ │ ├── ToggleItemCheckedCommand.cs
│ │ │ │ │ │ │ └── UpdateItemCommand.cs
│ │ │ │ │ │ ├── DTOs
│ │ │ │ │ │ │ ├── ItemListaDto.cs
│ │ │ │ │ │ │ └── ItemTotalDto.cs
│ │ │ │ │ │ ├── Queries
│ │ │ │ │ │ │ ├── GetItemByIdQuery.cs
│ │ │ │ │ │ │ └── GetItensByListaQuery.cs
│ │ │ │ │ │ └── Validators
│ │ │ │ │ │ └── CreateItemValidator.cs
│ │ │ │ │ ├── Listas
│ │ │ │ │ │ ├── Commands
│ │ │ │ │ │ │ ├── CreateListaCommand.cs
│ │ │ │ │ │ │ ├── DeleteListaCommand.cs
│ │ │ │ │ │ │ └── UpdateListaCommand.cs
│ │ │ │ │ │ ├── DTOs
│ │ │ │ │ │ │ ├── ListaDto.cs
│ │ │ │ │ │ │ └── ListaTotalDto.cs
│ │ │ │ │ │ ├── Queries
│ │ │ │ │ │ │ ├── GetListaByIdQuery.cs
│ │ │ │ │ │ │ ├── GetListaTotaisQuery.cs
│ │ │ │ │ │ │ └── GetListasByBlocoQuery.cs
│ │ │ │ │ │ └── Validators
│ │ │ │ │ │ └── CreateListaValidator.cs
│ │ │ │ │ ├── Nucleos
│ │ │ │ │ │ ├── Commands
│ │ │ │ │ │ │ ├── CreateNucleoCommand.cs
│ │ │ │ │ │ │ ├── DeleteNucleoCommand.cs
│ │ │ │ │ │ │ ├── ShareNucleoCommand.cs
│ │ │ │ │ │ │ └── UpdateNucleoCommand.cs
│ │ │ │ │ │ ├── DTOs
│ │ │ │ │ │ │ ├── NucleoDto.cs
│ │ │ │ │ │ │ ├── NucleoListDto.cs
│ │ │ │ │ │ │ └── NucleoStatsDto.cs
│ │ │ │ │ │ ├── Queries
│ │ │ │ │ │ │ ├── GetNucleoByIdQuery.cs
│ │ │ │ │ │ │ ├── GetNucleoStatsQuery.cs
│ │ │ │ │ │ │ └── GetNucleosQuery.cs
│ │ │ │ │ │ └── Validators
│ │ │ │ │ │ └── CreateNucleoValidator.cs
│ │ │ │ │ └── Tarefas
│ │ │ │ │ ├── Commands
│ │ │ │ │ │ ├── ConcluirTarefaCommand.cs
│ │ │ │ │ │ ├── CreateTarefaCommand.cs
│ │ │ │ │ │ ├── DeleteTarefaCommand.cs
│ │ │ │ │ │ └── UpdateTarefaCommand.cs
│ │ │ │ │ ├── DTOs
│ │ │ │ │ │ └── TarefaDto.cs
│ │ │ │ │ ├── Queries
│ │ │ │ │ │ ├── GetTarefasByBlocoQuery.cs
│ │ │ │ │ │ └── GetTarefasVencendoQuery.cs
│ │ │ │ │ └── Validators
│ │ │ │ │ └── CreateTarefaValidator.cs
│ │ │ │ └── Nucleos.Application.csproj
│ │ │ └── Nucleos.Application.UnitTests
│ │ │ └── ApplicationUnitTests.cs
│ │ ├── 3. Domain
│ │ │ ├── Nucleos.Domain
│ │ │ │ ├── Entities
│ │ │ │ │ ├── AIContext.cs
│ │ │ │ │ ├── AIInsight.cs
│ │ │ │ │ ├── AIInteraction.cs
│ │ │ │ │ ├── ActivityLog.cs
│ │ │ │ │ ├── AuditableEntity.cs
│ │ │ │ │ ├── BaseEntity.cs
│ │ │ │ │ ├── Bloco.cs
│ │ │ │ │ ├── BlocoCalculo.cs
│ │ │ │ │ ├── CalendarioEvento.cs
│ │ │ │ │ ├── Categoria.cs
│ │ │ │ │ ├── Conquista.cs
│ │ │ │ │ ├── EnergyLog.cs
│ │ │ │ │ ├── Habito.cs
│ │ │ │ │ ├── HabitoRegistro.cs
│ │ │ │ │ ├── ItemLista.cs
│ │ │ │ │ ├── Lista.cs
│ │ │ │ │ ├── Meta.cs
│ │ │ │ │ ├── Notification.cs
│ │ │ │ │ ├── Nucleo.cs
│ │ │ │ │ ├── NucleoAchievement.cs
│ │ │ │ │ ├── NucleoCompartilhamento.cs
│ │ │ │ │ ├── NucleoIcon.cs
│ │ │ │ │ ├── NucleoRelation.cs
│ │ │ │ │ ├── PasswordReset.cs
│ │ │ │ │ ├── Plan.cs
│ │ │ │ │ ├── SoftDeleteEntity.cs
│ │ │ │ │ ├── Streak.cs
│ │ │ │ │ ├── Subscription.cs
│ │ │ │ │ ├── Tarefa.cs
│ │ │ │ │ ├── Timer.cs
│ │ │ │ │ ├── User.cs
│ │ │ │ │ ├── UserConquista.cs
│ │ │ │ │ ├── UserLevel.cs
│ │ │ │ │ ├── UserPreference.cs
│ │ │ │ │ ├── UserProfile.cs
│ │ │ │ │ ├── UserRole.cs
│ │ │ │ │ ├── UserSecurity.cs
│ │ │ │ │ └── XP_Log.cs
│ │ │ │ ├── Enums
│ │ │ │ │ ├── FrequenciaHabito.cs
│ │ │ │ │ ├── PermissaoCompartilhamento.cs
│ │ │ │ │ ├── PrioridadeTarefa.cs
│ │ │ │ │ ├── StatusTarefa.cs
│ │ │ │ │ ├── TipoBloco.cs
│ │ │ │ │ ├── TipoLista.cs
│ │ │ │ │ ├── TipoNucleo.cs
│ │ │ │ │ ├── TipoOperacaoCalculo.cs
│ │ │ │ │ └── UserRoleEnum.cs
│ │ │ │ ├── Events
│ │ │ │ │ ├── HabitoRegistradoEvent.cs
│ │ │ │ │ ├── NivelAlcancadoEvent.cs
│ │ │ │ │ ├── TarefaConcluidaEvent.cs
│ │ │ │ │ └── UserRegisteredEvent.cs
│ │ │ │ ├── Interfaces
│ │ │ │ │ ├── IBlocoRepository.cs
│ │ │ │ │ ├── INucleoRepository.cs
│ │ │ │ │ ├── IRepository.cs
│ │ │ │ │ └── IUserRepository.cs
│ │ │ │ ├── Nucleos.Domain.csproj
│ │ │ │ ├── Specifications
│ │ │ │ │ ├── BaseSpecification.cs
│ │ │ │ │ ├── TarefaSpecifications.cs
│ │ │ │ │ └── UserSpecifications.cs
│ │ │ │ └── ValueObjects
│ │ │ │ ├── CPF.cs
│ │ │ │ ├── Cor.cs
│ │ │ │ ├── Email.cs
│ │ │ │ └── Telefone.cs
│ │ │ └── Nucleos.Domain.UnitTests
│ │ │ └── DomainUnitTests.cs
│ │ └── 4. Infrastructure
│ │ ├── Nucleos.Infrastructure
│ │ │ ├── AI
│ │ │ │ ├── ContextBuilder.cs
│ │ │ │ ├── IAIService.cs
│ │ │ │ ├── OpenAIService.cs
│ │ │ │ └── PromptTemplates.cs
│ │ │ ├── Calculo
│ │ │ │ ├── CalculoEngine.cs
│ │ │ │ ├── FiltroProcessor.cs
│ │ │ │ └── Operacoes
│ │ │ │ ├── ContagemOperacao.cs
│ │ │ │ ├── MaxOperacao.cs
│ │ │ │ ├── MediaOperacao.cs
│ │ │ │ ├── MinOperacao.cs
│ │ │ │ └── SomaOperacao.cs
│ │ │ ├── DependencyInjection.cs
│ │ │ ├── External
│ │ │ │ ├── GoogleCalendarService.cs
│ │ │ │ └── StripeService.cs
│ │ │ ├── Gamification
│ │ │ │ ├── ConquistaChecker.cs
│ │ │ │ ├── GamificationEngine.cs
│ │ │ │ ├── LevelCalculator.cs
│ │ │ │ └── StreakCalculator.cs
│ │ │ ├── Identity
│ │ │ │ ├── CurrentUserService.cs
│ │ │ │ ├── JwtGenerator.cs
│ │ │ │ └── PasswordHasher.cs
│ │ │ ├── Nucleos.Infrastructure.csproj
│ │ │ ├── Persistence
│ │ │ │ ├── Configurations
│ │ │ │ │ ├── BlocoConfiguration.cs
│ │ │ │ │ ├── ItemListaConfiguration.cs
│ │ │ │ │ ├── ListaConfiguration.cs
│ │ │ │ │ ├── NucleoConfiguration.cs
│ │ │ │ │ ├── TarefaConfiguration.cs
│ │ │ │ │ └── UserConfiguration.cs
│ │ │ │ ├── Context
│ │ │ │ │ └── NucleosDbContext.cs
│ │ │ │ ├── Migrations
│ │ │ │ ├── Repositories
│ │ │ │ │ ├── BlocoRepository.cs
│ │ │ │ │ ├── NucleoRepository.cs
│ │ │ │ │ ├── Repository.cs
│ │ │ │ │ └── UserRepository.cs
│ │ │ │ └── UnitOfWork
│ │ │ │ └── UnitOfWork.cs
│ │ │ └── Services
│ │ │ ├── BackgroundJobs
│ │ │ │ ├── HangfireService.cs
│ │ │ │ ├── IJobService.cs
│ │ │ │ └── Jobs
│ │ │ │ ├── CalculateStreaksJob.cs
│ │ │ │ ├── GenerateInsightsJob.cs
│ │ │ │ └── SendEmailJob.cs
│ │ │ ├── Cache
│ │ │ │ ├── ICacheService.cs
│ │ │ │ ├── MemoryCacheService.cs
│ │ │ │ └── RedisCacheService.cs
│ │ │ ├── DateTime
│ │ │ │ ├── DateTimeService.cs
│ │ │ │ └── IDateTime.cs
│ │ │ ├── Email
│ │ │ │ ├── EmailService.cs
│ │ │ │ ├── EmailTemplateService.cs
│ │ │ │ ├── IEmailService.cs
│ │ │ │ └── Templates
│ │ │ ├── FileStorage
│ │ │ │ ├── IFileStorageService.cs
│ │ │ │ ├── LocalFileStorageService.cs
│ │ │ │ └── S3FileStorageService.cs
│ │ │ ├── Notifications
│ │ │ │ ├── INotificationService.cs
│ │ │ │ ├── IRealTimeNotifier.cs
│ │ │ │ └── NotificationService.cs
│ │ │ └── Queue
│ │ │ ├── IQueueService.cs
│ │ │ └── RabbitMQService.cs
│ │ └── Nucleos.Infrastructure.IntegrationTests
│ │ └── IntegrationTests.cs
│ └── tests
│ ├── Nucleos.Architecture.Tests
│ │ └── ArchitectureTests.cs
│ ├── Nucleos.FunctionalTests
│ │ └── FunctionalTests.cs
│ └── Nucleos.UnitTests
│ └── UnitTest1.cs
└── estructure.md