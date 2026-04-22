using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarDB.Models;
using System.Drawing.Drawing2D;
using static RentACarDB.RentACarDB_DTO;
namespace RentACarDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        public readonly RentacardbContext _context;
        public CarController(RentacardbContext contenxt)
        {
            _context = contenxt;
        }

        [HttpGet("List")]
        public IActionResult GetList()
        {
            var model = _context.Cars.ToList();
            return Ok(model);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromForm] RentACarDBAdd_DTO input)
        {
            try
            {
                string fileName = null;

                if (input.FileImage != null && input.FileImage.Length > 0)
                {

                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(input.FileImage.FileName);


                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "cars");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var filePath = Path.Combine(uploadsFolder, fileName);


                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        input.FileImage.CopyToAsync(stream);
                    }
                }

                var car = new Models.Car
                {
                    Brand = input.brand,
                    Year = input.year,
                    Model = input.model,
                    DailyPrice = input.DailyPrice,
                    Plate = input.Plate,
                    Avaible = input.Avaible,
                    DistrictId = input.DistrictId,
                    RentCount = input.RentCount,
                    ImageName = fileName
                };

                _context.Cars.Add(car);
                _context.SaveChanges();

                return Ok(new { StatusCode = 200, message = "Araç başarı ile eklendi.", Data = car });
            }
            catch (Exception Ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    message = "Araç eklenemedi! Hata: " + Ex.Message,
                    inner = Ex.InnerException?.Message
                });
            }
        }
        [HttpDelete("Delete")]
        public IActionResult Remove(int id)
        {
            try
            {
                var car = _context.Cars
                    .Include(c => c.Rentals)
                    .FirstOrDefault(c => c.CarsId == id);

                if (car == null)
                    return NotFound(new { StatusCode = 404, message = $"{id} nolu Araç bulunamadı." });


                foreach (var rental in car.Rentals)
                {
                    rental.CarsId = null;
                }


                if (!string.IsNullOrEmpty(car.ImageName))
                {
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "cars");
                    var filePath = Path.Combine(uploadsPath, car.ImageName);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Cars.Remove(car);
                _context.SaveChanges();

                return Ok(new { StatusCode = 200, message = "Araç ve fotoğraf başarı ile silindi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    message = "Araç silinemedi! Hata: " + ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }


        [HttpPost("Update")]
        public IActionResult Update([FromForm] RentACarDB_DTO input)
        {
            try
            {
                var car = _context.Cars.Find(input.ID);

                if (car == null)
                    return NotFound(new { StatusCode = 404, message = input.ID + " nolu Araç bulunamadı." });

                var oldImageName = car.ImageName;
                if (input.FileImage != null && input.FileImage.Length > 0)
                {
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "cars");


                    var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(input.FileImage.FileName);
                    var newFilePath = Path.Combine(uploadsPath, newFileName);

                    using (var stream = new FileStream(newFilePath, FileMode.Create))
                    {
                        input.FileImage.CopyToAsync(stream);
                    }


                    if (!string.IsNullOrEmpty(oldImageName))
                    {
                        var oldFilePath = Path.Combine(uploadsPath, oldImageName);
                        if (System.IO.File.Exists(oldFilePath))
                            System.IO.File.Delete(oldFilePath);
                    }

                    car.ImageName = newFileName;
                }
                car.Brand = input.brand;
                car.Year = input.year;
                car.Model = input.model;
                car.DailyPrice = input.DailyPrice;
                car.Avaible = input.Avaible;
                car.Plate = input.Plate;
                car.DistrictId = input.DistrictId;
                car.RentCount = input.RentCount;


                _context.SaveChanges();
                return Ok(new { StatusCode = 200, message = "Araç başarı ile güncellendi.", car });
            }
            catch (Exception Ex)
            {

                return StatusCode(500, new { StatusCode = 500, message = "Araç Güncellenemedi! Hata: " + Ex.Message });
            }
        }
        [HttpGet("Find")]
        public IActionResult Find(int id)
        {
            try
            {
                var car = _context.Cars.Find(id);

                if (car == null)
                    return NotFound(new { StatusCode = 404, message = id + " nolu Araç bulunamadı." });


                return Ok(new
                {


                    StatusCode = 200,
                    message = "Araç başarı ile bulundu",
                    id = car.CarsId,
                    Brand = car.Brand,
                    Model = car.Model,
                    Year = car.Year,
                    DailyPrice = car.DailyPrice,
                    Plate = car.Plate,
                    Avaible = car.Avaible,
                    DistrictId = car.DistrictId,
                    RentCount = car.RentCount,
                    FileImage = car.ImageName,

                });


            }
            catch (Exception Ex)
            {
                return StatusCode(500, new { StatusCode = 500, message = "Araç Bulunamadı! Hata: " + Ex.Message });

            }
        }

        [HttpGet("GetPopularCars")]
        public IActionResult GetPopularCars()
        {
            try
            {
                // En çok kiralanan 5 arabayı getir
                var popularCars = _context.Cars
                    .OrderByDescending(c => c.RentCount)
                    .Take(5)
                    .Select(car => new
                    {
                        carsId = car.CarsId,
                        brand = car.Brand,
                        model = car.Model,
                        dailyPrice = car.DailyPrice,
                        imageName = car.ImageName,
                        rentCount = car.RentCount
                    })
                    .ToList();

                return Ok(popularCars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    message = "Popüler arabalar getirilemedi! Hata: " + ex.Message
                });
            }
        }
    }
}
