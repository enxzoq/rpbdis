using System;
using System.Collections.Generic;

namespace Lab2;

public partial class VwSubscriberContract
{
    public int SubscriberId { get; set; }

    public string? SubscriberFullName { get; set; }

    public string? HomeAddress { get; set; }

    public string? PassportData { get; set; }

    public int ContractId { get; set; }

    public DateOnly? ContractDate { get; set; }

    public string? PhoneNumber { get; set; }

    public string TariffPlanName { get; set; } = null!;

    public decimal? SubscriptionFee { get; set; }

    public string? EmployeeFullName { get; set; }

    public string? Position { get; set; }

    public string? Education { get; set; }
}
