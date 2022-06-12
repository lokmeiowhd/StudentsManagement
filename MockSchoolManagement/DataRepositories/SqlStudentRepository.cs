using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MockSchoolManagement.Infrastructure;
using MockSchoolManagement.Models;

namespace MockSchoolManagement.DataRepositories
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        private readonly ILogger logger;

        public SqlStudentRepository(AppDbContext context, ILogger<SqlStudentRepository> logger)
        {
            this._context = context;
            this.logger = logger;
        }

        public Student Delete(int id)
        {
            Student student = _context.Students.Find(id);

            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }

            return student;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            #region 日志级别演示
            logger.LogTrace("LogTrace(跟踪) 学生仓储类");
            logger.LogDebug("LogDebug(调试) 学生仓储类");
            logger.LogInformation("LogInformation(信息) 学生仓储类");
            logger.LogWarning("LogWarning(警告) 学生仓储类");
            logger.LogError("LogError(错误) 学生仓储类");
            logger.LogCritical("LogCritical(严重) 学生仓储类");
            #endregion

            return _context.Students;
        }

        public Student GetStudentById(int id)
        {
            return _context.Students.Find(id);
        }

        public Student Insert(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return student;
        }

        public Student Update(Student updateStudent)
        {
            var student = _context.Students.Attach(updateStudent);
            student.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updateStudent;
        }
    }
}
