using Budget.Data;
using Budget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Services
{
    public class CategoryService
    {
        private readonly Guid _userId;

        public CategoryService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateCategroy(CategoryCreate model)
        {
            var entity =
                new Category()
                {
                    UserId = _userId,
                    CategoryName = model.CategoryName
                };

             using (var ctx = new ApplicationDbContext())
            {
                ctx.Categories.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<CategoryListItem> GetCategories()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Categories
                        .Where(c => c.UserId == _userId)
                        .Select(
                            e =>
                                new CategoryListItem
                                {
                                    CategoryId = e.CategoryId,
                                    CategoryName = e.CategoryName
                                }

                        );
                return query.ToArray();
            }
        }

        public bool UpdateCategory(CategoryEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Categories
                        .Single(c => c.CategoryId == model.CategoryId && c.UserId == _userId);

                entity.CategoryName = model.CategoryName;

                return ctx.SaveChanges() == 1;
            }
        }

        public CategoryEdit GetCategoryById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Categories
                        .Single(c => c.CategoryId == id && c.UserId == _userId);
                return
                    new CategoryEdit
                    {
                        CategoryId = entity.CategoryId,
                        CategoryName = entity.CategoryName
                    };
            }
        }

        public bool DeleteCategory(int catId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Categories
                        .Single(c => c.CategoryId == catId && c.UserId == _userId);
                ctx.Categories.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
