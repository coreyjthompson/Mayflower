using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayflower.Core.Infrastructure.Queries
{
    public class PageInfo
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; } = 20;
    }
}
