using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Models
{
    public class IncomeCategoryListItem
    {
        [Display(Name = "Category")]
        public int IncomeCategoryId { get; set; }
        [Display(Name = "Category Name")]
        public string IncomeCategoryName { get; set; }
    }
}
