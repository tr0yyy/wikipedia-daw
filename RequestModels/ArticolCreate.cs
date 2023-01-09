using WikipediaDAW.Models;

namespace WikipediaDAW.RequestModels
{
    public class ArticolCreate
    {
        public string? Domeniu { get; set; }

        public string Titlu { get; set; }

        public string Continut { get; set; }

        public string? User { get; set; }

        public bool Protejat { get; set; }

        public DateTime Data_adaugarii { get; set; }
        
        public ArticolCreate(string domeniu ,string titlu, string continut, string user, bool protejat, DateTime data_adaugarii) {
            Domeniu = domeniu;
            Titlu= titlu;
            Continut = continut;
            User = user;
            Protejat = protejat;
            Data_adaugarii = data_adaugarii;
        }
    }
}
