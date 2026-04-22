namespace RentACarDB
{
    public class RentalDTO
    {
        public int ID { get; set; }
        public int carID { get; set; }
        public int customerID { get; set; }

        public DateOnly startDate { get; set; }
        public DateOnly endDate { get; set; }
        public float totalPrice     { get; set; }

        public bool avaible {  get; set; }

    }
    public class RentalAddDTO
    {
       
        public int carID { get; set; }
        public int customerID { get; set; }

        public DateOnly startDate { get; set; }
        public DateOnly endDate { get; set; }
        public float totalPrice { get; set; }

        public bool avaible { get; set; }

    }
}
