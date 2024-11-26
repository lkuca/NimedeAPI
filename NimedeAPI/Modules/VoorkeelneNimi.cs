namespace NimedeAPI.Modules
{
    public class VoorkeelneNimi
    {
        public int VoorkeelneNimiId { get; set; }
        public int NimiId { get; set; }
        public string voorkeelneNimi { get; set; }
        public string keel { get; set; }
        public DateTime DateCreated { get; set; }

        public Nimi nimi { get; set; }
    }
}
