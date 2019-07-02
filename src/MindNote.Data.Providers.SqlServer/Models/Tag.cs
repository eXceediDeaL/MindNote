namespace MindNote.Data.Providers.SqlServer.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public string UserId { get; set; }

        public Data.Tag ToModel()
        {
            return new Data.Tag
            {
                Id = Id,
                Name = Name,
                Color = Color,
            };
        }

        public static Tag FromModel(Data.Tag data)
        {
            Tag res = new Tag
            {
                Id = data.Id,
                Name = data.Name,
                Color = data.Color,
            };
            return res;
        }
    }
}
