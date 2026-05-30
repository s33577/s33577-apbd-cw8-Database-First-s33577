using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormatTEST.Entities;

public class PCComponent
{
    public int PCId { get; set; }
    [ForeignKey(nameof(PCId))]
    public PC PC { get; set; }
    
    [MaxLength(10)]
    public string ComponentCode { get; set; }
    
    [ForeignKey(nameof(ComponentCode))]
    public Component Component { get; set; }
    
    public int Amount { get; set; }
}