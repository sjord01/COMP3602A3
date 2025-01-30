using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace COMP3602A3
{
    public class ProcessData
    {

        public List<Invoice> ParseInvoices(TextReader reader)
        {
            var invoices = new List<Invoice>();

            string line;
            while ((line = reader.ReadLine()) != null) invoices.Add(ParseInvoice(line));

            return invoices;
        }

        private Invoice ParseInvoice(string line)
        {
            var parts = line.Split('|');
            var headerParts = parts[0].Split(':');

            var invoice = new Invoice
            {
                InvoiceNumber = headerParts[0],
                InvoiceDate = DateTime.ParseExact(headerParts[1], "yyyy/MM/dd", CultureInfo.InvariantCulture),
                Terms = headerParts[2],
                OrderNumber = headerParts[3],
                ShipBy = headerParts[4],
                Attn = headerParts[5]
            };

            for (var i = 1; i < parts.Length; i++)
            {
                var detailParts = parts[i].Split(':');
                invoice.Details.Add(new InvoiceDetail
                {
                    Quantity = int.Parse(detailParts[0]),
                    Sku = detailParts[1],
                    Description = detailParts[2],
                    Price = decimal.Parse(detailParts[3]),
                    Taxable = detailParts[4] == "1"
                });
            }

            return invoice;
        }
    }
}