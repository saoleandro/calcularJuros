using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CalcularJuros.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CalcularJuros.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CalculaJurosController : ControllerBase
    {
        private readonly IHttpContextAccessor _accessor;

        public CalculaJurosController(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

      
        [HttpGet]
        [Route("{initValue}/{month}")]
        public async Task<IActionResult> GetCalcular(decimal initValue, int month)
        {
            try
            {
                string finalValue = string.Empty;

                using (var client = new HttpClient())
                {
                    var url = _accessor.HttpContext?.Request?.Host.ToString();
                    client.BaseAddress = new Uri(string.Concat("http://",url));

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetDepartments using HttpClient  
                    HttpResponseMessage Res = await client.GetAsync("TaxaJuros");

                    if (Res.IsSuccessStatusCode)
                    {
                        var ObjResponse = Res.Content.ReadAsStringAsync().Result;
                        string taxaJuros = JsonConvert.DeserializeObject<string>(ObjResponse);
                        double valueTaxa = Convert.ToDouble(Convert.ToDecimal(initValue) * (1 + Convert.ToDecimal(taxaJuros)));
                        decimal valorFinal = decimal.Parse(Math.Pow(valueTaxa, Convert.ToDouble(month)).ToString(), NumberStyles.AllowExponent);
                        var div = "1".PadRight(valorFinal.ToString().Length - 2, char.Parse("0"));
                        finalValue = (valorFinal / Int64.Parse(div)).ToString("N2");

                    }

                    return Ok(finalValue);
                }

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}