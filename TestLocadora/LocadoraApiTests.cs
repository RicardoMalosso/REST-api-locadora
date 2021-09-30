using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using REST_api_locadora.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace TestLocadora
{
    public class LocadoraApiTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public LocadoraApiTests()
        {
            //Arrange
            //instancia um servidor de testes e um cliente http
            //fonte: https://docs.microsoft.com/pt-br/dotnet/architecture/microservices/multi-container-microservice-net-applications/test-aspnet-core-services-web-apps
            _server = new TestServer(new WebHostBuilder()
            .UseStartup<REST_api_locadora.Startup>());
            _client = _server.CreateClient();
        }
        [Theory]
        [InlineData("{}")]
        [InlineData("{\"Name\": -1 }")]
        [InlineData("{\"Name\": false }")]
        [InlineData("{\"Name\": null }")]

        public async Task RejectClienteBadName(string JSONData)
        {

            var contentString = new StringContent(JSONData, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //Act
            var response = await _client.PostAsync("/api/clientes", contentString);

            //Assert
            //requests com bad names devem retornar response codes 400. Bad Request.
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task testClientesRoutes()
        {
            //act
            var response = await _client.GetAsync("/api/clientes");
            //assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"Name\": -1 }")]
        [InlineData("{\"Name\": false }")]
        [InlineData("{\"Name\": null }")]

        public async Task RejectFilmeBadName(string JSONData)
        {

            var contentString = new StringContent(JSONData, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //Act
            var response = await _client.PostAsync("/api/filmes", contentString);

            //Assert
            //requests com bad names devem retornar response codes 400. Bad Request.
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task RejectDuplicateCliente()
        {
            var JSONData = "{\"Name\": \"Nome válido\" }";
            var contentString = new StringContent(JSONData, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //Act
            //primeiro insere e desconsidera o resp, depois tenta inserir com o registro identico já existindo
            await _client.PostAsync("/api/clientes", contentString);
            var response = await _client.PostAsync("/api/clientes", contentString);

            //Assert
            //nesse caso nomes não podem ser registrados duas vezes.
            //requests com bad names devem retornar response codes 400. Bad Request.
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task rentAMovieTests()
        {
            //registra filme e usuário
            var JSONData = "{\"Name\": \"Usuário José\" }";
            var contentString = new StringContent(JSONData, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync("/api/clientes", contentString);
            response.EnsureSuccessStatusCode();

            JSONData = "{\"Name\": \"Filme Titanic\" }";
            contentString = new StringContent(JSONData, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response = await _client.PostAsync("/api/filmes", contentString);
            response.EnsureSuccessStatusCode();

            //vários testes feitos nesse método pq ainda não sei persistir
            //dados de teste necessários como cadastro de filme e usuário na memória
            //entre mais de um método. Para evitar repetição de código igual, coloquei
            //diversos testes nesse mesmo método

            //aluga o filme 1 para o usuário 1
            JSONData = "{\"MovieId\": 1,\"RenterId\": 1}";
            contentString = new StringContent(JSONData, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response = await _client.PostAsync("/api/locacoes", contentString);
            response.EnsureSuccessStatusCode();

            //aluga o filme 1 para o usuário 1 novamente enquanto já está alugado!

            JSONData = "{\"MovieId\": 1,\"RenterId\": 1}";
            contentString = new StringContent(JSONData, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response = await _client.PostAsync("/api/locacoes", contentString);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

            //retorna a locação 1
            JSONData = "{\"Id\": 1}";
            contentString = new StringContent(JSONData, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response = await _client.PostAsync("/api/locacoes/retornar", contentString);
            response.EnsureSuccessStatusCode();

            //aluga novamente para o mesmo usuário, agora que foi retornado
            JSONData = "{\"MovieId\": 1,\"RenterId\": 1}";
            contentString = new StringContent(JSONData, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response = await _client.PostAsync("/api/locacoes", contentString);
            response.EnsureSuccessStatusCode();

        }
    }
}


