using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Data
{
    public class Income
    {
        [Key]
        public int IncomeId { get; set; }
        [Required]
        public int IncomeCategoryId { get; set; }
        public virtual IncomeCategory IncomeCategory { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedUtc { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string Note { get; set; }
    }

}
