namespace CouchbaseDemo.Model
{
    public class OrderModel
    {
        public Guid Id { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public decimal OrderTotal { get; set; }

        public DateTime OrderCreateDate { get; set; }

        public long OrderIndex { get; set; }
    }
}
