using FluentValidation;
using PLMSide.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PLMSide.Validator
{
    public class RoleValidator: AbstractValidator<Roles>
    {
        public RoleValidator()
        {
            RuleFor(x => x.RoleName).NotEmpty()
                .WithName("角色名").WithMessage("{PropertyName}是必须填写的")
                .MaximumLength(5).WithMessage("{PropertyName}的最大长度是{MaxLength}");

            RuleFor(x => x.RoleName).Must(roleName =>
            {
                return true;
            });
        }
    }
}
