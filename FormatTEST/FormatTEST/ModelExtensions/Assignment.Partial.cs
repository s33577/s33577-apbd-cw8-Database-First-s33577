namespace FormatTEST.Models;

public partial class Assignment
{
    public bool IsOverdue(DateTime date)
    {
        return DueDate < date;
    }
    
}
