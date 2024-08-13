using System.ComponentModel.DataAnnotations;

namespace Entities.TransferObjects
{
    public record CompoundInterstModel
    {
        [Range(0, int.MaxValue, ErrorMessage = "Start sum can't be below 0")]
        public int StartSum { get; set; } = 0;

        [Required(ErrorMessage = "Interest Rate required")]
        [Range(0, 1, ErrorMessage = "Interest rate can't be below 0 and more than 100!")]
        public double YearInterestRate { get; init; }

        [Range(1, 365, ErrorMessage = "In year there are only 365 days")]
        public int NumberOfPeriods { get; set; } = 12;

        [Required(ErrorMessage = "You must provide necessaery amount of years.")]
        public int YearsNumber { get; init; }

        public bool ifReinvestment { get; set; } = true;
    }
}
