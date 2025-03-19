namespace kurs.Models
{
    public class OrderStatus
    {
        public int StatusId { get; set; } // Уникальный идентификатор статуса заказа
        public string Status { get; set; } // Статус заказа (например, "Ожидается", "В обработке", "Доставлено", "Возвращено")
        public DateTime DateUpdated { get; set; } // Дата последнего обновления статуса
        public string TrackingNumber { get; set; } // Номер отслеживания (если применимо)

        public int OrderId { get; set; } // Идентификатор заказа
        public Order? Order { get; set; } // Связь с заказом

    }
   
}
