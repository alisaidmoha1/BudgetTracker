using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Models
{
    public class CategoryListItem
    {
        [Display(Name="Category")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
