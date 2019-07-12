using Microsoft.EntityFrameworkCore;
using MindNote.Data.Providers.SqlServer.Models;
using MindNote.Data.Repositories;
using System.Text;

namespace MindNote.Data.Providers.SqlServer
{
    public class DataRepository : IDataRepository
    {
        private DataContext dataContext;

        public DataRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            Notes = new NoteRepository(dataContext, this);
            Categories = new CategoryRepository(dataContext, this);
            Users = new UserRepository(dataContext, this);
        }

        public INoteRepository Notes { get; }

        public ICategoryRepository Categories { get; }

        public IUserRepository Users { get; }
    }
}
