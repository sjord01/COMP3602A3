namespace COMP3602A3
{
    public class InvoiceDetail
    {
        public int Quantity { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Taxable { get; set; }
    }
}