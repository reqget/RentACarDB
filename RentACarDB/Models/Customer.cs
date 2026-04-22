using System;
using System.Collections.Generic;

namespace RentACarDB.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerLastName { get; set; }

    public DateOnly? CustomerBirthDate { get; set; }

    public string? CustomerEmail { get; set; }

    public string? CustomerTellNo { get; set; }

    public string? CustomerPassword { get; set; }

    public int? Cities { get; set; }

    public int? District { get; set; }

    public virtual City? CitiesNavigation { get; set; }

    public virtual District? DistrictNavigation { get; set; }

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
