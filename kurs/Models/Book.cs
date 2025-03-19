namespace kurs.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int CountBooks { get; set; }
        public string Description { get; set; }

        //Внешний ключ для связи с автором
        public int AuthorId { get; set; }
        public Author? Authors { get; set; }  // Связь с автором

        //внешний ключ для клиентов
        //public int CustomerId { get; set; }
        //public Customer? Customers { get; set; }
    }
}
