namespace MindNote.Data.Providers.SqlServer.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public string UserId { get; set; }

        public ItemStatus Status { get; set; }

        public Data.Category ToModel()
        {
            return new Data.Category
            {
                Id = Id,
                Name = Name,
                Color = Color,
                UserId = UserId,
                Status = Status,
            };
        }

        public static Category FromModel(Data.Category data)
        {
            Category res = new Category
            {
                Id = data.Id,
                Name = data.Name,
                Color = data.Color,
                UserId = data.UserId,
                Status = data.Status,
            };
            return res;
        }
    }
}
