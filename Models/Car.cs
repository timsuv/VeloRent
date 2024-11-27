using System;
using System.Collections.Generic;

namespace VeloRent.Models;

public partial class Car
{
    public int Id { get; set; }

    public int CarTypeId { get; set; }

    public int LocationId { get; set; }

    public string LicensePlate { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int Mileage { get; set; }

    public string? MaintenanceStatus { get; set; }

    public virtual CarType CarType { get; set; } = null!;

    public virtual CarRentalLocation Location { get; set; } = null!;

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
