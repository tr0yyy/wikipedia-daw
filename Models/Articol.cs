namespace WikipediaDAW.Models
{
    public class Articol
    {
        public int Id { get; set; }
        public string Domeniu { get; set; }
        public string Titlu { get; set; }
        public User Autor { get; set; }
        public DateTime Data_adaugarii { get; set; }
        public string Continut { get; set; }
        public bool Protejat { get; set; }

       

    }
}
