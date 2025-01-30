using System;
using System.Collections.Generic;

namespace COMP3602A3
{
    public class Invoice
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Terms { get; set; }
        public string OrderNumber { get; set; }
        public string ShipBy { get; set; }
        public string Attn { get; set; }
        public List<InvoiceDetail> Details { get; set; } = new List<InvoiceDetail>();
    }
}