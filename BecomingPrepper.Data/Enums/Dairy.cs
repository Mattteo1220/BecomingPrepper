using System.ComponentModel;

namespace BecomingPrepper.Data.Enums
{
    public enum Dairy
    {
        [Description("Powdered Milk")]
        PowderedMilk = 1,

        [Description("Infant Formula")]
        InfantFormula = 2,

        [Description("Evaporated Milk")]
        EvaporatedMilk = 3,

        [Description("Whey Milk")]
        WheyMilk = 4,
    }
}
