using System.ComponentModel.DataAnnotations.Schema;

namespace Visualizesse.Domain.Entities;

public class Subcategory(string description, int categoryId, Guid? userId = null)
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    public string Description { get; private set; } = description;
    public int CategoryId { get; private set; } = categoryId;
    public Guid? UserId { get; private set; } = userId;
    
    // Propriedade de navegação para representar o relacionamento com User
    public Category Category { get; set; }
    public User? User { get; set; }
}