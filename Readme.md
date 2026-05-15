# C# Database Application

Aplikasi manajemen database PostgreSQL dengan fitur authentication (Register & Login) menggunakan C# .NET 9.0.

## Fitur

- Koneksi ke PostgreSQL
- Registrasi user baru
- Login user dengan email/username
- Password di-hash menggunakan BCrypt
- CLI interaktif

## Struktur Project

```
C# Database/
├── src/
│   ├── Program.cs              # Entry point aplikasi
│   ├── model/
│   │   └── Users.cs            # Model DTO
│   ├── Repository/
│   │   └── AuthRepository.cs  # Logika auth (register/login)
│   └── utils/
│       └── getConnection.cs    # Konfigurasi database
├── .env.dev                    # Environment variables
├── C# Database.csproj         # Project file
└── README.md
```

## Prerequisites

- .NET 9.0 SDK
- PostgreSQL (atau database lain sesuai konfigurasi)
- dotenv.net, Npgsql, BCrypt.Net-Next

## Setup

### 1. Clone/Setup Project

```bash
dotnet restore
```

### 2. Konfigurasi Database

Edit file `.env.dev` sesuai konfigurasi PostgreSQL Anda:

```env
DB_HOST=localhost
DB_PORT=5432
DB_USER=<masukan username>
DB_PASS=<masukan password>
DB_NAME=<masukan database>
```

### 3. Buat Tabel Users

Jalankan SQL ini di database PostgreSQL Anda:

```sql
CREATE TABLE IF NOT EXISTS Users (
    user_id VARCHAR(100) PRIMARY KEY,
    firstname VARCHAR(100) NOT NULL,
    lastname VARCHAR(100) NOT NULL,
    username VARCHAR(100) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    create_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

## Build & Run

### Build

```bash
dotnet build
```

### Run dengan Environment File

```bash
dotnet run -- --env-file .env.dev
```

Atau jika sudah di-build:

```bash
bin/Debug/net9.0/"C# Database.exe" --env-file .env.dev
```

## Penggunaan

```
=== Aplikasi Database C# ===

=== Menu ===
1. Register
2. Login
3. Exit
Pilih: 
```

### Register
Pilih menu `1`, lalu masukkan:
- Firstname
- Lastname
- Username
- Email
- Password

### Login
Pilih menu `2`, lalu masukkan:
- Username atau Email
- Password

## Dependencies

| Package | Version | Description |
|---------|---------|-------------|
| Npgsql | 10.0.2 | PostgreSQL driver |
| BCrypt.Net-Next | 4.2.0 | Password hashing |
| dotenv.net | 4.0.2 | Environment loader |

## Lisensi

MIT