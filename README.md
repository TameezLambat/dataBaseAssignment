# dataBaseAssignment

## Overview
NetEaseDB is a web application built with ASP.NET Core MVC that allows users to manage event bookings, venues, and event information.

## Features
- Create, view, edit, and delete Venues, Events, and Bookings
- Prevent deletion of Venues and Events if they are associated with active Bookings
- Display success and error messages using TempData
- Venue image support via URL (e.g., for Azure Blob Storage)
- Prevention of double bookings

## Technologies Used
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server / LocalDB
- Bootstrap (styling)
- Azure Blob Storage (for images)

## How to Run
1. Clone this repository.
2. Open the solution in Visual Studio.
3. Apply migrations and update the database.
4. Account for any secrets:
   - Azure Blob Storage connection string
   - Database connection string
5. Run the project.

## Folder Structure
- `Controllers/` – MVC Controllers
- `Models/` – Entity Models
- `Views/` – Razor Pages
- `wwwroot/` – Static files
- `README.md` – Project documentation

## Future Enhancements
- Advanced filtering with event type lookup.
