using Budget.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Services
{
    public class ExpenseReportService
    {
        private readonly Guid _userId;

        public ExpenseReportService(Guid userId)
        {
            _userId = userId;
        }

        public IEnumerable<ExpenseReport> GetAllExpenses()
        {
            using (var ctx = new ApplicationDbContext())
            {
                return ctx.ExpenseReports.ToList();
            }
        }

        public IEnumerable<ExpenseReport> GetSearchResults(string searchString)
        {

            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .ExpenseReports
                        .Where(c => c.UserId == _userId && c.ItemName == searchString)
                        .Select(
                            e =>
                                new ExpenseReport
                                {
                                    ItemId = e.ItemId,
                                    ItemName = e.ItemName,
                                    Amount = e.Amount,
                                    ExpenseDate = e.ExpenseDate,
                                    Category = e.Category,
                                }

                        );
                return query.ToList();
            }

        }


        public bool AddExpense(ExpenseReport model)
        {
            var entity =
                new ExpenseReport()
                {
                    UserId = _userId,
                    ItemId = model.ItemId,
                    ItemName = model.ItemName,
                    Amount = model.Amount,
                    ExpenseDate = model.ExpenseDate,
                    Category = model.Category
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.ExpenseReports.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public bool UpdateExpense(ExpenseReport expense)
        {
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Entry(expense).State = System.Data.Entity.EntityState.Modified;

                return ctx.SaveChanges() == 1;
            }
        }

        public ExpenseReport GetExpenseData(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .ExpenseReports
                        .Single(c => c.ItemId == id && c.UserId == _userId);
                return
                    new ExpenseReport
                    {
                        ItemId = entity.ItemId,
                        ItemName = entity.ItemName,
                        Amount = entity.Amount,
                        ExpenseDate = entity.ExpenseDate,
                        Category = entity.Category
                    };
            }
        }

        public bool DeleteExpense(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .ExpenseReports
                        .Single(c => c.ItemId == id && c.UserId == _userId);
                ctx.ExpenseReports.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public Dictionary<string, decimal> CalculateMonthlyExpense()
        {
            using (var ctx = new ApplicationDbContext())
            {
                Dictionary<string, decimal> dictMonthySum = new Dictionary<string, decimal>();


                decimal foodSum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Food" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -7))).Select(cat => cat.Amount).Sum();

                decimal shoppingSum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Shopping" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -7))).Select(cat => cat.Amount).Sum();

                decimal travelSum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Travel" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -7))).Select(cat => cat.Amount).Sum();

                decimal healthSum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Food" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -7))).Select(cat => cat.Amount).Sum();

                dictMonthySum.Add("Food", foodSum);
                dictMonthySum.Add("Shopping", shoppingSum);
                dictMonthySum.Add("Travel", travelSum);
                dictMonthySum.Add("Health", healthSum);

                return dictMonthySum;


            }
        }

        public Dictionary<string, decimal> CalculateWeeklyExpense()
        {
            using (var ctx = new ApplicationDbContext())
            {

                Dictionary<string, decimal> dictWeeklySum = new Dictionary<string, decimal>();

                decimal foodSum = ctx.Expenses.Where(cat => cat.Category.ExpenseCategoryName == "Food" && (cat.CreatedUtc > DbFunctions.AddDays(DateTime.Now, -7)))
                    .Select(cat => cat.Amount)
                    .Sum();

                decimal shoppingSum = ctx.Expenses.Where
                   (cat => cat.Category.ExpenseCategoryName == "Shopping" && (cat.CreatedUtc > DbFunctions.AddDays(DateTime.Now, -28)))
                   .Select(cat => cat.Amount)
                   .Sum();

                decimal travelSum = ctx.Expenses.Where
                   (cat => cat.Category.ExpenseCategoryName == "Travel" && (cat.CreatedUtc > DbFunctions.AddDays(DateTime.Now, -28)))
                   .Select(cat => cat.Amount)
                   .Sum();

                decimal healthSum = ctx.Expenses.Where
                   (cat => cat.Category.ExpenseCategoryName == "Health" && (cat.CreatedUtc > DbFunctions.AddDays(DateTime.Now, -28)))
                   .Select(cat => cat.Amount)
                   .Sum();

                dictWeeklySum.Add("Food", foodSum);
                dictWeeklySum.Add("Shopping", shoppingSum);
                dictWeeklySum.Add("Travel", travelSum);
                dictWeeklySum.Add("Health", healthSum);

                return dictWeeklySum;
            }
        }
    }
}

