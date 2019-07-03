namespace MindNote.Data.Providers.SqlServer.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public string UserId { get; set; }

        public Data.Category ToModel()
        {
            return new Data.Category
            {
                Id = Id,
                Name = Name,
                Color = Color,
            };
        }

        public static Category FromModel(Data.Category data)
        {
            Category res = new Category
            {
                Id = data.Id,
                Name = data.Name,
                Color = data.Color,
            };
            return res;
        }
    }
}
