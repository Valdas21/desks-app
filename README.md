# desks-app

## Prerequisites

### Backend (`desks-api`)
- **.NET SDK**: .NET 8 
- **NuGet packages used**:
  - `Microsoft.EntityFrameworkCore (8.0.0)`
  - `Microsoft.EntityFrameworkCore.InMemory (8.0.0)`
  - `Swashbuckle.AspNetCore (6.6.2)`

> The API uses an **EF Core InMemory** database. No external DB setup is required.

### Seeded demo data
On startup, the app seeds the in-memory database via `SeedData.Initialize(...)` It creates:

**Users**
- `Id=1` Jonas Jonaitis — `jonas@gmail.com` / `123456`
- `Id=2` Petras Petraitis — `petras@gmail.com` / `123456`
- `Id=3` Juozas Juozaitis — `juozas@gmail.com` / `123456`

**Desks**
- Desk `1` (available)
- Desk `2` (available)
- Desk `3` (available)
- Desk `4` (**under maintenance**)
- Desk `5` (available)

**Reservations** (relative to current UTC date)
- Reservation `Id=1`: Desk `3`, User `1`, **tomorrow** 09:00–17:00 (UTC)
- Reservation `Id=2`: Desk `1`, User `2`, **in 2 days** 10:00–12:00 (UTC)
- Reservation `Id=3`: Desk `2`, User `2`, **past** (from 5 days ago to 2 days ago, UTC)

- ### FrontEnd (`desks-web`)
- React + TypeScript + Bootstrap library

## How to run

### Backend (`desks-api`)
1. Open a terminal in the `desks-api` folder.
2. Restore packages and run the API:
   dotnet run
### Frontend (`desks-web`)
1. Open a terminal in the `desks-web` folder.
2. Restore packages and run the API:
   - npm i
   - npm run dev

