using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Budget.Models
{
    public class IncomeEdit
    {
        public int IncomeId { get; set; }
        [Display(Name = "Category")]
        public int IncomeCategoryId { get; set; }
        public IEnumerable<SelectListItem> IncomeCategories { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedUtc { get; set; }

        public decimal Amount { get; set; }

        public string Note { get; set; }
    }
}
