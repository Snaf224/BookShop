namespace kurs.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Fio { get; set; }
        public string Bio { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
