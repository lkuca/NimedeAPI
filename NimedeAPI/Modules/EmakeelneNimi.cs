namespace NimedeAPI.Modules
{
    public class EmakeelneNimi
    {
        public int EmakeelneNimiId { get; set; }
        public int NimiId { get; set; }
        public string emakeelneNimi { get; set; }
        public string keel { get; set; }
        public DateTime DateCreated { get; set; }

        public Nimi nimi { get; set; }
    }
}
