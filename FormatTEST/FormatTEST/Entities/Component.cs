using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormatTEST.Entities;

public class Component
{
    [Key]
    [MaxLength(10)]
    public string Code { get; set; }
    
    [Required]
    [MaxLength(300)]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    public int ComponentManufacturersId { get; set; }
    [ForeignKey(nameof(ComponentManufacturersId))]
    public ComponentManuFacturer Manufacturers { get; set; }
    
    public int ComponentTypesId { get; set; }
    [ForeignKey(nameof(ComponentTypesId))]
    public ComponentType Type { get; set; }
    
    
    public ICollection<PCComponent> PCComponents { get; set; } = new List<PCComponent>();
    
    
}