using System.Xml.Serialization;
using WorldRank.Application;
using WorldRank.Application.Interfaces;

namespace WorldRank.Gateway
{
    public class EcbHTTPClient : IEcbHttpClient
    {
        private readonly HttpClient _httpClient;

        public EcbHTTPClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<CurrencyRateDto>> GetLatestRatesAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", cancellationToken);

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

            var serializer = new XmlSerializer(typeof(Envelope));

            var responseDTO = (Envelope)serializer.Deserialize(stream);
            
            var cube = responseDTO.Cube.Cube1;
            
            var datetime = cube.time;

            var rates = cube.Cube.Select(x => new CurrencyRateDto(x.currency, x.rate, datetime)).ToArray();
            return rates;
        }
    }
}
