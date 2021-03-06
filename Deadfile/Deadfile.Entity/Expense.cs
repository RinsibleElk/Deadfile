﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    public class Expense
    {
        public int ExpenseId { get; set; }

        [Required(ErrorMessage = "An Expense must be given a short description"),
         MaxLength(100, ErrorMessage = "An Expense description must have at most 100 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "An Expense must have an amount associated")]
        public double NetAmount { get; set; }

        [MaxLength(500, ErrorMessage = "The free notes for an Expense must be less than 500 characters long.")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "An Expense must have a creation date.")]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "An Expense must have a Type")]
        public ExpenseType Type { get; set; }

        [Required(ErrorMessage = "An Expense must have a State")]
        public BillableState State { get; set; } = BillableState.Active;

        public int JobId { get; set; }
        public virtual Job Job { get; set; }
        public int? InvoiceId { get; set; }
    }
}
