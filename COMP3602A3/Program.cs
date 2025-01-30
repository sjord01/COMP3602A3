using System;
using System.IO;

namespace COMP3602A3
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: InvoiceApp <filename>");
                return;
            }

            var filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: File not found.");
                return;
            }

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    var processor = new ProcessData();
                    var invoices = processor.ParseInvoices(reader);

                    UiUtility.DisplayInvoices(invoices);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}