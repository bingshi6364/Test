using FluentValidation;
using PLMSide.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PLMSide.Validator
{
    public class POS_MasterValidator : AbstractValidator<POS_Master>
    {
        public POS_MasterValidator() {
            RuleFor(t => t.POS_Status).NotNull().WithMessage("POS_Status不能为空");
            RuleFor(t => t.POS_CODE).NotNull().WithMessage("POS_CODE不能为空");
            RuleFor(t => t.POS_CHANNEL).NotNull().WithMessage("POS_CHANNEL不能为空");
            RuleFor(t => t.CUSTOMER_CODE).NotNull().WithMessage("CUSTOMER_CODE不能为空");
            //RuleFor(t => t.CUSTOMER_GROUP).NotNull().WithMessage("CUSTOMER_GROUP不能为空");
            //RuleFor(t => t.CUSTOMER_NAME).NotNull().WithMessage("CUSTOMER_NAME不能为空");
            //RuleFor(t => t.BRANCH_EN).NotNull().WithMessage("BRANCH_EN不能为空");
            //RuleFor(t => t.BRANCH_CN).NotNull().WithMessage("BRANCH_CN不能为空");
           // RuleFor(t => t.WHS_CHANNEL).NotNull().WithMessage("WHS_CHANNEL不能为空");
           // RuleFor(t => t.Kids_in_Large_Store).NotNull().WithMessage("Kids_in_Large_Store不能为空");
            RuleFor(t => t.Region).NotNull().WithMessage("Region不能为空");
            RuleFor(t => t.PROVINCE_CN).NotNull().WithMessage("PROVINCE_CN不能为空");
            RuleFor(t => t.PROVINCE_EN).NotNull().WithMessage("PROVINCE_EN不能为空");
            RuleFor(t => t.CITY_CN).NotNull().WithMessage("CITY_CN不能为空");
            RuleFor(t => t.CITY_EN).NotNull().WithMessage("CITY_EN不能为空");
            RuleFor(t => t.CITY_TIER).NotNull().WithMessage("CITY_TIER不能为空");
            RuleFor(t => t.ADDRESS).NotNull().WithMessage("ADDRESS不能为空");
            RuleFor(t => t.GROSS_SPACE).NotNull().WithMessage("GROSS_SPACE不能为空");
            RuleFor(t => t.FCST_MPSA).NotNull().WithMessage("FCST_MPSA不能为空");
            RuleFor(t => t.Buying_REGION).NotNull().WithMessage("Buying_REGION不能为空");
            RuleFor(t => t.BIG_FORMAT).NotNull().WithMessage("BIG_FORMAT不能为空");
            RuleFor(t => t.Biz_Mode).NotNull().WithMessage("Biz_Mode不能为空");
            //RuleFor(t => t.Product_Channel).NotNull().WithMessage("Product_Channel不能为空");
            //RuleFor(t => t.PackagePOS).NotNull().WithMessage("PackagePOS不能为空");
           // RuleFor(t => t.AssortmentPOS).NotNull().WithMessage("AssortmentPOS不能为空");
        }
    }
}