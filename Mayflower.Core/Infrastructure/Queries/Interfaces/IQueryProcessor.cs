using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mayflower.Core.Infrastructure.Queries.Interfaces;

namespace Mayflower.Core.Infrastructure.Queries
{
    public interface IQueryProcessor
    {
        Task<TResult> Execute<TResult>(IQuery<TResult> query);
    }
}
