using System;
using System.Collections.Generic;

namespace RentACarDB.Models;

public partial class City
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<District> Districts { get; set; } = new List<District>();
}
