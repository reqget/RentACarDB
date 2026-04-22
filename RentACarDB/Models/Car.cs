using System;
using System.Collections.Generic;

namespace RentACarDB.Models;

public partial class Car
{
    public int? CarsId { get; set; }

    public string? Brand { get; set; }

    public string? Model { get; set; }

    public short? Year { get; set; }

    public string? DailyPrice { get; set; }

    public string? Plate { get; set; }

    public bool Avaible { get; set; }

    public int? DistrictId { get; set; }

    public int? RentCount { get; set; }

    public string? ImageName { get; set; }

   
    public virtual District? District { get; set; }

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
