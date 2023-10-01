using barber_shop.Integration.Interfces;
using barber_shop.Integration.Refit;
using barber_shop.Integration.Response;

namespace barber_shop.Integration
{
    public class ViaCepIntegration : IViaCepIntegration
    {
        private readonly IViaCepIntegrationRefit _viaCepIntegrationRefit;

        public ViaCepIntegration(
            IViaCepIntegrationRefit viaCepIntegrationRefit
            )
        {
            _viaCepIntegrationRefit = viaCepIntegrationRefit;
        }

        public async Task<ViaCepResponse> GetDataViaCep(string zipCode)
        {
            var responseData = await _viaCepIntegrationRefit.GetDataViaCep(zipCode);

            if (responseData != null && responseData.IsSuccessStatusCode)
            {
                return responseData.Content;
            }

            return null;
        }
    }
}
