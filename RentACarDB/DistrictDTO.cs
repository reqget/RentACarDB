namespace RentACarDB
{
    public class DistrictDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int? CityId { get; set; }
    }
    public class DistrictAddDTO
    {

        public string? Name { get; set; }

        public int? CityId { get; set; }
    }
}
