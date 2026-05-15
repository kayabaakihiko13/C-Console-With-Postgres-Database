using System;
namespace Users.Models
{
    public record UsersDTO(
        string UserId, 
        string Firstname, 
        string Lastname, 
        string Username, 
        string Email, 
        DateTime CreateAt
    );
    // DTO khusus untuk menampung input register
    public record RegisterInput(
        string Firstname, 
        string Lastname, 
        string Username, 
        string Email, 
        string Password
    );
}