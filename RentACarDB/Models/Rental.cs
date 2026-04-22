using System;
using System.Collections.Generic;

namespace RentACarDB.Models;

public partial class Rental
{
    public int Id { get; set; }

    public int? CarsId { get; set; }

    public int? CustomerId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public float? TotalPrice { get; set; }

    public bool? Avaible { get; set; }

    public virtual Car? Cars { get; set; }

    public virtual Customer? Customer { get; set; }
}
