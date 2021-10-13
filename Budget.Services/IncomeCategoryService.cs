using Budget.Data;
using Budget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Services
{
    public class IncomeCategoryService
    {
        private readonly Guid _userId;

        public IncomeCategoryService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateIncomeCategroy(IncomeCategoryCreate model)
        {
            var entity =
                new IncomeCategory()
                {
                    UserId = _userId,
                    IncomeCategoryName = model.IncomeCategoryName
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.IncomeCategories.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<IncomeCategoryListItem> GetIncomeCategories()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .IncomeCategories
                        .Where(c => c.UserId == _userId)
                        .Select(
                            e =>
                                new IncomeCategoryListItem
                                {
                                    IncomeCategoryId = e.IncomeCategoryId,
                                    IncomeCategoryName = e.IncomeCategoryName
                                }

                        );
                return query.ToArray();
            }
        }

        public bool UpdateIncomeCategory(IncomeCategoryEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .IncomeCategories
                        .Single(c => c.IncomeCategoryId == model.IncomeCategoryId && c.UserId == _userId);

                entity.IncomeCategoryName = model.IncomeCategoryName;

                return ctx.SaveChanges() == 1;
            }
        }

        public IncomeCategoryEdit GetIncomeCategoryById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .IncomeCategories
                        .Single(c => c.IncomeCategoryId == id && c.UserId == _userId);
                return
                    new IncomeCategoryEdit
                    {
                        IncomeCategoryId = entity.IncomeCategoryId,
                        IncomeCategoryName = entity.IncomeCategoryName
                    };
            }
        }

        public bool DeleteIncomeCategory(int catId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .IncomeCategories
                        .Single(c => c.IncomeCategoryId == catId && c.UserId == _userId);
                ctx.IncomeCategories.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
