using System.ComponentModel.DataAnnotations;

namespace FormatTEST.Entities;

public class ComponentManuFacturer
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string Abbreviation { get; set; }
    
    [Required]
    [MaxLength(300)]
    public string FullName { get; set; }
    
    public DateTime FoundationDate { get; set; }
    
    public ICollection<Component> Components { get; set; } = new List<Component>();
    
    
    
    
}