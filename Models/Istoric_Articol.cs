namespace WikipediaDAW.Models
{
    public class Istoric_Articol
    {

        public int Id { get; set; }
        
        public Articol articol { get; set; }

        public string continut_vechi { get; set; }

        public DateTime data_editarii { get; set; }

    }
}
