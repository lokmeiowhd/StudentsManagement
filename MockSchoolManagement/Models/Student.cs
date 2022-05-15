using System.ComponentModel.DataAnnotations;
using MockSchoolManagement.Models.EnumTypes;

namespace MockSchoolManagement.Models
{
    public class Student
    {
        /// <summary>
        /// 编码
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 主修科目
        /// </summary>
        public MajorEnum? Major { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string PhotoPath { get; set; }
    }
}
