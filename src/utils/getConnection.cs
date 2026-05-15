using System;
using System.IO;
using Npgsql;

namespace CSharpDatabase.Utils
{
    public static class DatabaseConfig
    {
        static DatabaseConfig()
        {
            IntializeEnviroment();
        }
        private static void IntializeEnviroment()
        {
            string envFilePath = ".env";
            string [] args = Environment.GetCommandLineArgs();
            // check flag --env-file
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--env-file" && i + 1 < args.Length)
                {
                    envFilePath = args[i + 1];
                    break;
                }
            }
            // lalu jalankan parser internal
            LoadEnvFile(envFilePath);
        }
        private static void LoadEnvFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"[DatabaseConfig] File konfigurasi '{filePath}' tidak ditemukan!");
            }

            foreach (var line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                var parts = line.Split('=', 2);
                if (parts.Length != 2) continue;

                var key = parts[0].Trim();
                var value = parts[1].Trim();

                Environment.SetEnvironmentVariable(key, value);
            }
        }
        private static string ConnectionString
        {
            get
            {
                string host = Environment.GetEnvironmentVariable("DB_HOST");
                string port = Environment.GetEnvironmentVariable("DB_PORT");
                string user = Environment.GetEnvironmentVariable("DB_USER");
                string pass = Environment.GetEnvironmentVariable("DB_PASS");
                string name = Environment.GetEnvironmentVariable("DB_NAME");

                if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(name))
                {
                    throw new InvalidOperationException("[DatabaseConfig] Gagal memuat variabel database. Periksa kembali isi file .env Anda.");
                }

                return $"Host={host};Port={port};Username={user};Password={pass};Database={name}";
            }
        }
        /// <summary>
        /// Membuat dan mengembalikan objek koneksi NpgsqlConnection.
        /// File .env akan dibaca otomatis saat method ini dipanggil pertama kali.
        /// </summary>
        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }
    }
}
