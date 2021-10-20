﻿using Budget.Data;
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
        [Display(Name ="Category")]
        public int ExpenseCategoryId { get; set; }
        public SelectList ExpenseCategories { get; set; }
        //public Category Category { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedUtc { get; set; } = DateTime.Now;
        [Required]
        public decimal Amount { get; set; }
        public decimal Total { get { return Amount; } set { } }
        [Required]
        [Display(Name ="Repeat")]
        public bool IsRepeat { get; set; }
        [Display(Name ="Note(Optional)")]
        public string Note { get; set; }
    }
}
