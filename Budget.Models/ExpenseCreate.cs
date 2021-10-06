using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Budget.Models
{
    public class ExpenseCreate
    {
        [Required]
        public int CategoryId { get; set; }
        public SelectList Categories { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedUtc { get; set; } = DateTime.Now;
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public bool IsRepeat { get; set; }
        public string Note { get; set; }
    }
}
