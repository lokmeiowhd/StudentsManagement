/**
 * Student创建视图模型
 * 
 * */
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MockSchoolManagement.Models.EnumTypes;

namespace MockSchoolManagement.Models
{
    public class StudentCreateViewModel
    {
        [Display(Name = "名字")]
        [Required(ErrorMessage = "请输入名字"), MaxLength(50, ErrorMessage = "名字的长度不能超过50个字符")]
        public string Name { get; set; }

        [Display(Name = "主修科目")]
        [Required(ErrorMessage = "请选择一门科目")]
        public MajorEnum? Major { get; set; }

        [Display(Name = "电子邮箱")]
        [Required(ErrorMessage = "请输入邮箱地址")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "邮箱的格式不正确")]
        public string Email { get; set; }

        [Display(Name ="头像")]
        public List<IFormFile> Photos { get; set; }
    }
}
