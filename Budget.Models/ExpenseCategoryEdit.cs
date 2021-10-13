using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Models
{
    public class ExpenseCategoryEdit
    {
        [Display(Name ="Category ID")]
        public int ExpenseCategoryId { get; set; }
        [Display(Name ="Category Name")]
        public string ExpenseCategoryName { get; set; }
    }
}
