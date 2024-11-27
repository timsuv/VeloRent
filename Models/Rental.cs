using System;
using System.Collections.Generic;

namespace VeloRent.Models;

public partial class Rental
{
    public int Id { get; set; }

    public int CarId { get; set; }

    public int CustomerId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal TotalCost { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
