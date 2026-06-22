# Agent Instructions

Clean Architecture-based system for synchronizing user data from SQL Server to Salesforce using Azure Service Bus and Azure Functions.

The system is designed for reliability, scalability, and traceability using event-driven architecture and outbox pattern.

---

## Architecture Style

The solution follows Clean Architecture (SOC - Separation of Concerns):

- **Core**
  - Domain entities, enums, and core contracts
  - No external dependencies

- **Application**
  - Interfaces, DTOs, and business contracts
  - Defines use cases and service abstractions

- **Infrastructure**
  - Database access (EF Core)
  - Repository implementations
  - Outbox implementation
  - External integrations (Service Bus, Salesforce adapters)

- **Integration**
  - Service Bus publishers
  - Outbox publisher implementation
  - Cross-service communication layer

- **WebAPI**
  - REST APIs for User CRUD operations
  - Entry point from Angular UI
  - Responsible for writing to SQL and creating Outbox messages

- **Azure Functions**
  - Service Bus triggered processing
  - Salesforce synchronization logic
  - Schema validation and transformation

---

## End-to-End Data Flow

1. Angular UI calls Web API
2. Web API performs CRUD operation on SQL Server
3. Outbox record is created for domain event
4. Background hosted service processes Outbox table
5. Outbox publisher sends event to Azure Service Bus topic (`user-events`)
6. Azure Function listens to Service Bus subscription
7. Function validates schema against Salesforce metadata
8. If valid → data is upserted into Salesforce
9. Processing is logged and tracked end-to-end

---

## Key Design Patterns

- Repository Pattern (data access abstraction)
- Outbox Pattern (reliable event delivery)
- Background Hosted Service (Outbox processing worker)
- Event-driven architecture using Azure Service Bus Topics
- Clean separation between API, integration, and processing layers

---

## Reliability & Resilience Strategy

### Retry Handling
- Outbox publisher retries failed message publishing
- Azure Function retries Service Bus processing failures

### Failure Handling
- Messages exceeding retry threshold are marked as **DeadLetter**
- Errors are logged with structured context

### Idempotency Strategy
- EventId is used to avoid duplicate processing
- ExternalId is used for Salesforce upsert operations

---

## Observability & Logging

- Serilog used for structured logging
- CorrelationId is propagated across all layers:
  - Web API
  - Outbox Publisher
  - Service Bus Messages
  - Azure Functions

- Azure Application Insights used for telemetry and monitoring
- Logs include:
  - CorrelationId
  - EventId
  - UserId
  - MessageId

---

## Salesforce Integration Rules

- Schema is dynamically fetched before sync
- Schema validation is mandatory before upsert
- API version is configurable
- Object mapping strictly follows Salesforce metadata
- Any schema mismatch results in failed processing with logging

---

## Coding Principles

- Keep services small and focused (Single Responsibility Principle)
- Avoid business logic in controllers
- Use async/await for all I/O operations
- Prefer explicit code over abstraction
- Do not bypass Outbox for external communication
- Maintain clear separation between domain, application, and infrastructure layers

---

## Messaging Contract

- Service Bus Topic: `user-events`
- Events must include:
  - EventId
  - CorrelationId
  - UserId
  - OccurredOn

---

## Monitoring Expectations

- Every request must be traceable using CorrelationId
- Every event must be traceable using EventId
- Failures must never be silent
- Dead-lettered messages must be observable and diagnosable

---

## Development Guidelines

- Keep architecture consistent across all modules
- Do not introduce tight coupling between WebAPI and Salesforce
- Ensure deterministic behavior in Azure Functions