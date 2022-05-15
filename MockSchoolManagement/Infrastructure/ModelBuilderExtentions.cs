using Microsoft.EntityFrameworkCore;
using MockSchoolManagement.Models;

namespace MockSchoolManagement.Infrastructure
{
    public static class ModelBuilderExtentions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(new Student
            {
                Id = 1,
                Name = "刘双龙",
                Major = Models.EnumTypes.MajorEnum.FirstGrade,
                Email = "jackeyliu187@163.com",
                PhotoPath="长发.png"
            });

            modelBuilder.Entity<Student>().HasData(new Student
            {
                Id = 2,
                Name = "马会芳",
                Major = Models.EnumTypes.MajorEnum.FirstGrade,
                Email = "jackeyliu187@163.com",
                PhotoPath = "短发.jpg"
            });
        }
    }
}
