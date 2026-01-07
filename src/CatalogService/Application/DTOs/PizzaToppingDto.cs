namespace CatalogService.Application.DTOs
{
    public class PizzaToppingDto
    {
        public string? Name { get; set; }
        public int ExtraPrice { get; set; }
        public List<Guid> PizzaIds { get; set; } = new();
    }
}
