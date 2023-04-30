using CsvHelper;
using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Commands;
using Mayflower.Core.Infrastructure.Queries;
using Mayflower.Core.Infrastructure.Queries.Reminders;
using System.Globalization;

namespace Mayflower.Web.Data
{
    //public class ReminderService
    //{
    //    private readonly IQueryProcessor _queries;
    //    private readonly ICommandProcessor _commands;

    //    public ReminderService(IQueryProcessor queries) 
    //    {
    //        _queries = queries;
    //    }


    //    public async Task<List<Reminder>> GetRemindersByAccountIdAsync(int id)
    //    {
            
    //        var query = new GetAllRemindersByFinancialAccountIdQuery
    //        {
    //            Id = id
    //        };

    //        var test = await _queries.Execute(query);

    //        return test;

    //        //return await Task.FromResult(new List<Reminder>());

    //    }

    //}
}