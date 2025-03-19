namespace kurs.Models
{
    public class Order
    {
        public int OrderId {  get; set; }
        public string OrderName { get; set; }
        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; }

        public int CustomerId { get; set; } // Внешний ключ покупателя
        public Customer Customer { get; set; } // Связь с покупателем

        public ICollection<Book> Books { get; set; } // Связь с книгами (через промежуточную таблицу)
        public ICollection<OrderStatus> OrderStatuses { get; set; } // Статусы заказа
    }
}
