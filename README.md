# Car-Mender | Distributed Workshop Management (Incomplete)

A distributed system backend designed for managing automotive repair lifecycles and multi-tenant organizational structures.

> **Project Status:** This repository is **on hold indefinitely**. Development ceased during the early backend implementation phase due to shifting priorities. It remains as a reference for specific .NET patterns.

## 🏗️ Architecture & Context
*High-level overview of the implemented backend logic.*

* **Objective:** Establish a multi-tenant management engine with strict referential integrity between companies, branches, and workers.
* **Architecture Pattern:** Clean Architecture with **CQRS** (Command Query Responsibility Segregation) via MediatR.
* **Data Flow:** API Controllers -> MediatR Commands/Queries -> Domain Logic Handlers -> EF Core (SQL Server).

## ⚖️ Key Design Decisions
*Technical justifications for the core implemented features.*

* **Partial Updates: JSON Patch (RFC 6902)**
    * **Decision:** Implementation of `application/json-patch+json` using `NewtonsoftJson`.
    * **Rationale:** To allow granular state mutations of complex entities (like Appointments) without the need for full-payload transmissions.
    * **Trade-off:** Required custom AutoMapper profiles to safely map dynamic patches to tracked Domain Entities.

* **Communication: MediatR-based CQRS**
    * **Decision:** Total decoupling of read and write operations.
    * **Rationale:** Ensures that business logic remains isolated within specific Command Handlers, facilitating easier testing and future scalability.
    * **Trade-off:** Increased the initial file count and project complexity for a small-scale domain.

## 🧠 Engineering Challenges
*Specific technical hurdles addressed during development.*

* **Hierarchical Tenant Isolation:**
    * **Problem:** Preventing data leakage in a Company > Branch > Worker hierarchy.
    * **Implementation:** Enforced multi-level validation in Handlers to verify that resources (e.g., a specific vehicle) belong to the correct Branch before establishing new transactional links.
    * **Outcome:** Guaranteed referential consistency at the application level.

## 🛠️ Tech Stack
* **Core:** .NET 8 (C#)
* **Persistence:** SQL Server, Entity Framework Core 8
* **Tooling:** MediatR, AutoMapper, FluentValidation, xUnit

## 🙋‍♂️ Author

**Kamil Fudala**

- [GitHub](https://github.com/FreakyF)
- [LinkedIn](https://www.linkedin.com/in/kamil-fudala/)

## ⚖️ License

This project is licensed under the [MIT License](LICENSE).
