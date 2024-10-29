using System;
using System.Collections.Generic;

namespace Lab2;

public partial class ServiceStatistic
{
    public int Id { get; set; }

    public int? ContractId { get; set; }

    public int? CallDuration { get; set; }

    public int? Smscount { get; set; }

    public int? Mmscount { get; set; }

    public int? DataTransferAmount { get; set; }

    public virtual ServiceContract? Contract { get; set; }
}
