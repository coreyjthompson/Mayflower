using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mayflower.Core.Infrastructure.Queries
{
    [DataContract(Name = nameof(Paged<T>) + "Of{0}")]
    public class Paged<T>
    {
        [DataMember] public PageInfo Paging { get; set; } = new PageInfo();

        [DataMember] public T[] Items { get; set; } = new T[0];
    }
}
