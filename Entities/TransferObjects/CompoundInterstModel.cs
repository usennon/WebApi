using System.ComponentModel.DataAnnotations;

namespace Entities.TransferObjects
{
    public record CompoundInterstModel
    {
        [Range(0, double.PositiveInfinity, ErrorMessage = "Start sum can't be below 0")]
        public int StartSum = 0;

        [Required(ErrorMessage = "Interest Rate required")]
        [Range(0, 1, ErrorMessage = "Interest rate can't be below 0 and more than 100!")]
        public double YearInterestRate;

        [Range(1, 365, ErrorMessage = "In year there are only 365 days")]
        public int NumberOfPeriods = 12;

        [Required(ErrorMessage = "You must provide necessaery amount of years.")]
        public int YearsNumber;

        public bool ifReinvestment = true;
    }
}
