using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Application.Dto.Board
{
    public class BurnDownPointDto
    {
        public DateTime Date { get; set; }
        public int RemainingTasks { get; set; }
        public int IdealTasks { get; set; }
    }
}
