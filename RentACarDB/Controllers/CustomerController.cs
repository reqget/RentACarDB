using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarDB.Models;
using static RentACarDB.CustomerDTO;

namespace RentACarDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public readonly RentacardbContext _context;

        public CustomerController(RentacardbContext contenxt)
        {
            _context = contenxt;
        }

        [HttpGet("List")]
        public IActionResult GetList()
        {
            var model = _context.Customers.ToList();
            return Ok(model);
        }

         [HttpPost("Add")]
        public IActionResult Add([FromForm] CustomerAddDTO input)
        {
            try
            {
                var customer = new Customer
                {
                    CustomerName = input.name,
                    CustomerLastName = input.surname,
                    CustomerPassword = input.password,
                    CustomerBirthDate = input.CustomerBirthDate,
                    CustomerEmail = input.CustomerEmail,
                    CustomerTellNo = input.CustomerTellNo,
                    Cities = input.Cities,
                    District = input.District,

                };

                _context.Customers.Add(customer);
                _context.SaveChanges();

                return Ok(new { StatusCode = 200, message = "Müşteri başarı ile eklendi.", Data = customer });
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
                var customer = _context.Customers
                    .Include(c => c.Rentals)
                    .FirstOrDefault(c => c.CustomerId == id);

                if (customer == null)
                    return NotFound(new { StatusCode = 404, message = id + " nolu Müşteri bulunamadı." });

                // Rental tablosundaki müşteri bağlantılarını null yap
                foreach (var rental in customer.Rentals)
                {
                    rental.CustomerId = null;
                }

                _context.Customers.Remove(customer);
                _context.SaveChanges();

                return Ok(new { StatusCode = 200, message = "Müşteri başarıyla silindi. Bağlı kiralama kayıtları güncellendi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    message = "Müşteri silinemedi! Hata: " + ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        [HttpPost("Update")]
        public IActionResult Update([FromForm] CustomerDTO input)
        {
            try
            {
                var customer = _context.Customers.Find(input.ID);

                if (customer == null)
                    return NotFound(new { StatusCode = 404, message = input.ID + " nolu Müşteri bulunamadı." });

                customer.CustomerName = input.name;
                customer.CustomerLastName = input.surname;
                customer.CustomerPassword = input.password;
                customer.CustomerEmail = input.CustomerEmail;
                customer.CustomerTellNo = input.CustomerTellNo;
                customer.Cities = input.Cities;
                customer.District = input.District;
              

                _context.SaveChanges();


                return Ok(new { StatusCode = 200, message = "Müşteri başarı ile güncellendi.", customer });
            }
            catch (Exception Ex)
            {

                return StatusCode(500, new { StatusCode = 500, message = "Müşteri Güncellenemedi! Hata: " + Ex.Message });
            }
        }
        [HttpGet("Find")]
        public IActionResult Find(int id)
        {
            try
            {
                var customer = _context.Customers.Find(id);

                if (customer == null)
                    return NotFound(new { StatusCode = 404, message = id + " nolu Müşteri bulunamadı." });


                return Ok(new
                {


                    StatusCode = 200,
                    message = "Müşteri başarı ile bulundu",
                    customer = customer.CustomerId,
                    name = customer.CustomerName,
                    surname = customer.CustomerLastName,
                    Year = customer.CustomerBirthDate,
                    password = customer.CustomerPassword,
                    customerEmail = customer.CustomerEmail,
                    customerTellNo = customer.CustomerTellNo,
                    cities = customer.Cities,
                    District = customer.District,


                });


            }
            catch (Exception Ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "Müşteri Bulunamadı! Hata: " + Ex.Message });

            }
        }
    }
}
