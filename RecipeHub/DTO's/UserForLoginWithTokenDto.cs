using System.ComponentModel.DataAnnotations;

namespace RecipeHub.DTO_s;

public class UserForLoginWithTokenDto
{
    [Required]
    [MaxLength(256)]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [MaxLength(64)]
    public string Password { get; set; } = string.Empty;
}