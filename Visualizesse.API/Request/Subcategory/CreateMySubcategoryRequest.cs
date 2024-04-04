using System.ComponentModel.DataAnnotations;

namespace Visualizesse.API.Request.Subcategory;

public class CreateMySubcategoryRequest(int categoryId, string description)
{
    [Required]
    public int CategoryId { get; set; } = categoryId;
    
    [Required]
    public string Description { get; set; } = description;
}