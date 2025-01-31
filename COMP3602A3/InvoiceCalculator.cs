using System;

namespace COMP3602A3
{
    public static class InvoiceCalculator
    {
        public static (decimal subtotal, decimal pstTotal, decimal gst, decimal total) CalculateTotals(Invoice invoice)
        {
            decimal subtotal = 0;
            decimal pstTotal = 0;

            foreach (var detail in invoice.Details)
            {
                decimal lineTotal = detail.Quantity * detail.Price;
                subtotal += lineTotal;

                if (detail.Taxable)
                {
                    pstTotal += Math.Round(lineTotal * Config.PstRate, 2, MidpointRounding.AwayFromZero);
                }
            }

            subtotal = Math.Round(subtotal, 2, MidpointRounding.AwayFromZero);
            decimal gst = Math.Round(subtotal * Config.GstRate, 2, MidpointRounding.AwayFromZero);
            pstTotal = Math.Round(pstTotal, 2, MidpointRounding.AwayFromZero);
            decimal total = subtotal + gst + pstTotal;

            return (subtotal, pstTotal, gst, total);
        }

        public static decimal CalculateDiscount(decimal total, string terms)
        {
            return Math.Round(total * GetDiscountPercentage(terms), 2, MidpointRounding.AwayFromZero);
        }

        public static int GetDiscountDays(string terms)
        {
            return int.Parse(terms.Substring(2, 2));
        }

        private static decimal GetDiscountPercentage(string terms)
        {
            return int.Parse(terms.Substring(0, 2)) / 1000.0m;
        }
    }
}