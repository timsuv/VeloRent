using System;
using System.Collections.Generic;

namespace VeloRent.Models;

public partial class CarType
{
    public int Id { get; set; }

    public string Make { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string Type { get; set; } = null!;

    public decimal DailyRate { get; set; }

    public int NumberOfSeats { get; set; }

    public string FuelType { get; set; } = null!;

    public string AutomaticOrManual { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
