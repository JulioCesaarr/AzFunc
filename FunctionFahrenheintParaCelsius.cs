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
    public class FunctionFahrenheintParaCelsius
    {
        private readonly ILogger<FunctionFahrenheintParaCelsius> _logger;

        public FunctionFahrenheintParaCelsius(ILogger<FunctionFahrenheintParaCelsius> log)
        {
            _logger = log;
        }

        [FunctionName("ConverterFahrenheintParaCelsius")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Conversão" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "fahrenheint", In = ParameterLocation.Path, Required = true, Type = typeof(double), Description = "O valor em **Fahrenheint** para conversão em celsius")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "Retorna o valor em celsius")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ConverterFahrenheintParaCelsius/{fahrenheint}")] HttpRequest req, 
            double fahrenheint)
        {
            _logger.LogInformation($"Parametro recebido: {fahrenheint}", fahrenheint);

            var valorEmCelsius = (fahrenheint - 32) * 5 / 9;

            string responseMessage = $"O valor em Fahrenheint: {fahrenheint} em Celsius é: {valorEmCelsius}";
            _logger.LogInformation($"Conversão Efetuada. Resultado: {valorEmCelsius}");

            return new OkObjectResult(responseMessage);
        }
    }
}

