using System.ComponentModel.DataAnnotations;
using Deadfile.Entity;

namespace Deadfile.Model
{
    public static class InvoiceModelInvoiceReferenceValidator
    {
        public static ValidationResult InvoiceReferenceIsValid(int invoiceReference, ValidationContext context)
        {
            var invoiceModel = context.ObjectInstance as InvoiceModel;
            if (invoiceModel == null)
                return new ValidationResult("The Invoice Reference cannot be validated without an invoice model",
                    new string[] { nameof(InvoiceModel.InvoiceReference) });
            if (invoiceModel.InvoiceReferenceIsUniqueForCompany())
                return ValidationResult.Success;
            return new ValidationResult("The Invoice Reference must be unique for " + CompanyUtils.GetName(invoiceModel.Company),
                new string[] { nameof(InvoiceModel.InvoiceReference) });
        }
    }
}