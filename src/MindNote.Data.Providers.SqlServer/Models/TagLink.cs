using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.SqlServer.Models
{
    public enum TagLinkClase
    {
        Node,
        Struct
    }

    public class TagLink
    {
        public int Id { get; set; }

        public int ObjectId { get; set; }

        public int TagId { get; set; }

        public TagLinkClase Class { get; set; }

        public static async Task SetTagLink(int id, IEnumerable<int> data, TagLinkClase type, DataContext context)
        {
            var query = from r in context.TagLinks where r.ObjectId == id && r.Class == type select r;
            context.TagLinks.RemoveRange(query);
            foreach (var v in data)
            {
                var raw = new TagLink
                {
                    Class = type,
                    ObjectId = id,
                    TagId = v,
                };
                context.TagLinks.Add(raw);
            }
            await context.SaveChangesAsync();
        }

        public static async Task<IEnumerable<Tag>> GetTagLink(int id, TagLinkClase type, DataContext context)
        {
            var query = from r in context.TagLinks where r.ObjectId == id && r.Class == type select r.TagId;
            var res = new List<Tag>();
            foreach (var v in query)
            {
                var t = await context.Tags.FindAsync(v);
                if (t != null)
                    res.Add(t);
            }
            return res;
        }
    }
}
