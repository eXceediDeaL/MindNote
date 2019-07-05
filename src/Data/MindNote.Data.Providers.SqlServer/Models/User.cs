namespace MindNote.Data.Providers.SqlServer.Models
{
    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string Url { get; set; }

        public string Company { get; set; }

        public string Location { get; set; }

        public Data.User ToModel()
        {
            var res = new Data.User
            {
                Id = Id,
                Name = Name,
                Email = Email,
                Bio = Bio,
                Url = Url,
                Company = Company,
                Location = Location,
            };
            return res;
        }

        public static User FromModel(Data.User data)
        {
            User res = new User
            {
                Id = data.Id,
                Name = data.Name,
                Email = data.Email,
                Bio = data.Bio,
                Url = data.Url,
                Company = data.Company,
                Location = data.Location,
            };
            return res;
        }
    }
}
