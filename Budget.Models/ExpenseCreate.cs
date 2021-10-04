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
        public SelectList Categories { get; set; }
        [Required]
        public DateTimeOffset CreatedUtc { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public bool IsRepeat { get; set; }
        public string Note { get; set; }
    }
}
