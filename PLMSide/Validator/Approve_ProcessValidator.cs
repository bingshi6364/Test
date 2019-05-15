using FluentValidation;
using PLMSide.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PLMSide.Validator
{
    public class Approve_ProcessValidator:AbstractValidator<ApprovalProcess>
    {
        public Approve_ProcessValidator() {
            RuleFor(t => t.AS_ID).NotNull().WithMessage("Account Sales 不能为空");
            RuleFor(t => t.ASM_ID).NotNull().WithMessage("ASM 不能为空");
        }
    }
}
