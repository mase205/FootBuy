using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace project.Models;

[PrimaryKey(nameof(UserID), nameof(PlayerID))]
public class Wishlist {
    [Required]
    public int UserID {get; set;}
    [Required]
    public int PlayerID {get; set;}
}