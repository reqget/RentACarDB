using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarDB.Models;
using static RentACarDB.DistrictDTO;


namespace RentACarDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        public readonly RentacardbContext _context;

        public DistrictController(RentacardbContext contenxt)
        {
            _context = contenxt;
        }

        [HttpGet("List")]
        public IActionResult GetList()
        {
            var model = _context.Districts.ToList();
            return Ok(model);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromForm] DistrictAddDTO input)
        {
            try
            {
                var district = new District

                {
                    Name = input.Name,
                    CityId = input.CityId

                };

                _context.Districts.Add(district);
                _context.SaveChanges();

                return Ok(new { StatusCode = 200, message = "İlçe başarı ile eklendi.", Data = district });
            }
            catch (Exception Ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    message = "! Hata: " + Ex.Message
                });
            }
        }

        [HttpDelete("Delete")]
        public IActionResult Remove(int id)
        {
            try
            {
                var district = _context.Districts
                    .Include(d => d.Cars)
                    .Include(d => d.Customers)
                    .FirstOrDefault(d => d.Id == id);

                if (district == null)
                    return NotFound(new { StatusCode = 404, message = id + " nolu İlçe bulunamadı." });

              
                foreach (var car in district.Cars)
                {
                    car.DistrictId = null;
                }

               
                foreach (var customer in district.Customers)
                {
                    customer.District = null;
                }

                _context.Districts.Remove(district);
                _context.SaveChanges();

                return Ok(new { StatusCode = 200, message = "İlçe başarıyla silindi. Bağlı kayıtlar güncellendi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    message = "İlçe silinemedi! Hata: " + ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        [HttpPost("Update")]
        public IActionResult Update([FromForm] DistrictDTO input)
        {
            try
            {
                var district = _context.Districts.Find(input.Id);

                if (district == null)
                    return NotFound(new { StatusCode = 404, message = input.Id + " nolu İlçe bulunamadı." });

                district.Name = input.Name;
                _context.SaveChanges();


                return Ok(new { StatusCode = 200, message = "İlçe başarı ile güncellendi.", district });
            }
            catch (Exception Ex)
            {

                return StatusCode(500, new { StatusCode = 500, message = "İlçe Güncellenemedi! Hata: " + Ex.Message });
            }
        }
        [HttpGet("Find")]
        public IActionResult Find(int id)
        {
            try
            {
                var district = _context.Districts.Find(id);

                if (district == null)
                    return NotFound(new { StatusCode = 404, message = id + " nolu İlçe bulunamadı." });


                return Ok(new
                {


                    StatusCode = 200,
                    message = "İlçe başarı ile bulundu",
                    id = district.Id,
                    name = district.Name,
                    cityId = district.CityId,
                    


                });


            }
            catch (Exception Ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "İlçe Bulunamadı! Hata: " + Ex.Message });

            }
        }

    }
}
