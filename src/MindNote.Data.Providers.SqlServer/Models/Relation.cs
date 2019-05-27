namespace MindNote.Data.Providers.SqlServer.Models
{
    public class Relation
    {
        public int Id { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public string Color { get; set; }

        public string Extra { get; set; }

        public Data.Relation ToModel()
        {
            return new Data.Relation
            {
                Id = Id,
                Color = Color,
                From = From,
                To = To,
                Extra = Extra,
            };
        }

        public static Relation FromModel(Data.Relation data)
        {
            return new Relation
            {
                Id = data.Id,
                From = data.From,
                To = data.To,
                Color = data.Color,
                Extra = data.Extra,
            };
        }
    }
}
