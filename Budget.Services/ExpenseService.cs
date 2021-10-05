using Budget.Data;
using Budget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Services
{
    public class ExpenseService
    {
        private readonly Guid _userId;

        public ExpenseService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateExpense(ExpenseCreate model)
        {
            var entity =
                new Expense()
                {
                    UserId = _userId,
                    CategoryId = model.CategoryId,
                    CreatedUtc = DateTimeOffset.UtcNow,
                    Amount = model.Amount,
                    IsRepeat = model.IsRepeat,
                    Note = model.Note
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Expenses.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<ExpenseListItem> GetExpense()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Expenses
                        .Where(c => c.UserId == _userId)
                        .Select(
                            e =>
                                new ExpenseListItem
                                {
                                    ExpenseId = e.ExpenseId,
                                    CategoryName = e.Category.CategoryName,
                                    CreatedUtc = e.CreatedUtc,
                                    Amount = e.Amount
                                }

                        );
                return query.ToArray();
            }
        }
    }
}
