using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LightSaver;
using LightSaver.Models;
using LightSaver.Services;
using LightSaver.Infrastructure.DAO;
using LightSaver.Domain.Services;

namespace LightSaver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {

        private readonly PricesService _pricesService;

        public PricesController(PricesService pricesService)
        {
            _pricesService = pricesService;
        }



        /* GET: api/Prices
         * This method returns all the prices stored in the DataBase
         * 
         */
        [HttpGet]
        public async Task<IEnumerable<Price>> GetPrices()
        {
            return await _pricesService.GetPrices();
        }

        // GET: api/Prices/18012022
        [HttpGet("{dateString}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Price>>> GetDatePrices(string dateString)
        {
            int day, month, year;
            try
            {
                day = int.Parse(dateString.Substring(0, 2));
                month = int.Parse(dateString.Substring(2, 2));
                year = int.Parse(dateString.Substring(4, 4));
            } catch (Exception)
            {
                return BadRequest("You have to provide a date in ddMMyyyy format!");
            }

            DateTime date = new DateTime(year, month, day);
            IEnumerable<Price> result = await _pricesService.GetDatePrices(date);
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }
    }
}
