﻿using System;
using System.Collections.Generic;

namespace VeloRent.Models;

public partial class CarRentalLocation
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
