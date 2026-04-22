using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RentACarDB.Models;
using static RentACarDB.RentalDTO;


namespace RentACarDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        public readonly RentacardbContext _context;

        public RentalController(RentacardbContext contenxt)
        {
            _context = contenxt;
        }
        [HttpGet("List")]
        public IActionResult GetList()
        {
            var model = _context.Rentals.ToList();
            return Ok(model);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromForm] RentalAddDTO input)
        {
            try
            {
                var rental = new Rental
                {
                    CarsId = input.carID,
                    CustomerId = input.customerID,
                    StartDate = input.startDate,
                    EndDate = input.endDate,
                    Avaible = input.avaible,
                    TotalPrice = input.totalPrice
                };
                _context.Rentals.Add(rental);
                _context.SaveChanges();

                return Ok(new { StatusCode = 200, message = "Veri başarı ile eklendi.", Data = rental });

            }
            catch (Exception Ex)
            {

                return StatusCode(500, new
                {
                    StatusCode = 500,
                    message = "Veri eklenemedi! Hata: " + Ex.Message,
                    inner = Ex.InnerException?.Message
                });
            }

        }
        [HttpDelete("Delete")]
        public IActionResult Remove(int id)
        {
            try
            {
                var rental = _context.Rentals.Find(id);
                if (rental == null)
                    return NotFound(new { StatusCode = 404, message = id + " nolu Kiralama bulunamadı." });

                _context.Rentals.Remove(rental);
                _context.SaveChanges();

                return Ok(new { StatusCode = 200, message = "Kiralama başarı ile silindi." });
            }
            catch (Exception Ex)
            {

                return StatusCode(500, new { StatusCode = 500, message = "Kiralama silinemedi! Hata: " + Ex.Message });
            }
        }

        [HttpPost("Update")]
        public IActionResult Update([FromForm] RentalDTO input)
        {
            try
            {
                var rental = _context.Rentals.Find(input.ID);

                    if(rental == null)
                    return NotFound(new { StatusCode = 404, message = input.ID + " nolu Kiralama bulunamadı." });

                rental.CarsId = input.carID;
                rental.CustomerId = input.customerID;
                rental.Avaible = input.avaible;
                rental.StartDate = input.startDate;
                rental.EndDate = input.endDate; 
                rental.TotalPrice = input.totalPrice;

                _context.SaveChanges();

                return Ok(new { StatusCode = 200, message = "Kiralama başarı ile güncellendi.", rental });

            }
            catch (Exception Ex)
            {

                return StatusCode(500, new { StatusCode = 500, message = "Kiralama Güncellenemedi! Hata: " + Ex.Message });
            }
        }
        [HttpGet("Find")]
        public IActionResult Find(int id)
        {
            try
            {
                var rental = _context.Rentals.Find(id);

                if (rental == null)
                    return NotFound(new { StatusCode = 404, message = id + " nolu Kiralama bulunamadı." });


                return Ok(new
                {

                    id = rental.Id,
                    StatusCode = 200,
                    message = "Kiralama başarı ile bulundu",
                    carID = rental.CarsId,
                    customerID = rental.CustomerId,
                    avaible = rental.Avaible,
                    startDate = rental.StartDate,
                    endDate = rental.EndDate,
                    totalPrice = rental.TotalPrice,


                });


            }
            catch (Exception Ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "Şehir Bulunamadı! Hata: " + Ex.Message });

            }
        }
    }
}
