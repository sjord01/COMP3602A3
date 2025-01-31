using System;
using System.Collections.Generic;

namespace COMP3602A3
{
    public class UiUtility
    {
        public static void DisplayInvoices(List<Invoice> invoices)
        {
            Console.Title = "COMP3602 - Assignment3 - A01183994";
            Console.WriteLine("Invoice Listing");
            Console.WriteLine(new string('-', Config.LineWidth));

            foreach (var invoice in invoices)
            {
                DisplayInvoiceHeader(invoice);
                DisplayInvoiceDetails(invoice);
            }
        }

        private static void DisplayInvoiceHeader(Invoice invoice)
        {
            Console.WriteLine($"{"Invoice Number:",-20}{invoice.InvoiceNumber,-30}{"Terms:",-15}{ParseTerms(invoice.Terms)}");
            Console.WriteLine($"{"Invoice Date:",-20}{invoice.InvoiceDate.ToString("MMM dd, yyyy"),-30}{"Ship Via:",-15}{invoice.ShipBy}");
            Console.WriteLine($"{"Order Number:",-20}{invoice.OrderNumber,-30}{"Attn:",-15}{invoice.Attn}");
            Console.WriteLine(new string('-', Config.LineWidth));
        }

        private static void DisplayInvoiceDetails(Invoice invoice)
        {
            var (subtotal, pstTotal, gst, total) = InvoiceCalculator.CalculateTotals(invoice);

            // Table header
            Console.WriteLine($"{ "Qty",-4}{ "SKU",-16}{ "Description",-42}{ "Price",8}{ "PST",4}{ "Ext",13}");
            Console.WriteLine(new string('-', Config.LineWidth));

            // Invoice lines
            foreach (var detail in invoice.Details)
            {
                decimal lineTotal = detail.Quantity * detail.Price;
                string pstFlag = detail.Taxable ? "Y" : "N";

                Console.WriteLine($"{detail.Quantity,-4}{detail.Sku,-16}{detail.Description,-42}{detail.Price,8:N2}{pstFlag,3}{lineTotal,14:N2}");
            }

            // Invoice summary
            Console.WriteLine(new string('-', Config.LineWidth));
            Console.WriteLine($"{"Subtotal:",29}{subtotal,58:N2}");
            Console.WriteLine($"{"GST [5.0%]:",31} {gst,55:N2}");

            if (pstTotal > 0)
            {
                Console.WriteLine($"{"PST [7.0%]:",31} {pstTotal,55:N2}");
            }

            Console.WriteLine(new string('-', Config.LineWidth));
            Console.WriteLine($"{"Total:",26} {total,60:N2}");

            PrintDiscount(total, invoice);
        }

        private static void PrintDiscount(decimal total, Invoice invoice)
        {
            DateTime discountDate = invoice.InvoiceDate.AddDays(InvoiceCalculator.GetDiscountDays(invoice.Terms));
            decimal discount = InvoiceCalculator.CalculateDiscount(total, invoice.Terms);

            Console.WriteLine();
            Console.WriteLine($"{"Discount:  [If paid by: " + discountDate.ToString("MMM dd, yyyy") + "]",57} {discount,29:N2}");
            Console.WriteLine();
        }

        private static string ParseTerms(string terms)
        {
            int discount = int.Parse(terms.Substring(0, 2));
            int days = int.Parse(terms.Substring(2, 2));
            return $"{discount / 10.0:F1}% {days} days ADI";
        }
    }
}