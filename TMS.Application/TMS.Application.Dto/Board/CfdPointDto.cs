using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Application.Dto.Board
{
    public class CfdPointDto
    {
        public DateTime Date { get; set; }
        public Dictionary<string, int> ColumnTaskCounts { get; set; } = [];
    }
}
