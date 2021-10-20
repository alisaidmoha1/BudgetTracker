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
    public class IncomeService
    {
        private readonly Guid _userId;

        public IncomeService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateIncome(IncomeCreate model)
        {
            var entity =
                new Income()
                {
                    UserId = _userId,
                    IncomeCategoryId = model.IncomeCategoryId,
                    CreatedUtc = model.CreatedUtc,
                    Amount = model.Amount,
                    Note = model.Note
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Incomes.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<IncomeListItem> GetIncome()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Incomes
                        .Where(c => c.UserId == _userId)
                        .Select(
                            e =>
                                new IncomeListItem
                                {
                                    IncomeId = e.IncomeId,
                                    IncomeCategoryName = e.IncomeCategory.IncomeCategoryName,
                                    CreatedUtc = e.CreatedUtc,
                                    Amount = e.Amount,
                                    Note = e.Note
                                }

                        ); ;
                return query.ToArray();
            }
        }

        public bool UpdateIncome(IncomeEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Incomes
                        .Single(c => c.IncomeId == model.IncomeId && c.UserId == _userId);

                entity.IncomeCategoryId = model.IncomeCategoryId;
                entity.CreatedUtc = model.CreatedUtc;
                entity.Amount = model.Amount;
                entity.Note = model.Note;

                return ctx.SaveChanges() == 1;
            }
        }

        public IncomeEdit GetIncomeById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Incomes
                        .Single(c => c.IncomeId == id && c.UserId == _userId);
                return
                    new IncomeEdit
                    {
                        IncomeCategoryId = entity.IncomeCategoryId,
                        IncomeId = entity.IncomeId,
                        CreatedUtc = entity.CreatedUtc,
                        Amount = entity.Amount,
                        Note = entity.Note
                    };
            }
        }

        public Dictionary<string, decimal> CalculateYearlyIncome()
        {
            using (var ctx = new ApplicationDbContext())
            {

                Dictionary<string, decimal> dictYearlySum = new Dictionary<string, decimal>();


                decimal salary = ctx.Incomes.Where(cat => cat.IncomeCategory.IncomeCategoryName == "Salary" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -13)))
                    .Select(cat => cat.Amount)
                    .Sum();

                decimal freelance = ctx.Incomes.Where
                   (cat => cat.IncomeCategory.IncomeCategoryName == "Freelance" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -13)))
                   .Select(cat => cat.Amount)
                   .Sum();

                decimal other = ctx.Incomes.Where
                   (cat => cat.IncomeCategory.IncomeCategoryName == "Other" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -13)))
                   .Select(cat => cat.Amount)
                   .Sum();

                dictYearlySum.Add("Salary", salary);
                dictYearlySum.Add("Freelance", freelance);
                dictYearlySum.Add("Other", other);


                return dictYearlySum;


            }
        }

        public Dictionary<string, decimal> CalculateMonthlyIncome()
        {
            using (var ctx = new ApplicationDbContext())
            {

                Dictionary<string, decimal> dictMonthlySum = new Dictionary<string, decimal>();

                decimal salary = ctx.Incomes.Where(cat => cat.IncomeCategory.IncomeCategoryName == "Salary" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -4)))
                    .Select(cat => cat.Amount)
                    .Sum();

                decimal freelance = ctx.Incomes.Where
                   (cat => cat.IncomeCategory.IncomeCategoryName == "Freelance" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -4)))
                   .Select(cat => cat.Amount)
                   .Sum();

                decimal other = ctx.Incomes.Where
                   (cat => cat.IncomeCategory.IncomeCategoryName == "Other" && (cat.CreatedUtc > DbFunctions.AddMonths(DateTime.Now, -4)))
                   .Select(cat => cat.Amount)
                   .Sum();

                dictMonthlySum.Add("Salary", salary);
                dictMonthlySum.Add("Freelance", freelance);
                dictMonthlySum.Add("Other", other);
                

                return dictMonthlySum;
            }
        }

        public bool DeleteIncome(int catId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Incomes
                        .Single(c => c.IncomeId == catId && c.UserId == _userId);
                ctx.Incomes.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}

