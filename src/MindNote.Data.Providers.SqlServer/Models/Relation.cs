namespace MindNote.Data.Providers.SqlServer.Models
{
    public class Relation
    {
        public int Id { get; set; }

        public string Nodes { get; set; }

        public string Color { get; set; }

        public bool IsSelected { get; set; }

        public Data.Relation ToModel()
        {
            return new Data.Relation
            {
                Id = Id,
                Nodes = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(Nodes),
                Color = Color,
                IsSelected = IsSelected,
            };
        }

        public static Relation FromModel(Data.Relation data)
        {
            return new Relation
            {
                Id = data.Id,
                Nodes = Newtonsoft.Json.JsonConvert.SerializeObject(data.Nodes),
                Color = data.Color,
                IsSelected = data.IsSelected,
            };
        }
    }
}
