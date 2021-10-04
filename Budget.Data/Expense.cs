using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Data
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        [Required]
        public DateTimeOffset CreatedUtc { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [Display(Name ="Repeat")]
        public bool IsRepeat { get; set; }
        public string Note { get; set; }
    }
}
