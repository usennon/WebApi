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
        public int StartInterval { get; init; }

        [Required(ErrorMessage = "Interval Required")]
        public int EndInterval { get; init; }

        [Required(ErrorMessage = "Interval Required")]
        public int IntervalsAmount { get; init; }
    }
}
