using System;
using System.Collections.Generic;

namespace RentACarDB.Models;

public partial class District
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CityId { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual City? City { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
