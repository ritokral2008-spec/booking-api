# BookingApi

REST API для управления бронированиями помещений.

Проект реализован на **ASP.NET Core / .NET 10** с использованием PostgreSQL, Entity Framework Core, Redis и JWT-аутентификации. Приложение полностью запускается через Docker Compose.

---

## 🚀 Возможности

* 🔐 JWT-аутентификация
* 👤 Регистрация и авторизация пользователей
* 🏠 CRUD для помещений
* 📅 Создание и управление бронированиями
* 🔎 Фильтрация, сортировка и пагинация
* ⚡ Кэширование с Redis
* 🗄️ PostgreSQL + Entity Framework Core
* ✅ Валидация входных данных с FluentValidation
* 🛡️ Централизованная обработка исключений
* 📖 Swagger / OpenAPI
* 🐳 Полный запуск проекта через Docker Compose
* 🔄 Автоматическое применение EF Core migrations

---

## 🛠️ Технологии

| Технология                   | Назначение                  |
| ---------------------------- | --------------------------- |
| **C# / .NET 10**             | Основной язык и платформа   |
| **ASP.NET Core Web API**     | REST API                    |
| **Entity Framework Core 10** | ORM                         |
| **PostgreSQL**               | Основная база данных        |
| **Redis**                    | Кэширование                 |
| **JWT Bearer**               | Аутентификация              |
| **FluentValidation**         | Валидация DTO               |
| **Swagger / OpenAPI**        | Документация API            |
| **Docker / Docker Compose**  | Контейнеризация             |
| **xUnit**                    | Интеграционное тестирование |

---

## 🏗️ Архитектура

Проект разделён на несколько уровней ответственности:

```text
Controllers
    ↓
Services
    ↓
Repositories
    ↓
Entity Framework Core
    ↓
PostgreSQL
```

Дополнительно:

```text
                    ┌──────────────┐
                    │   Client     │
                    └──────┬───────┘
                           │ HTTP
                           ▼
                    ┌──────────────┐
                    │ Controllers  │
                    └──────┬───────┘
                           ▼
                    ┌──────────────┐
                    │   Services   │
                    └──────┬───────┘
                           ▼
                    ┌──────────────┐
                    │ Repositories │
                    └──────┬───────┘
                           ▼
                    ┌──────────────┐
                    │ PostgreSQL   │
                    └──────────────┘

                    ┌──────────────┐
                    │    Redis     │
                    └──────────────┘
                           ▲
                           │
                    BookingService
```

### Основные слои

**Controllers**

Отвечают за HTTP-запросы и ответы API.

**Services**

Содержат бизнес-логику приложения.

**Repositories**

Отвечают за взаимодействие с базой данных.

**DTOs**

Используются для передачи данных между API и клиентом.

**Validators**

Проверяют корректность входных данных.

**Middleware**

Централизованно обрабатывает исключения и формирует единый формат ошибок.

---

## 📁 Структура проекта
BookingApi/
│
├── Controllers/
│   ├── AuthController.cs
│   ├── BookingController.cs
│   ├── RoomController.cs
│   └── UserController.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── DTOs/
│   ├── Auth/
│   ├── Booking/
│   ├── Room/
│   └── User/
│
├── Entities/
│   ├── Booking.cs
│   ├── Room.cs
│   └── User.cs
│
├── ExceptionMiddleware/
│   └── ExceptionMiddleware.cs
│
├── Repositories/
│   ├── Interfaces/
│   └── ...
│
├── Services/
│   ├── Interfaces/
│   └── ...
│
├── Migrations/
│
├── Program.cs
├── Dockerfile
├── docker-compose.yml
├── .env.example
└── .gitignore
## 🔐 Аутентификация

API использует JWT Bearer Authentication.

После авторизации клиент получает JWT-токен, который необходимо передавать в HTTP-запросах:

Authorization: Bearer <token>

Swagger также настроен для работы с JWT.

## 🗄️ База данных

Используется:

PostgreSQL 17

Entity Framework Core автоматически применяет существующие migrations при запуске приложения:

db.Database.Migrate();

Поэтому отдельный ручной запуск миграций после старта контейнеров не требуется.

## ⚡ Redis

Redis используется для кэширования данных.

В Docker Compose API подключается к Redis по имени сервиса:

redis:6379

Это позволяет контейнерам взаимодействовать внутри Docker-сети без использования localhost.

## 🐳 Запуск через Docker
Требования

Перед запуском необходимо установить:

Docker
Docker Compose

Проверить установку:

docker --version
docker compose version
1. Клонирование репозитория
git clone https://github.com/ritokral2008-spec/BookingApi.git
cd BookingApi
2. Создание .env

Скопировать пример конфигурации:

cp .env.example .env

Для Windows PowerShell:

Copy-Item .env.example .env

Пример:

POSTGRES_DB=BookingApi
POSTGRES_USER=postgres
POSTGRES_PASSWORD=change-me

JWT_ISSUER=BookingApi
JWT_AUDIENCE=BookingApiClient
JWT_KEY=change-me-to-a-long-random-key

.env содержит локальные секреты и не должен добавляться в Git.

3. Запуск
docker compose up --build

После запуска будут доступны:

API       → http://localhost:8080
Swagger   → http://localhost:8080/swagger
PostgreSQL → localhost:5432
Redis      → localhost:6379

Проверить состояние контейнеров:

docker compose ps

Остановить приложение:

docker compose down

Удалить контейнеры вместе с volumes:

docker compose down -v

-v удалит данные PostgreSQL, поэтому использовать эту команду только если база больше не нужна.

## 📖 Swagger

После запуска приложения документация API доступна по адресу:

http://localhost:8080/swagger

Swagger позволяет:

просматривать endpoints;
отправлять HTTP-запросы;
тестировать авторизацию;
передавать JWT Bearer Token;
проверять ответы API.

## 📌 Основные сущности

User

Пользователь системы.

User
 ├── Id
 ├── Username
 ├── PasswordHash
 └── Role
Room

Помещение, доступное для бронирования.

Room
 ├── Id
 ├── Name
 ├── Capacity
 └── ...
Booking

Бронирование помещения пользователем.

Booking
 ├── Id
 ├── UserId
 ├── RoomId
 ├── StartTime
 ├── EndTime
 └── Status

Возможные статусы бронирования:

Pending
Confirmed
Cancelled
## 🔎 Работа с бронированиями

API поддерживает:

создание бронирования;
получение списка бронирований;
получение конкретного бронирования;
обновление;
отмену;
подтверждение;
пагинацию;
фильтрацию;
сортировку.

Пример параметров запроса:

GET /api/bookings?page=1&pageSize=10
## 🧪 Тестирование

Для проекта предусмотрены интеграционные тесты.

Используется:

xUnit

Тесты работают с отдельной базой данных PostgreSQL.

Запуск тестов:

dotnet test
## ⚙️ Конфигурация

Основные настройки приложения передаются через environment variables.

PostgreSQL
ConnectionStrings__DefaultConnection
Redis
Redis__ConnectionString
JWT
Jwt__Issuer
Jwt__Audience
Jwt__Key

Docker Compose связывает эти переменные с соответствующими сервисами:

API
├── PostgreSQL
└── Redis
## 🔒 Безопасность

Секретные данные не хранятся в репозитории.

Локальные значения находятся в:

.env

а шаблон конфигурации:

.env.example

доступен в репозитории.

Для production необходимо использовать отдельные секреты и более безопасную инфраструктуру управления конфигурацией.

## 📋 Пример запуска
git clone https://github.com/ritokral2008-spec/BookingApi.git

cd BookingApi

cp .env.example .env

docker compose up --build

После этого открыть:

http://localhost:8080/swagger
## 📈 Что можно развивать дальше

Возможные направления развития проекта:

refresh tokens;
role-based authorization;
более подробные integration tests;
unit tests для business logic;
Docker multi-stage optimization;
CI/CD через GitHub Actions;
structured logging;
health checks;
rate limiting;
более продвинутое Redis-кэширование;
deployment в облачную инфраструктуру.
## 📄 License

Проект создан в учебных целях и используется для практики разработки backend-приложений на C#/.NET.
