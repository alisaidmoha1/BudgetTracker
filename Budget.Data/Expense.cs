using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Data
{
    public enum Category { Food=1, Shopping, Travel, Health, Rent, Donation, Utility, Other}
    public class Expense
    {
        [Key]
        [Required]
        public int ExpenseId { get; set; }
        [Required]
        //public Category Category { get; set; }
        public int ExpenseCategoryId { get; set; }
        public virtual ExpenseCategory Category { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        [Display(Name ="Date")]
        [DisplayFormat(ApplyFormatInEditMode =true, DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime CreatedUtc { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string Note { get; set; }

        public decimal Total
        {
            get; set;
        }


    }
}
