using System.ComponentModel.DataAnnotations;

namespace FormatTEST.Entities;

public class PC
{
    
    [Key]
    public int Id { get; set; }
    
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    public float Weight { get; set; }
    public int Warranty { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Stock { get; set; }
    
    public ICollection<PCComponent> PCComponents { get; set; } = new List<PCComponent>();
}