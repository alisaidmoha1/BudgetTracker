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
        [Display(Name ="Category")]
        public string CategoryName { get; set; }
        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTimeOffset CreatedUtc { get; set; }
        [Display(Name ="Amount")]
        public decimal Amount { get; set; }
        
    }
}
