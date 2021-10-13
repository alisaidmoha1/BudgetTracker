using Budget.Data;
using Budget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Services
{
    public class ExpenseCategoryService
    {
        private readonly Guid _userId;

        public ExpenseCategoryService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateExpenseCategroy(ExpenseCategoryCreate model)
        {
            var entity =
                new ExpenseCategory()
                {
                    UserId = _userId,
                    ExpenseCategoryName = model.ExpenseCategoryName
                };

             using (var ctx = new ApplicationDbContext())
            {
                ctx.ExpenseCategories.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<ExpenseCategoryListItem> GetExpenseCategories()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .ExpenseCategories
                        .Where(c => c.UserId == _userId)
                        .Select(
                            e =>
                                new ExpenseCategoryListItem
                                {
                                    ExpenseCategoryId = e.ExpenseCategoryId,
                                    ExpenseCategoryName = e.ExpenseCategoryName
                                }

                        );
                return query.ToArray();
            }
        }

        public bool UpdateExpenseCategory(ExpenseCategoryEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .ExpenseCategories
                        .Single(c => c.ExpenseCategoryId == model.ExpenseCategoryId && c.UserId == _userId);

                entity.ExpenseCategoryName = model.ExpenseCategoryName;

                return ctx.SaveChanges() == 1;
            }
        }

        public ExpenseCategoryEdit GetExpenseCategoryById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .ExpenseCategories
                        .Single(c => c.ExpenseCategoryId == id && c.UserId == _userId);
                return
                    new ExpenseCategoryEdit
                    {
                        ExpenseCategoryId = entity.ExpenseCategoryId,
                        ExpenseCategoryName = entity.ExpenseCategoryName
                    };
            }
        }

        public bool DeleteExpenseCategory(int catId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .ExpenseCategories
                        .Single(c => c.ExpenseCategoryId == catId && c.UserId == _userId);
                ctx.ExpenseCategories.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
