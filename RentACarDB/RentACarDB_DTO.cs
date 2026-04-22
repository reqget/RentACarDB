using Microsoft.AspNetCore.Http;

namespace RentACarDB
{
    public class RentACarDB_DTO
    {

        public int ID { get; set; }
        public string brand {  get; set; }
        public string model { get; set; }
        public short year { get; set; }
        public string DailyPrice { get; set; }

        public string? Plate { get; set; }

        public bool Avaible { get; set; }

        public int? DistrictId { get; set; }

        public int? RentCount { get; set; }
        public IFormFile? FileImage { get; set; }
    }
    public class RentACarDBAdd_DTO
    {

          
        public string brand { get; set; }
        public string model { get; set; }
        public short year { get; set; }
        public string? DailyPrice { get; set; }

        public string? Plate { get; set; }

        public bool Avaible { get; set; }

        public int? DistrictId { get; set; }

        public int? RentCount { get; set; }
        public IFormFile? FileImage { get; set; }
        
    }
}
