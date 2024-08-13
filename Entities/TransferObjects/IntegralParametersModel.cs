using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.TransferObjects
{
    public record IntegralParametersModel
    {
        [Required(ErrorMessage = "Interval Required")]
        public int StartInterval;

        [Required(ErrorMessage = "Interval Required")]
        public int EndInterval;

        [Required(ErrorMessage = "Interval Required")]
        public int IntervalsAmount;
    }
}
