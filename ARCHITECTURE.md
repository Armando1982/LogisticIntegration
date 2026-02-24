# Architecture Specification: LogisticIntegration (CyB 2.0)

## 1. Core Principles
* **Architecture:** Clean Architecture + Domain-Driven Design (DDD).
* **Language:** English strictly for all code, properties, and comments. Spanish for business discussions.
* **Paradigm:** Favor Composition over Inheritance (No deep inheritance trees for roles).
* **Persistence:** Domain entities must be 100% agnostic of Entity Framework Core (No Data Annotations like `[Key]` or `[Table]`).
* **State Management:** Private setters for all properties. State changes only via public Domain Methods.

## 2. Ubiquitous Language (Glossary)
* **Trip / Collection:** `CollectionTrip`
* **Truck:** `Truck`
* **Driver:** `Driver`
* **Yard / Yard Manager:** `Yard` / `YardManager`
* **Weighbridge / Scale:** `Weighbridge`
* **Weighing Order (OPD):** `WeighingOrder`
* **Hopper:** `Hopper`
* **Weights:** `GrossWeight`, `TareWeight`, `NetWeight`, `DocumentaryWeight`
* **Weight Difference:** `WeightDifference` (Formula: Physical Net Weight - Documentary Weight)
* **Shrinkage / Penalty:** `Shrinkage` / `Penalty`

## 3. Bounded Contexts & Core Aggregates

### A. Shared Kernel (Identity)
* `Employee`: Physical person and system identity. Base for all operational roles via Composition.

### B. Logistics Context (Transit & Fleet)
* `CollectionTrip`: Aggregate Root managing the lifecycle of a trip (Scheduled -> InTransit -> InYard).
* `Driver`: References `EmployeeId`.

### C. Weighing & Yard Context
* `WeighingOrder`: Aggregate Root. Enforces physical rules (Tare cannot be > Gross). Calculates `NetWeight` automatically.
* `WeightReading`: Value object for scale captures.
* `HopperDischarge`: Tracks physical unloading to specific hoppers, including 'Tomcat' interventions.

### D. Settlement Context (Financial)
* `TripSettlement`: Aggregate Root for financial reconciliation.
* **Business Rule 1 (Consolidated Prorating):** Up to 10% shrinkage is absorbed. 
* **Business Rule 2 (Penalty):** Negative differences exceeding tolerance are charged to the driver at the MAXIMUM provider price of that trip.