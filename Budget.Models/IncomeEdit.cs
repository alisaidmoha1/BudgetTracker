﻿using System;
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
        public SelectList Categories { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedUtc { get; set; }

        public decimal Amount { get; set; }

        public bool IsRepeat { get; set; }
        public string Note { get; set; }
    }
}
