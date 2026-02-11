# Fintech Payment & Ledger Service

A high-throughput payment processing API built with **.NET 9**, designed to simulate real-world fintech transaction constraints such as:

- Idempotent payment handling
- Double-spend prevention
- Concurrent balance updates
- Transaction integrity
- Redis-backed idempotency & caching
- MySQL transactional consistency
- Dockerized infrastructure

---

## Problem Statement

In fintech systems, payment APIs must guarantee:

1. No double charges
2. No inconsistent balances
3. Safe concurrent updates
4. Idempotent retries from clients
5. Strong transactional integrity

This project simulates a simplified payment gateway where:

- Users have accounts
- Clients initiate payments
- The system debits the sender
- Credits the receiver
- Records ledger entries
- Ensures atomic transaction consistency

---

## Architecture Overview

Client
│
▼
ASP.NET Core API
│
├── MySQL (ACID transactions)
│ ├── Accounts
│ ├── PaymentTransactions
│ └── LedgerEntries
│
└── Redis
├── Idempotency Keys
└── Rate limiting (future extension)

---

## Key Engineering Decisions

### 1️. Why MySQL?

- Strong ACID compliance
- Transaction support with isolation levels
- Row-level locking support
- Widely used in fintech environments

Isolation level configured:

READ COMMITTED

---

### 2️. Why Redis?

Used for:

- Idempotency key storage
- Fast lookup of previous payment attempts
- Avoiding repeated database writes

Redis is chosen because:

- O(1) key lookup
- Extremely low latency
- Reduces DB contention under retry storms

---

### 3️. How Double-Spend Is Prevented

Inside a DB transaction:

- Account row is selected with concurrency control
- Balance is checked
- Balance updated
- Ledger entries inserted
- Payment transaction recorded

If any step fails → entire transaction rolls back.

---

### 4️. How Idempotency Works

Client sends:

Idempotency-Key: <unique-guid>

Flow:

1. Check Redis for key
2. If exists → return cached response
3. If not:
   - Process payment
   - Store response in Redis
   - Return result

This prevents:

- Double debit
- Retry duplication
- Network retry issues

---

### 5️. What Breaks First Under Load?

Likely bottlenecks:

1. MySQL row locks under heavy concurrent writes
2. Redis memory if idempotency TTL not managed
3. API thread pool saturation if no rate limiting

Mitigation strategies:

- Horizontal scaling
- Read replicas
- Queue-based payment processing
- Circuit breaker for downstream dependencies

---

## 📂 Project Structure

/src
	/LedgerFlow
		/LedgerFlow.Api
			Common/
			Payments/
			Resilience/
			Dockerfile
		/LedgerFlow.Domain
			/Entities
			/Enums
			/Exceptions
		/LedgerFlow.Infrastructure
			/Idempotency
			/Migrations
			/Persistence
			/Repositories
			/Seed
		/LedgerFlow.Migrations
/docker
	docker-compose.yml

/docs
	architecture.html
	failure-mode.html
	fintech-notes.html
	tradeoffs.html
	scaling.html
	tradeoffs.html

---

## 🛠 Tech Stack

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core 9
- MySQL 8
- Redis 7
- Docker & Docker Compose

---

## Running Locally (Docker)

```bash
cd docker
docker compose up --build
```

API available at:

http://localhost:8080

---

## 🧪 Example Payment Request

POST /api/payments

Headers:

Idempotency-Key: 123e4567-e89b-12d3-a456-426614174000

Body:

```json
{
  "fromAccountId": "11111111-1111-1111-1111-111111111111",
  "toAccountId": "22222222-2222-2222-2222-222222222222",
  "amount": 100.00
}
```

---

## Load Testing (Basic)

Tested with:

- 200 concurrent requests
- Same sender account
- Multiple idempotency keys

Observations:

- No double-spend occurred
- DB row locks observed under high contention
- Average response time increased linearly under lock pressure

Future improvements:

- Queue-based processing
- Optimistic concurrency alternative
- Read/write separation

---

## Production Hardening (Future Work)

- Distributed tracing (OpenTelemetry)
- Structured logging
- Circuit breaker
- Rate limiting middleware
- Background reconciliation job
- Fraud detection hooks

## What This Project Demonstrates

- Understanding of transactional integrity
- Concurrency handling
- Idempotency implementation
- Redis integration strategy
- Dockerized system design
- Fintech-grade thinking

## Author

Ahmad Afif Aulia Hariz
Software Engineer – Distributed Systems & Fintech Simulation