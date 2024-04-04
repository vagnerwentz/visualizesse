namespace Visualizesse.Domain.Entities;

public class Category(int id, string description)
{
    public int Id { get; private set; } = id;
    public string Description { get; private set; } = description;
}