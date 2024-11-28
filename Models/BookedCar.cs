using System;
using System.Collections.Generic;

namespace VeloRent.Models;

public partial class BookedCar
{
    public int Id { get; set; }

    public int CarId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int RentalId { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual Rental Rental { get; set; } = null!;
}
