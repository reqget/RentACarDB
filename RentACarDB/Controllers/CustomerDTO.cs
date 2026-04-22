namespace RentACarDB.Controllers
{
    public class CustomerDTO
    {
        public string name { get; set; }

        public string surname { get; set; }

        public string password { get; set; }

        public DateOnly date { get; set; }
    }
}
