namespace MindNote.Data.Providers.SqlServer.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public string Extra { get; set; }

        public Data.Tag ToModel()
        {
            return new Data.Tag
            {
                Id = Id,
                Name = Name,
                Color = Color,
                Extra = Extra,
            };
        }

        public static Tag FromModel(Data.Tag data)
        {
            return new Tag
            {
                Id = data.Id,
                Name = data.Name,
                Color = data.Color,
                Extra = data.Extra,
            };
        }
    }
}
