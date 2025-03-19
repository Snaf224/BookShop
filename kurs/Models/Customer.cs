namespace kurs.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<Order> Orders { get; set; }
        //public string ProfilePicture { get; set; }
    }
}
