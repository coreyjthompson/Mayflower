using CsvHelper;
using Mayflower.Web.DTO;
using System.Globalization;

namespace Mayflower.Web.Data
{
    public class AllyBankService
    {
        private const string BASEPATH = @"C:\Users\cthompson\Downloads\transactions.csv";

        private static readonly int[] Accounts = new[]
        {
            1032497347, 1054452618
        };

        public Task<List<Transaction>> GetTransactionsAsync()
        {
            return Task.FromResult(new List<Transaction>());
        }

        public List<Transaction> GetTransactions()
        {
            //using (var reader = new StreamReader(BASEPATH))
            //{
            //    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            //    {
            //        var records = csv.GetRecords<Transaction>().ToList();

            //        return records;
            //    }
            //}

            using StreamReader reader = new StreamReader(BASEPATH);
            using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<Transaction>().ToList();

            return records;
        }

        //public List<Transaction> GetTransactionsByAccountNumbers(IList<string> accounts)
        //{
        //    using (var reader = new StreamReader("path\\to\\file.csv"))
        //    {
        //        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        //        {
        //            var records = csv.GetRecords<Transaction>().ToListAsync();
        //            return records;
        //        }
        //    }

        //    return await records;
        //}

        //public async Task<List<Transaction>> GetTransactionsByAccountNumbersAsync(int[] accounts)
        //{
        //    using (var reader = new StreamReader("path\\to\\file.csv"))
        //    {
        //        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        //        {
        //            var records = csv.GetRecords<Transaction>().ToListAsync();
        //            return records;
        //        }
        //    }

        //    return await records;
        //}

        public void GetTransactionData()
        {
            string filePath = @"C:\Users\cthompson\Downloads\ally-transactions.csv";

            StreamReader? reader = null;
            if (File.Exists(filePath))
            {
                reader = new StreamReader(File.OpenRead(filePath));
                List<string> listA = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line?.Split(',');
                    if(values != null)
                    {
                        foreach (var item in values)
                        {
                            listA.Add(item);
                        }
                        foreach (var coloumn1 in listA)
                        {
                            System.Diagnostics.Debug.WriteLine(coloumn1);
                        }
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("File doesn't exist");
                //throw new FileNotFoundException(filePath + " does not exist");
                
            }
             
        }
    }
}