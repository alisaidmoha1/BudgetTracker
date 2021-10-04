using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Models
{
  public class ExpenseListItem
    {
        public int ExpenseId { get; set; }
        public string CategoryName { get; set; }
        public DateTimeOffset CreatedUtc { get; set; }
        [Display(Name ="Date")]
        public decimal Amount { get; set; }
        
    }
}
