namespace TaskManagerApi.DTOs;

public class RegisterRequest
{
    public string Email { get; set; } = String.Empty;

    public string Password { get; set; } = String.Empty;
}