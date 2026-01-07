using DeliveryService.Application.Interface;
using DeliveryService.Domian.Entity;
using static System.Net.WebRequestMethods;

namespace DeliveryService.Http
{
    public class DeliveryServiceClient :IDeliveryServiceClient
    {
        private readonly HttpClient _httpClient;

        public DeliveryServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task ValidateOrderExistsAsync(Guid orderId)
        {
            var response = await _httpClient.GetAsync($"/api/Order/{orderId}");
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Заказ с таким Id не найден");
            }
        }
    }
}
