DeliveryService/
│
├── Application/                <-- Правила бизнеса, интерфейсы, DTO
│   ├── Interfaces/
│   │   ├── ICourierRepository.cs
│   │   ├── IDeliveryTaskRepository.cs
│   │   └── IUnitOfWork.cs
│   ├── Services/
│   │   ├── CourierService.cs
│   │   └── DeliveryTaskService.cs
│   ├── DTOs/
│   │   ├── CourierDto.cs
│   │   └── DeliveryTaskDto.cs
│   └── Common/
│       ├── Exceptions/
│       └── Validators/
│
├── Domain/                     <-- Чистые бизнес-сущности
│   ├── Entities/
│   │   ├── Courier.cs
│   │   └── DeliveryTask.cs
│   └── Enums/
│       └── OrderStatuses.cs
│
├── Infrastructure/             <-- Работа с внешним миром (БД, API, логирование)
│   ├── Persistence/
│   │   ├── CourierDbContext.cs
│   │   ├── Repositories/
│   │   │   ├── CourierRepository.cs
│   │   │   └── DeliveryTaskRepository.cs
│   │   └── Migrations/
│   ├── Logging/
│   │   └── SerilogConfig.cs
│   └── External/
│       └── DeliveryServiceClient.cs
│
├── WebApi/ (или просто DeliveryService/)
│   ├── Controllers/
│   │   ├── CourierController.cs
│   │   └── DeliveryTaskController.cs
│   ├── Configuration/
│   │   └── ConfigServiceColl.cs
│   ├── Middlewares/
│   │   ├── ExceptionHandlingMiddleware.cs
│   │   └── RequestLoggingMiddleware.cs
│   ├── appsettings.json
│   ├── Program.cs
│   └── DeliveryService.http
│
└── Tests/                      <-- Unit и Integration тесты
    ├── Application.Tests/
    ├── Infrastructure.Tests/
    └── WebApi.Tests/
