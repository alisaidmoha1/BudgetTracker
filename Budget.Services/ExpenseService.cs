﻿using Budget.Data;
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
                    CreatedUtc = model.CreatedUtc,
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
                                {   ExpenseId = e.ExpenseId,
                                    CategoryName = e.Category.CategoryName,
                                    CreatedUtc = e.CreatedUtc,
                                    Amount = e.Amount
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

                entity.CategoryId = model.CategoryId;
                entity.CreatedUtc = entity.CreatedUtc;
                entity.Amount = entity.Amount;
                entity.IsRepeat = model.IsRepeat;
                entity.Note = entity.Note;

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
                        CategoryId = entity.CategoryId,
                        CreatedUtc = entity.CreatedUtc,
                        Amount = entity.Amount,
                        IsRepeat = entity.IsRepeat,
                        Note = entity.Note
                    };
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
