using System.ComponentModel.DataAnnotations;

namespace FormatTEST.Entities;

public class ComponentType
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string Abbreviation { get; set; }
    
    [Required]
    [MaxLength(150)]
    public string Name { get; set; }
    
    
    public ICollection<Component> Components { get; set; } = new List<Component>();
    
}