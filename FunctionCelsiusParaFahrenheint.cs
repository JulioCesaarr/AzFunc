using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace ConversaoFahrenheint
{
    public class FunctionCelsiusParaFahrenheint
    {
        private readonly ILogger<FunctionCelsiusParaFahrenheint> _logger;

        public FunctionCelsiusParaFahrenheint(ILogger<FunctionCelsiusParaFahrenheint> log)
        {
            _logger = log;
        }

        [FunctionName("FunctionCelsiusParaFahrenheint")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Conversão" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "celsius", In = ParameterLocation.Path, Required = true, Type = typeof(double), Description = "O valor em **Celsius** para conversão em fahrenheint")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "Retorna o valor em fahrenheint")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ConverterCelsiusParaFahrenheint/{celsius}")] HttpRequest req, 
            double celsius)
        {
            _logger.LogInformation($"Parametro recebido: {celsius}", celsius);

            var valorEmFahrenheint = (celsius * 9) / 5 + 32;

            string responseMessage = $"O valor em Celsius: {celsius} em Fahrenhei é: {valorEmFahrenheint}";
            _logger.LogInformation($"Conversão Efetuada. Resultado: {valorEmFahrenheint}");

            return new OkObjectResult(responseMessage);
        }
    }
}

