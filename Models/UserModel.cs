using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models;

public class User {
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UID {get; set;}
    [Required]
    public string? Username {get; set;}
    [Required]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
    public string? Email{get; set;}
    [Required]
    public string? Club{get; set;}
    [Required]
    [DataType(DataType.Password)]
    [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
    public string? Password{get; set;}
}