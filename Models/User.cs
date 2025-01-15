using System.ComponentModel.DataAnnotations;

namespace asp_rest_model.Models;

public class User
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Nome é obrigatório")]
    public string Name { get; set; } = string.Empty;
}