using FluentValidation;
using PLMSide.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PLMSide.Validator
{
    public class CustomersInfoValidator : AbstractValidator<CustomersInfo>
    {
        public CustomersInfoValidator() {
            //RuleFor(t => t.Customer_Code).NotNull().WithMessage("Customer Code不能为空");
            //RuleFor(t => t.Customer_Group).NotNull().WithMessage("Customer Name不能为空");
            RuleFor(t => t.Product_Channel).NotNull().WithMessage("Product Channel不能为空");
            RuleFor(t => t.WHS_Channel).NotNull().WithMessage("WHS Channel不能为空");
            RuleFor(t => t.Customer_Group).NotNull().WithMessage("Customer Group不能为空");
            RuleFor(t => t.branch_EN).NotNull().WithMessage("Branch EN不能为空");
            //RuleFor(t => t.branch_CN).NotNull().WithMessage("Branch CN不能为空");
            RuleFor(t => t.Account_Sales).NotNull().WithMessage("Account Sales不能为空");
            //RuleFor(t => t.kids_account_sales).NotNull().WithMessage("Kids Account Sales不能为空");
            //RuleFor(t => t.Category_Sales).NotNull().WithMessage("Category Sales不能为空");
            RuleFor(t => t.Status).NotNull().WithMessage("Status不能为空");
        }
    }
}
