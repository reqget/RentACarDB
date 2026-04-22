using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarDB.Models;
using static RentACarDB.CityDTO;


namespace RentACarDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        public readonly RentacardbContext _context;

        public CityController(RentacardbContext contenxt)
        {
            _context = contenxt;
        }

        [HttpGet("List")]
        public IActionResult GetList()
        {
            var model = _context.Cities.ToList();
            return Ok(model);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromForm] CityAddDTO input)
        {
            try
            {
                var city = new City

                {
                    Name = input.Name,
                  
                    
                };

                _context.Cities.Add(city);
                _context.SaveChanges();

                return Ok(new { StatusCode = 200, message = "Şehir başarı ile eklendi.", Data = city });
            }
            catch (Exception Ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    message = "! Hata: " + Ex.Message,
                    inner = Ex.InnerException.Message
                });
            }
        }

        [HttpDelete("Delete")]
        public IActionResult Remove(int id)
        {
            try
            {
                var city = _context.Cities
                    .Include(c => c.Districts)
                        .ThenInclude(d => d.Cars)
                    .Include(c => c.Districts)
                        .ThenInclude(d => d.Customers)
                    .FirstOrDefault(c => c.Id == id);

                if (city == null)
                    return NotFound(new { StatusCode = 404, message = "Şehir bulunamadı." });

             
                foreach (var district in city.Districts)
                {
                    foreach (var car in district.Cars)
                        car.DistrictId = null;

                    foreach (var customer in district.Customers)
                        customer.District = null;
                }

               
                _context.Districts.RemoveRange(city.Districts);

              
                _context.Cities.Remove(city);

                _context.SaveChanges();

                return Ok(new { StatusCode = 200, message = "Şehir ve bağlı ilçeler başarıyla silindi. Bağlı kayıtlar null olarak güncellendi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    message = "Şehir silinemedi! Hata: " + ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }




        [HttpPost("Update")]
        public IActionResult Update([FromForm] CityDTO input)
        {
            try
            {
                var city = _context.Cities.Find(input.Id);

                if (city == null)
                    return NotFound(new { StatusCode = 404, message = input.Id + " nolu Şehir bulunamadı." });

                city.Name = input.Name;
              

                _context.SaveChanges();


                return Ok(new { StatusCode = 200, message = "Şehir başarı ile güncellendi.", city });
            }
            catch (Exception Ex)
            {

                return StatusCode(500, new { StatusCode = 500, message = "Şehir Güncellenemedi! Hata: " + Ex.Message });
            }
        }
        [HttpGet("Find")]
        public IActionResult Find(int id)
        {
            try
            {
                var city = _context.Cities.Find(id);

                if (city == null)
                    return NotFound(new { StatusCode = 404, message = id + " nolu Şehir bulunamadı." });


                return Ok(new
                {


                    StatusCode = 200,
                    message = "Şehir başarı ile bulundu",
                    name = city.Name,
                    id = city.Id
                });


            }
            catch (Exception Ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "Şehir Bulunamadı! Hata: " + Ex.Message });

            }
        }



    }
}
