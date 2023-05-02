using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayflower.Core.Infrastructure.Interfaces.Queries
{
    public interface IQueryProcessor
    {
        Task<TResult> Execute<TResult>(IQuery<TResult> query);
    }
}
