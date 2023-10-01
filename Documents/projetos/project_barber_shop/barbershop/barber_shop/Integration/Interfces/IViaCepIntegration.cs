using barber_shop.Integration.Response;

namespace barber_shop.Integration.Interfces
{
    public interface IViaCepIntegration
    {
        Task<ViaCepResponse> GetDataViaCep(string zipCode);
    }
}
