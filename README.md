# CloudShelf Blob Storage API 🌩️

A backend REST API built with **ASP.NET Core** that allows users to upload images via URL, store them securely on **Azure Blob Storage**, and retrieve them using temporary **SAS (Shared Access Signature)** URLs. All image metadata is persisted in **SQL Server** using **Entity Framework Core**.

---

## 🏗️ Tech Stack

| Technology | Purpose |
|---|---|
| ASP.NET Core (.NET 10) | Backend API framework |
| Azure Blob Storage | Cloud image storage |
| SQL Server | Metadata persistence |
| Entity Framework Core | ORM + Code First Migrations |
| Azure SAS Tokens | Secure temporary image access |

---

## 📁 Project Structure

```
CloudShelf_Blob_Storage_
├── Controllers
│   └── ImageController.cs        # API endpoints
├── Data
│   └── AppDBContext.cs           # EF Core DbContext
├── DTOs
│   ├── UploadImageRequest.cs     # Request model
│   └── ImageResponse.cs          # Response model
├── Models
│   └── Image.cs                  # Database entity
├── Services
│   ├── Interfaces
│   │   └── IBlobService.cs       # Service contract
│   └── Implementations
│       └── BlobService.cs        # Azure Blob logic
├── Migrations                    # EF Core migrations
├── appsettings.json              # Configuration
└── Program.cs                    # App entry point
```

---

## 🚀 Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) + SSMS
- [Azure Account](https://portal.azure.com)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)

---

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/CloudShelf-Blob-Storage.git
cd CloudShelf-Blob-Storage
```

---

### 2. Azure Setup

1. Go to [portal.azure.com](https://portal.azure.com)
2. Create a **Storage Account**
3. Inside it, create a **Container** named `images` (set to Private)
4. Go to **Access Keys** and copy:
   - Connection String
   - Account Name
   - Account Key

---

### 3. Configure appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=BlobStorage;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "AzureStorage": {
    "ConnectionString": "your-azure-connection-string",
    "ContainerName": "images",
    "AccountName": "your-account-name",
    "AccountKey": "your-account-key"
  }
}
```

> ⚠️ **Never commit appsettings.json to GitHub!** Add it to `.gitignore`

---

### 4. Run EF Core Migrations

Open **Package Manager Console** in Visual Studio:

```
Add-Migration InitialCreate
Update-Database
```

Verify the `Images` table was created in SSMS.

---

### 5. Run the Project

Press **F5** in Visual Studio or:

```bash
dotnet run
```

API will be available at `https://localhost:7001`

---

## 📡 API Endpoints

### Upload Image
```
POST /api/image/upload
```

**Request Body:**
```json
{
    "imageUrl": "https://example.com/photo.jpg"
}
```

**Response:**
```json
{
    "id": 1,
    "sasUrl": "https://yourstore.blob.core.windows.net/images/abc123.jpg?sv=...",
    "originalUrl": "https://example.com/photo.jpg",
    "uploadedAt": "2024-01-01T12:00:00"
}
```

---

### Retrieve Image
```
GET /api/image/{id}
```

**Response:**
```json
{
    "id": 1,
    "sasUrl": "https://yourstore.blob.core.windows.net/images/abc123.jpg?sv=...",
    "originalUrl": "https://example.com/photo.jpg",
    "uploadedAt": "2024-01-01T12:00:00"
}
```

> 💡 The `sasUrl` is valid for **1 hour**. Call the GET endpoint anytime to get a fresh link.

---

## 🔒 Security

- Azure container is set to **Private** — no public access
- Images are accessed via **SAS Tokens** that expire after 1 hour
- Credentials stored in `appsettings.json` — never hardcode or push to GitHub

---

## 🗄️ Database Schema

```sql
CREATE TABLE Images (
    Id          INT PRIMARY KEY IDENTITY,
    FileName    NVARCHAR(255),     -- Azure blob name (GUID)
    OriginalUrl NVARCHAR(500),     -- URL provided by user
    ContentType NVARCHAR(100),     -- image/jpeg, image/png etc
    UploadedAt  DATETIME           -- UTC timestamp
)
```

---

## 📦 NuGet Packages

```xml
<PackageReference Include="Azure.Storage.Blobs" Version="12.x.x" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0" />
```

---

## 🧪 Testing with Postman

1. Run the project in Visual Studio
2. Open Postman
3. Test `POST /api/image/upload` with a JSON body containing an `imageUrl`
4. Copy the returned `id`
5. Test `GET /api/image/{id}` with that id
6. Paste the `sasUrl` in your browser to view the image

---

## 🌱 Future Improvements

- [ ] Delete image endpoint
- [ ] Get all images endpoint
- [ ] Global error handling middleware
- [ ] Logging with Serilog
- [ ] Authentication with JWT
- [ ] Docker support

---

## 👨‍💻 Author

**Muhammad Huzaifa**  
Built with 💙 using ASP.NET Core + Azure
