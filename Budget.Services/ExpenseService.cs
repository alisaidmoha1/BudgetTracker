using Budget.Data;
using Budget.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                    ExpenseCategoryId = model.ExpenseCategoryId,
                    CreatedUtc = model.CreatedUtc,
                    Amount = model.Amount,
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
                                {   ExpenseId = e.ExpenseId,
                                    ExpenseCategoryName = e.Category.ExpenseCategoryName,
                                    CreatedUtc = e.CreatedUtc,
                                    Amount = e.Amount,
                                    Note = e.Note
                                }

                        ); ;
                return query.ToArray();
            }
        }

        public bool UpdateExpense(ExpenseEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Expenses
                        .Single(c => c.ExpenseId == model.ExpenseId && c.UserId == _userId);

                entity.ExpenseCategoryId = model.ExpenseCategoryId;
                entity.CreatedUtc = model.CreatedUtc;
                entity.Amount = model.Amount;
                entity.Note = model.Note;

                return ctx.SaveChanges() == 1;
            }
        }

        public ExpenseEdit GetExpenseById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Expenses
                        .Single(c => c.ExpenseId == id && c.UserId == _userId);
                return
                    new ExpenseEdit
                    {
                        ExpenseCategoryId = entity.ExpenseCategoryId,
                        ExpenseId = entity.ExpenseId,
                        CreatedUtc = entity.CreatedUtc,
                        Amount = entity.Amount,
                        Note = entity.Note
                    };
            }
        }

        public Dictionary<string, decimal?> CalculateMonthlyExpense()
        {
            using (var ctx = new ApplicationDbContext())
            {

                Dictionary<string, decimal?> dictMonthySum = new Dictionary<string, decimal?>();


                decimal? grocerySum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Grocery" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -7))).Select(cat => cat.Amount).Sum();

                decimal? shoppingSum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Shopping" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -7))).Select(cat => cat.Amount).Sum();

                decimal? travelSum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Travel" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -7))).Select(cat => cat.Amount).Sum();

                decimal? healthSum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Health" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -7))).Select(cat => cat.Amount).Sum();

                decimal? rentSum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Rent" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -7))).Select(cat => cat.Amount).Sum();

                decimal? utilitiesSum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Utilities" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -7))).Select(cat => cat.Amount).Sum();


                dictMonthySum.Add("Grocery", grocerySum);
                dictMonthySum.Add("Shopping", shoppingSum);
                dictMonthySum.Add("Travel", travelSum);
                dictMonthySum.Add("Rent", rentSum);
                dictMonthySum.Add("Health", healthSum);
                dictMonthySum.Add("Utilities", utilitiesSum);

                return dictMonthySum;

            }
        }

        public Dictionary<string, decimal?> CalculateWeeklyExpense()
        {
            using (var ctx = new ApplicationDbContext())
            {

                Dictionary<string, decimal?> dictWeeklySum = new Dictionary<string, decimal?>();

                decimal? grocerySum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Grocery" && (cat.CreatedUtc > DbFunctions.AddDays(DateTime.Now, -28)))
                    .Select(cat => cat.Amount)
                    .Sum();

                decimal? shoppingSum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Shopping" && (cat.CreatedUtc > DbFunctions.AddDays(DateTime.Now, -28)))
                   .Select(cat => cat.Amount)
                   .Sum();

                decimal? travelSum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Travel" && (cat.CreatedUtc > DbFunctions.AddDays(DateTime.Now, -28)))
                   .Select(cat => cat.Amount)
                   .Sum();

                decimal? healthSum = ctx.Expenses.Where
                   (cat => cat.Category.ExpenseCategoryName == "Health" && (cat.CreatedUtc > DbFunctions.AddDays(DateTime.Now, -28)))
                   .Select(cat => cat.Amount)
                   .Sum();

                decimal? rentSum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Rent" && (cat.CreatedUtc > DbFunctions.AddDays(DateTime.Now, -28)))
                   .Select(cat => cat.Amount)
                   .Sum();

                decimal? utilitiesSum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Utilities" && (cat.CreatedUtc > DbFunctions.AddDays(DateTime.Now, -28)))
                   .Select(cat => cat.Amount)
                   .Sum();

                dictWeeklySum.Add("Grocery", grocerySum);
                dictWeeklySum.Add("Shopping", shoppingSum);
                dictWeeklySum.Add("Travel", travelSum);
                dictWeeklySum.Add("Health", healthSum);
                dictWeeklySum.Add("Rent", rentSum);
                dictWeeklySum.Add("Utilities", utilitiesSum);


                return dictWeeklySum;
            }
        }

        public bool DeleteExpense(int catId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Expenses
                        .Single(c => c.ExpenseId == catId && c.UserId == _userId);
                ctx.Expenses.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
