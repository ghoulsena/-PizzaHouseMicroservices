using OrderService.Application.Interface;
using OrderService.Application.DTOs;

namespace OrderService.Http;

public class CatalogHttpClient: ICatalogHttpClient
{
    private readonly HttpClient _http;

    public CatalogHttpClient(HttpClient http) => _http = http;
    public async Task<T?> SendAsync<T>(string url)
    {
        var response = await _http.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Pizza not found. Status code: {response.StatusCode}");

        return await response.Content.ReadFromJsonAsync<T?>();
    }
   
    public async Task<DTOPizza?> GetPizzaAsync(Guid id) => await SendAsync<DTOPizza>($"api/pizza/{id}") ?? throw new Exception("Pizza not found");
    public async Task<List<DTOPizza>> GetAllPizzaAsync() => await SendAsync<List<DTOPizza>>($"api/pizza") ?? throw new Exception(" not found");




}