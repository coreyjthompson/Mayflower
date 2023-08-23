using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayflower.Core.Infrastructure.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMayflowerData(this IServiceCollection services, IConfiguration config)
        {
            if (config["ConnectionStrings:DefaultConnection"] == null)
            {
                throw new Exception("Mayflower database connection string not found at: ConnectionStrings:DefaultConnection");
            }

            return services.AddDbContextFactory<MayflowerContext>(options => options.UseSqlServer(config["ConnectionStrings:DefaultConnection"]));
        }
    }
}
