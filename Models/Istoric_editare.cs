namespace WikipediaDAW.Models
{
    public class Istoric_editare
    {
        public int Id { get; set; }
        public Articol Articol { get; set; }
        public DateTime Data_editarii { get; set; }
        public string Autor_editare { get; set; }
        public String Continut { get; set; }
    }
}
