using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models;

public class Player {
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PlayerID {get; set;}
    [Required]
    public string? Club {get; set;}
    [Required]
    public string? Country {get; set;}
    [Required]
    public string? Name {get; set;}
    public int Age {get; set;}
    [Required]
    public int Price {get; set;}
}