using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_engine.Models;

public partial class FinancialYear:IBaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public List<TearSheetOutput> TearSheetOutputs { get; set; } = new List<TearSheetOutput>();

    public List<DriverTearSheetOutput> DriverTearSheetOutputs { get; set; } = new List<DriverTearSheetOutput>();

}

