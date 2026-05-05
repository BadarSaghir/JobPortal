This is the comprehensive **`README.md`** for the **Career 635** project. It serves as the master guide for developers and system administrators, covering everything from the architecture and local asset setup to database management on Linux.

***

# Career 635: Personnel Recruitment & Requisition Portal

**Career 635** is a high-end, enterprise-grade recruitment management system built with **ASP.NET Core MVC** and a **CQS (Command Query Separation)** architecture. It is designed for organizational anonymity, merit-based automated screening, and high-fidelity personnel dossier management.

---

## 🏛 Architecture & Design Philosophy

- **Single-Project Vertical Slice:** Logic is organized by business feature (Auth, Jobs, Applicants) rather than technical layers.
- **CQS Pattern:** 
    - **Paramore.Brighter:** Handles all state changes (Commands).
    - **Paramore.Darker:** Handles all data retrieval (Queries) for maximum read performance.
- **Snapshot Logic:** Every job application creates a permanent, immutable snapshot of the applicant's 8-page bio-data. This ensures historical audit integrity even if the user's master profile changes later.
- **Permission-Based Identity:** Beyond simple roles, the system uses granular claims-based permissions (e.g., `jobs.manage`, `applicants.export`).
- **Matte Emerald Aesthetic:** A premium, professional UI built with **Tailwind CSS v4.0** and **Lucide Icons**, served entirely from local assets (No CDNs).

---

## 🛠 Tech Stack

| Category | Technology |
| :--- | :--- |
| **Framework** | .NET 10.0 / .NET 9.0 (LTS) |
| **Frontend** | ASP.NET Core MVC + Razor Views |
| **Styling** | Tailwind CSS v4.0 (Standalone CLI) |
| **Icons** | Lucide Icons (Local JS) |
| **Logic Engine** | Brighter (Cmd) & Darker (Query) |
| **Database** | MSSQL (Linux/Docker) & PostgreSQL |
| **ORM** | Entity Framework Core + Dapper (for high-speed reads) |
| **Background Jobs** | Quartz.NET (Scheduled tasks & ZIP processing) |
| **Documentation** | Scalar (OpenAPI 3.1 / .NET 9 Native) |

---

## 🚀 Getting Started (Linux & Windows)

### 1. Prerequisites
- **.NET SDK** (9.0 or 10.0)
- **MSSQL for Linux** (Docker recommended)
- **`dotnet-ef`** global tool:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

### 2. Local Asset Setup (No CDN)
Career 635 serves all assets locally. Ensure you run these commands in the `wwwroot` directory to initialize:

```bash
# Download Lucide Icons
curl -Lo wwwroot/lib/lucide.min.js "https://unpkg.com/lucide@latest/dist/umd/lucide.min.js"

# Download Tailwind v4 CLI (Windows)
curl -LO "https://github.com/tailwindlabs/tailwindcss/releases/latest/download/tailwindcss-windows-x64.exe"
mv tailwindcss-windows-x64.exe tailwind.exe

# Download Tailwind v4 CLI (Linux)
curl -LO "https://github.com/tailwindlabs/tailwindcss/releases/latest/download/tailwindcss-linux-x64"
chmod +x tailwindcss-linux-x64
mv tailwindcss-linux-x64 tailwind
```

### 3. Database Initialization
Update `appsettings.json` with your Linux `sa` credentials.

**Generate and Apply Migrations:**
```bash
# Generate SQL Server migration
dotnet ef migrations add InitialCreate -o Infrastructure/Persistence/Migrations/SqlServer --context AppDbContext

# Apply to Database
dotnet ef database update --context AppDbContext
```

---

## 📂 Project Structure

```text
Career635/
├── Areas/
│   ├── Admin/               # Personnel management & Requisitions
│   └── Candidate/           # 8-page digital dossier entry
├── Domain/
│   ├── Entities/            # Normalized Applicant, Job, & Auth entities
│   └── Common/              # ISoftDeletable & BaseEntity
├── Infrastructure/
│   ├── Persistence/         # DbContext, Configurations, Repositories
│   ├── DependencyInjection/ # Modular DI Hub
│   └── Jobs/                # Quartz.NET Background tasks
├── Features/                # CQS Handlers (Brighter/Darker)
├── wwwroot/
│   ├── dist/                # Compiled Tailwind v4 CSS
│   └── uploads/             # Protected CV & Photo storage
└── Program.cs               # Minimal entry point
```

---

## ⚡ Key Management Commands

### Compiling CSS (Tailwind v4)
Run this in a separate terminal during development:
```bash
./tailwind -i ./wwwroot/css/site.css -o ./wwwroot/dist/style.css --watch
```

### Exporting Production SQL
To generate an idempotent SQL script for the Linux server:
```bash
dotnet ef migrations script --output deployment.sql --context AppDbContext --idempotent
```

### Running Background Tasks
The following tasks run automatically via Quartz:
- **Auto-Close:** Closes `Published` jobs whose `ExpiresAt` date has passed.
- **Dossier ZIP:** Handles the background zipping of applicant CVs, photos, and Excel reports.

---

## 🔐 Security & Access

### Default Credentials (Seeded)
- **URL:** `/Account/Login`
- **Username:** `admin@career635.com`
- **Password:** `Admin@635!`

### Permission Enforcement
The system uses a **Dossier-safe** policy.
- `[Authorize(Policy = AppPermissions.JobsManage)]`: Create/Edit vacancies.
- `[Authorize(Policy = AppPermissions.ApplicantsView)]`: Access 8-page candidate dossiers.
- `[Authorize(Policy = AppPermissions.ApplicantsExport)]`: Trigger ZIP/Excel generation.

---

## 📝 Developer Notes

1. **Soft Delete:** Never call `context.Remove()`. The system automatically intercepts deletes and sets `IsDeleted = true`.
2. **Markdown:** Position descriptions support full Markdown. Use the **EasyMDE** editor in the admin panel.
3. **Print Friendly:** All Dossier Review pages are optimized for A4 printing. Sidebars and UI noise are automatically removed in the print preview.
4. **File Storage:** Files are saved in `[Root]/[CampId]/[JobId]/[CNIC]`. This ensures organized backups on the Linux file system.

***
**© 2025 Career 635 Personnel Division.** Authorized use only.