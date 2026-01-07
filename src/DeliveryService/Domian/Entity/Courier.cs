namespace DeliveryService.Domian.Entity
{
    public class Courier
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }  
        public bool IsAvailable { get; set; } = true;

        public static Courier Create(string name, bool isAvailable = true)
        {
            return new Courier
            {
                Id = Guid.NewGuid(),
            Name = name ?? string.Empty,
                IsAvailable = isAvailable
            };
        }


    }
}
