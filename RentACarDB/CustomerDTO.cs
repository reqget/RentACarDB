namespace RentACarDB
{
    public class CustomerDTO
    {
        public int ID { get; set; }
        public string name { get; set; }

        public string surname { get; set; }

        public string password { get; set; }

        public string? CustomerEmail { get; set; }
        public DateOnly? CustomerBirthDate { get; set; }

        public string? CustomerTellNo { get; set; }

        public int? Cities { get; set; }

        public int? District { get; set; }
    }
    public class CustomerAddDTO
    {
       
        public string name { get; set; }

        public string surname { get; set; }

        public DateOnly? CustomerBirthDate { get; set; }

        public string password { get; set; }

        public string? CustomerEmail { get; set; }

        public string? CustomerTellNo { get; set; }

        public int? Cities { get; set; }

        public int? District { get; set; }
    }
}
