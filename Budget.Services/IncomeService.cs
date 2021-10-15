using Budget.Data;
using Budget.Models;
using System;
using System.Collections.Generic;
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
                    IsRepeat = model.IsRepeat,
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
                                    Amount = e.Amount
                                }

                        ); ;
                return query.ToArray();
            }
        }

        public IEnumerable<IncomeListItem> GetSearchResult (string searchString)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Incomes.ToList();
                    
                  return (IEnumerable<IncomeListItem>)entity.Where(x => x.IncomeCategory.IncomeCategoryName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1);
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
                entity.CreatedUtc = entity.CreatedUtc;
                entity.Amount = entity.Amount;
                entity.IsRepeat = model.IsRepeat;
                entity.Note = entity.Note;

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
                        CreatedUtc = entity.CreatedUtc,
                        Amount = entity.Amount,
                        IsRepeat = entity.IsRepeat,
                        Note = entity.Note
                    };
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

