namespace MindNote.Data.Providers.SqlServer.Models
{
    public class Relation
    {
        public int Id { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public Data.Relation ToModel()
        {
            return new Data.Relation
            {
                Id = Id,
                From = From,
                To = To,
            };
        }

        public static Relation FromModel(Data.Relation data)
        {
            return new Relation
            {
                Id = data.Id,
                From = data.From,
                To = data.To,
            };
        }
    }
}
