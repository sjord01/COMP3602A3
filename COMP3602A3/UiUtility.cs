using System;
using System.Collections.Generic;

namespace COMP3602A3
{
    public class UiUtility
    {
        private const decimal GstRate = 0.05m;
        private const decimal PstRate = 0.07m;
        private const int LineWidth = 87;

        public static void DisplayInvoices(List<Invoice> invoices)
        {
            Console.Title = "COMP3602 - Assignment3 - A01183994";
            Console.WriteLine("Invoice Listing");
            Console.WriteLine(new string('-', LineWidth));

            foreach (var invoice in invoices)
            {
                DisplayInvoiceHeader(invoice);
                DisplayInvoiceDetails(invoice);
            }
        }

        private static void DisplayInvoiceHeader(Invoice invoice)
        {
            (int leftTitleWidth, int rightTitleWidth, int leftValueWidth, int rightValueWidth) =
                GetHeaderColumnWidths(invoice);

            PrintHeaderRow("Invoice Number:", invoice.InvoiceNumber, "Terms:", ParseTerms(invoice.Terms), leftTitleWidth, leftValueWidth, rightTitleWidth);
            PrintHeaderRow("Invoice Date:", invoice.InvoiceDate.ToString("MMM dd, yyyy"), "Ship Via:", invoice.ShipBy, leftTitleWidth, leftValueWidth, rightTitleWidth);
            PrintHeaderRow("Order Number:", invoice.OrderNumber, "Attn:", invoice.Attn, leftTitleWidth, leftValueWidth, rightTitleWidth);
            
            Console.WriteLine(new string('-', LineWidth));
        }

        private static void PrintHeaderRow(string leftTitle, string leftValue, string rightTitle, string rightValue, int leftTitleWidth, int leftValueWidth, int rightTitleWidth)
        {
            Console.WriteLine($"{FormatAligned(leftTitle, leftTitleWidth)}{FormatAligned(leftValue, leftValueWidth)}" +
                              $"{FormatAligned(rightTitle, rightTitleWidth)}{rightValue}");
        }

        private static void DisplayInvoiceDetails(Invoice invoice)
        {
            (decimal subtotal, decimal pstTotal, decimal gst, decimal total) = CalculateTotals(invoice);

            PrintInvoiceTableHeader();

            foreach (var detail in invoice.Details)
            {
                PrintInvoiceLine(detail);
            }

            PrintInvoiceSummary(subtotal, pstTotal, gst, total, invoice);
        }

        private static void PrintInvoiceTableHeader()
        {
            Console.WriteLine("Qty".PadRight(4) + "SKU".PadRight(16) + "Description".PadRight(42) + "Price".PadRight(9) + "PST".PadRight(3) + "Ext".PadLeft(13));
            Console.WriteLine(new string('-', LineWidth));
        }

        private static void PrintInvoiceLine(InvoiceDetail detail)
        {
            decimal lineTotal = detail.Quantity * detail.Price;
            string pstFlag = detail.Taxable ? "Y" : "N";

            Console.WriteLine($"{detail.Quantity,-4}{detail.Sku,-16}{detail.Description,-42}{detail.Price,9:N2}{pstFlag,3}{lineTotal,13:N2}");
        }

        private static void PrintInvoiceSummary(decimal subtotal, decimal pstTotal, decimal gst, decimal total, Invoice invoice)
        {
            Console.WriteLine(new string('-', LineWidth));
            Console.WriteLine($"{"Subtotal:",29}{subtotal,58:N2}");
            Console.WriteLine($"{"GST [5.0%]:",31} {gst,55:N2}");

            if (pstTotal > 0)
            {
                Console.WriteLine($"{"PST [7.0%]:",31} {pstTotal,55:N2}");
            }

            Console.WriteLine(new string('-', LineWidth));
            Console.WriteLine($"{"Total:",26} {total,60:N2}");

            PrintDiscount(total, invoice);
        }

        private static void PrintDiscount(decimal total, Invoice invoice)
        {
            DateTime discountDate = invoice.InvoiceDate.AddDays(GetDiscountDays(invoice.Terms));
            decimal discount = Math.Round(total * GetDiscountPercentage(invoice.Terms), 2, MidpointRounding.AwayFromZero);

            Console.WriteLine();
            Console.WriteLine($"{"Discount:  [If paid by: " + discountDate.ToString("MMM dd, yyyy") + "]",57} {discount,29:N2}");
            Console.WriteLine();
        }

        private static (decimal subtotal, decimal pstTotal, decimal gst, decimal total) CalculateTotals(Invoice invoice)
        {
            decimal subtotal = 0;
            decimal pstTotal = 0;

            foreach (var detail in invoice.Details)
            {
                decimal lineTotal = detail.Quantity * detail.Price;
                subtotal += lineTotal;

                if (detail.Taxable)
                {
                    pstTotal += Math.Round(lineTotal * PstRate, 2, MidpointRounding.AwayFromZero);
                }
            }

            subtotal = Math.Round(subtotal, 2, MidpointRounding.AwayFromZero);
            decimal gst = Math.Round(subtotal * GstRate, 2, MidpointRounding.AwayFromZero);
            pstTotal = Math.Round(pstTotal, 2, MidpointRounding.AwayFromZero);
            decimal total = subtotal + gst + pstTotal;

            return (subtotal, pstTotal, gst, total);
        }

        private static (int, int, int, int) GetHeaderColumnWidths(Invoice invoice)
        {
            int leftTitleWidth = Math.Max("Invoice Number".Length,
                Math.Max("Invoice Date".Length, "Order Number".Length)) + 6;
            int rightTitleWidth = Math.Max("Terms".Length,
                Math.Max("Ship Via".Length, "Attn".Length)) + 6;
            int leftValueWidth = Math.Max(invoice.InvoiceNumber.Length,
                Math.Max(invoice.InvoiceDate.ToString("MMM dd, yyyy").Length, invoice.OrderNumber.Length)) + 20;
            int rightValueWidth = Math.Max(ParseTerms(invoice.Terms).Length,
                Math.Max(invoice.ShipBy.Length, invoice.Attn.Length)) + 8;

            return (leftTitleWidth, rightTitleWidth, leftValueWidth, rightValueWidth);
        }

        private static string FormatAligned(string text, int width)
        {
            return text.PadRight(width);
        }

        private static int GetDiscountDays(string terms)
        {
            return int.Parse(terms.Substring(2, 2));
        }

        private static decimal GetDiscountPercentage(string terms)
        {
            return int.Parse(terms.Substring(0, 2)) / 1000.0m;
        }

        private static string ParseTerms(string terms)
        {
            int discount = int.Parse(terms.Substring(0, 2));
            int days = int.Parse(terms.Substring(2, 2));
            return $"{discount / 10.0:F1}% {days} days ADI";
        }
    }
}
