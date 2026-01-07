using OrderService.Application.DTOs;

namespace OrderService.Application.Interface;

public interface ICatalogHttpClient
{
    Task<T?> SendAsync<T>(string url);

    Task<DTOPizza?> GetPizzaAsync(Guid id);

    Task<List<DTOPizza>> GetAllPizzaAsync();
}
