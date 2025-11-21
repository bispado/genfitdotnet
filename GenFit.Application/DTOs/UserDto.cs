namespace GenFit.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Cpf { get; set; }
    public string? Telefone { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string? LinkedInUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateUserDto
{
    public string Role { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? SenhaHash { get; set; }
    public string? Cpf { get; set; }
    public string? Telefone { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string? LinkedInUrl { get; set; }
}

public class UpdateUserDto
{
    public string? Nome { get; set; }
    public string? Telefone { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string? LinkedInUrl { get; set; }
}
