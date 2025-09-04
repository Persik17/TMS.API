using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Application.Dto.Board
{
    public class BoardAnalyticsDto
    {
        public List<VelocityPointDto> Velocity { get; set; } = [];
        public List<BurnDownPointDto> BurnDown { get; set; } = [];
        public List<CfdPointDto> CFD { get; set; } = [];
    }
}
