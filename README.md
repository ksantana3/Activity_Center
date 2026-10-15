# Activity Center

An activity planner I built with C# and ASP.NET Core MVC while learning full-stack web development. Users register, log in, plan activities, and join or leave activities that other users have planned.

> **Status:** Archived portfolio project — not maintained; dependencies are outdated and it is not intended for deployment.

More of my work: [kylesantana.com](https://kylesantana.com)

## Features

- Registration and login with model validation (required fields, email format, minimum lengths, password confirmation, duplicate email check)
- Passwords hashed with ASP.NET Core Identity's `PasswordHasher`
- Session-based authentication
- Dashboard of upcoming activities (past activities are hidden) with start/end time, duration, coordinator and participant count
- Create, edit and delete your own activities
- Join and leave other users' activities
- Activity detail page listing the participants
- Data model: one-to-many (a user plans many activities) and many-to-many (users join activities through a `Join` table), built with Entity Framework Core

## Tech stack

Versions are as they appear in the project files:

- .NET Core 2.1 (`netcoreapp2.1`), ASP.NET Core MVC with Razor views
- Entity Framework Core with the SQLite provider (`Microsoft.EntityFrameworkCore.Sqlite` 2.1.4)
- Bootstrap 3.3.7 (bundled in `wwwroot/lib`)

## Database and configuration

The app uses a local **SQLite** file, `mydb.db`, created in the project folder. The connection is set directly in code (`Startup.cs` and `Models/DBContext.cs`), so there is nothing to configure to run it.

`appsettings.json` also contains a `DBInfo` section with a MySQL connection string. It is left over from an earlier MySQL setup and is **not read by the code**. The password in it is a placeholder. If you wire MySQL back in, don't put a real password in `appsettings.json`; supply it outside the repo instead, for example:

```bash
# .NET user secrets (local development)
dotnet user-secrets init
dotnet user-secrets set "DBInfo:ConnectionString" "server=localhost;userid=root;password=<your password>;port=3306;database=mydb;SslMode=None"

# or an environment variable (note the double underscore)
export DBInfo__ConnectionString="server=localhost;userid=root;password=<your password>;port=3306;database=mydb;SslMode=None"
```

## Running it locally

You'll need the .NET Core 2.1 SDK, which is long out of support. Newer SDKs won't build a `netcoreapp2.1` project without changes.

```bash
git clone https://github.com/ksantana3/Activity_Center.git
cd Activity_Center
dotnet restore
```

No EF Core migrations are checked in, and the app calls `Database.Migrate()` on startup, so create an initial migration first or the tables won't exist:

```bash
dotnet ef migrations add InitialCreate
dotnet run
```

Then open http://localhost:5000/ (the `Activity_Center` launch profile in `Properties/launchSettings.json`).

## Project structure

```
Activity_Center/
├── Program.cs                 # Web host entry point
├── Startup.cs                 # Services (EF Core, sessions, MVC) and pipeline
├── appsettings.json
├── Controllers/
│   └── HomeController.cs      # All routes: auth, dashboard, activity CRUD, join/leave
├── Models/
│   ├── User.cs                # Registered user
│   ├── LoginUser.cs           # Login form model
│   ├── Activity.cs            # Planned activity
│   ├── Join.cs                # User <-> Activity join table
│   └── DBContext.cs           # EF Core DbContext (SQLite)
├── Views/
│   ├── Home/                  # Index (login/register), Home, NewActivity, Edit, ShowActivity
│   └── Shared/_Layout.cshtml
└── wwwroot/                   # CSS, JS, images, Bootstrap
```

Routes:

| Path | Purpose |
| --- | --- |
| `/` | Login and registration page |
| `/register`, `/login`, `/logout` | Auth actions |
| `/home` | Dashboard |
| `/activity/new`, `/activity` | New activity form / create |
| `/view/{id}` | Activity details |
| `/edit/{id}`, `/update/{id}` | Edit an activity |
| `/delete/{id}` | Delete an activity |
| `/join/{id}`, `/leave/{id}` | Join or leave an activity |

## Notes

This was a learning project, and I've left the code as it was. Some routes (edit, update, delete, view) don't check who is signed in, and delete is a plain GET link.
