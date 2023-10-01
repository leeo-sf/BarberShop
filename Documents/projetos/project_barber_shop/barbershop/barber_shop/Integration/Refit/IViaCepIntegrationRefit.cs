using barber_shop.Integration.Response;
using Refit;

namespace barber_shop.Integration.Refit
{
    public interface IViaCepIntegrationRefit
    {
        //Refit é uma ferramenta para facilitar a comunicação com oustras api's
        //Essa interface busca os dados na api via cep
        [Get("/ws/{zipCode}/json")]
        Task<ApiResponse<ViaCepResponse>> GetDataViaCep(string zipCode);
    }
}
