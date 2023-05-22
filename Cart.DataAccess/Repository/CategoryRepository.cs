using Cart.DataAccess.Data;
using Cart.DataAccess.Repository.IRepository;
using Cart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cart.DataAccess.Repository
{
    internal class CategoryRepository : Repository<Category>, ICategoryRepository 
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
       

        public void Update(Category obj)
        {
            _db.Catego.Update(obj);
        }
    }
}
