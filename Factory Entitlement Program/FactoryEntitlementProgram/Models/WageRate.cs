namespace FactoryEntitlementProgram.Models
{
    public class WageRate
    {
        public int Id { get; set; }

        public decimal NormalHourRate { get; set; }
        public decimal OvertimeHourRate { get; set; }

        public DateTime EffectiveFrom { get; set; }
    }
}

