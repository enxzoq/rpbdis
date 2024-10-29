using System;
using System.Collections.Generic;

namespace Lab2;

public partial class Subscriber
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? HomeAddress { get; set; }

    public string? PassportData { get; set; }

    public virtual ICollection<ServiceContract> ServiceContracts { get; set; } = new List<ServiceContract>();
}
