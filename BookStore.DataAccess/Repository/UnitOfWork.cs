using BookStore.Data;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; set; }
        public IProductRepository Product { get; set; }
        public ICompanyRepository Company { get; set; }
        public IApplicationUserRepository ApplicationUser { get; set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
          Category = new CategoryRepository(_db);
          Product = new ProductRepository(_db);
          Company= new CompanyRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
        }
        
        public void save()
        {
           _db.SaveChanges();
        }
    }
}
