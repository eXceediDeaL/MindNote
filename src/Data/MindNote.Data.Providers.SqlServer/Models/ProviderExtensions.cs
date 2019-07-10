using MindNote.Data.Raws;
using System;
using System.Linq;

namespace MindNote.Data.Providers.SqlServer.Models
{
    public static class ProviderExtensions
    {
        public static Data.Category ToModel(this RawCategory data)
        {
            return new Data.Category
            {
                Id = data.Id,
                Name = data.Name,
                Color = data.Color,
                UserId = data.UserId,
                Status = data.Status,
            };
        }

        public static RawCategory FromModel(Data.Category data)
        {
            RawCategory res = new RawCategory
            {
                Id = data.Id,
                Name = data.Name,
                Color = data.Color,
                UserId = data.UserId,
                Status = data.Status,
            };
            return res;
        }

        public static Data.User ToModel(this RawUser data)
        {
            var res = new Data.User
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

        public static RawUser FromModel(Data.User data)
        {
            RawUser res = new RawUser
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

        public static string KeywordsToString(string[] keywords)
        {
            if (keywords == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Join(";", keywords.Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)));
            }
        }

        public static Data.Note ToModel(this RawNote data)
        {
            var res = new Data.Note
            {
                Id = data.Id,
                Title = data.Title,
                Content = data.Content,
                CategoryId = data.CategoryId,
                CreationTime = data.CreationTime,
                ModificationTime = data.ModificationTime,
                UserId = data.UserId,
                Status = data.Status,
            };
            if (data.Keywords == null)
            {
                res.Keywords = Array.Empty<string>();
            }
            else
            {
                res.Keywords = data.Keywords.Split(';').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }
            return res;
        }

        public static RawNote FromModel(Data.Note data)
        {
            RawNote res = new RawNote
            {
                Id = data.Id,
                Title = data.Title,
                Content = data.Content,
                CategoryId = data.CategoryId,
                CreationTime = data.CreationTime,
                ModificationTime = data.ModificationTime,
                UserId = data.UserId,
                Status = data.Status,
                Keywords = KeywordsToString(data.Keywords),
            };
            return res;
        }
    }
}
