using CsvHelper;
using System.Globalization;

namespace Mayflower.Web.Data
{
    public class TransactionService
    {
        private const string BASEPATH = @"C:\Users\cthompson\Downloads\transactions.csv";

        private static readonly int[] Accounts = new[]
        {
            1032497347, 1054452618
        };

        public Task<List<Dto.Transaction>> GetTransactionsAsync()
        {
            return Task.FromResult(new List<Dto.Transaction>());
        }

        public List<Dto.Transaction> GetTransactions()
        {
            using StreamReader reader = new StreamReader(BASEPATH);
            using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<Dto.Transaction>().ToList();

            return records;
        }

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