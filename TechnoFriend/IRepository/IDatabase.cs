using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoFriend.IRepository
{
    public interface IDatabase
    {
        List<Define> Get();
    }

    public class Database : IDatabase
    {
        private DataClasses1DataContext _context;

        public Database()
        {
            _context = new DataClasses1DataContext();
        }
        public List<Define> Get()
        {
            return (from def in _context.Defines select def).ToList();
        }
    }
}
