using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockSchoolManagement.Models;
using MockSchoolManagement.Models.EnumTypes;

namespace MockSchoolManagement.DataRepositories
{
    public class MockStudentRepository : IStudentRepository
    {
        private readonly List<Student> _studentList;

        public MockStudentRepository()
        {
            _studentList = new List<Student>()
            {
                new Student()
                {
                    Id=1,Name="张三",Major=MajorEnum.FirstGrade,Email="zhangshan@52bp.com"
                },
                new Student()
                {
                    Id=2,Name="李四",Major=MajorEnum.GradeThree,Email="zhangshan@52bp.com"
                },
                new Student()
                {
                    Id=3,Name="王五",Major=MajorEnum.SecondGrade,Email="zhangshan@52bp.com"
                }
            };
        }

        public Student GetStudentById(int id)
        {
            return _studentList.FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _studentList;
        }

        public Student Insert(Student student)
        {
            student.Id = _studentList.Max(s => s.Id) + 1;
            _studentList.Add(student);
            return student;
        }

        public Student Update(Student updateStudent)
        {
            Student student = _studentList.FirstOrDefault(s => s.Id == updateStudent.Id);
            if (student != null)
            {
                student.Email = updateStudent.Email;
                student.Major = updateStudent.Major;
                student.Name = updateStudent.Name;
            }
            return student;
        }

        public Student Delete(int id)
        {
            Student student = _studentList.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                _studentList.Remove(student);
            }
            return student;
        }
    }
}
