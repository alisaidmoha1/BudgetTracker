using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Data
{
    public class IncomeCategory
    {
        [Key]
        public int IncomeCategoryId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string IncomeCategoryName { get; set; }
    }
}
