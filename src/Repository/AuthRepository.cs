using System;
using System.Threading.Tasks;
using Npgsql;
using BCrypt.Net;
using Users.Models;
using CSharpDatabase.Utils;

namespace Users.Repositories
{
    public static class AuthRepository
    {
        // Fungsi terpisah untuk pendaftaran pengguna baru
        public static async Task<bool> RegisterAsync(RegisterInput input)
        {
            // Mengamankan password menggunakan BCrypt sebelum disimpan
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(input.Password);
            string newUserId = Guid.NewGuid().ToString();

            // Kolom target diubah menjadi 'password'
            string sql = @"INSERT INTO Users (user_id, firstname, lastname, username, email, password) 
                           VALUES (@id, @first, @last, @user, @email, @pass);";

            await using var conn = DatabaseConfig.GetConnection();
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", newUserId);
            cmd.Parameters.AddWithValue("first", input.Firstname);
            cmd.Parameters.AddWithValue("last", input.Lastname);
            cmd.Parameters.AddWithValue("user", input.Username);
            cmd.Parameters.AddWithValue("email", input.Email);
            cmd.Parameters.AddWithValue("pass", passwordHash); // Hasil hash disimpan ke kolom password

            int rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        // Fungsi terpisah untuk memvalidasi masuk akun
        public static async Task<UsersDTO?> LoginAsync(string usernameOrEmail, string password)
        {
            // Mengambil kolom 'password' dari database
            string sql = @"SELECT user_id, firstname, lastname, username, email, password, create_at 
                           FROM Users 
                           WHERE username = @input OR email = @input;";

            await using var conn = DatabaseConfig.GetConnection();
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("input", usernameOrEmail);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                // Ambil string hash yang tersimpan di kolom password
                string storedHash = reader.GetString(5);

                // Verifikasi input teks biasa dengan hash dari kolom password
                if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                {
                    return new UsersDTO(
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.GetDateTime(6)
                    );
                }
            }

            return null;
        }
    }
}
