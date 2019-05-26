using System.Collections.Generic;
using System.Threading.Tasks;
using MindNote.Data.Providers.SqlServer.Models;

namespace MindNote.Data.Providers.SqlServer
{
    class TagsProvider : ITagsProvider
    {
        DataContext context;

        public TagsProvider(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> Create(Tag data)
        {
            var raw = Models.Tag.FromModel(data);
            context.Tags.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task Delete(int id)
        {
            Models.Tag item = await context.Tags.FindAsync(id);
            if (item != null)
            {
                context.Tags.Remove(item);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Tag> Get(int id)
        {
            return (await context.Tags.FindAsync(id)).ToModel();
        }

        public Task<IEnumerable<Tag>> GetAll()
        {
            List<Tag> res = new List<Tag>();
            foreach (var v in context.Tags)
            {
                var item = v.ToModel();
                res.Add(item);
            }
            return Task.FromResult<IEnumerable<Tag>>(res);
        }

        public async Task<int> Update(int id, Tag data)
        {
            var item = await context.Tags.FindAsync(id);
            if (item != null)
            {
                var td = Models.Tag.FromModel(data);

                item.Name = td.Name;
                item.Color = td.Color;

                context.Tags.Update(item);
                await context.SaveChangesAsync();
                return data.Id;
            }
            return -1;
        }
    }
}
