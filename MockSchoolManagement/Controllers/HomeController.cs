using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MockSchoolManagement.DataRepositories;
using MockSchoolManagement.Models;
using MockSchoolManagement.ViewModels;

namespace MockSchoolManagement.Controllers
{
    [Route("Home")]
    [Authorize]
    public class HomeController : Controller
    {
        [Route("/")]
        [Route("Index")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<Student> students = _studentRepository.GetAllStudents();
            return View(students);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private readonly IStudentRepository _studentRepository;

        private IWebHostEnvironment _webHostEnvironment;

        private readonly ILogger logger;

        /// <summary>
        /// 使用构造函数注入的方式注入
        /// </summary>
        /// <param name="studentRepository">IStudentRepository</param>
        /// <param name="webHostEnvironment">webHostEnvironment</param>
        /// <param name="logger">ILogger<HomeController> logger</param>
        public HomeController(IStudentRepository studentRepository, IWebHostEnvironment webHostEnvironment, ILogger<HomeController> logger)
        {
            _studentRepository = studentRepository;
            _webHostEnvironment = webHostEnvironment;
            this.logger = logger;
        }

        #region 历史代码
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        /// <summary>
        /// 返回json数据
        /// </summary>
        /// <returns></returns>
        //public JsonResult Details()
        //{
        //    Student student = _studentRepository.GetStudent(1);
        //    return Json(student);
        //}

        ///返回视图Details.cshtml（默认和方法名相同）
        //public ViewResult Details()
        //{
        //    return View();
        //}

        ///增加参数则返回Test.cshtml视图
        //public ViewResult Details()
        //{
        //    return View("Test");
        //}

        ///绝对视图路径：必须加上.cshtml扩展名，MVC会从根目录开始搜索，推荐使用/或者~/
        //public ViewResult Details()
        //{
        //    //return View("MyViews/Test.cshtml");
        //    //return View("/MyViews/Test.cshtml");
        //    return View("~/MyViews/Test.cshtml");
        //}

        ///相对视图路径
        //public ViewResult Details()
        //{
        //    return View("../Test/Update");
        //    return View("../../Test/Update");
        //}

        //[Route("Details_ViewData")]
        //public ViewResult Details_ViewData()
        //{
        //    Student student = _studentRepository.GetStudent(1);
        //    //使用ViewData将PageTitle和Student模型传递给View
        //    ViewData["PageTitle"] = "Student Details";
        //    ViewData["Student"] = student;
        //    return View();
        //}

        //[Route("Details_ViewBag")]
        //public ViewResult Details_ViewBag()
        //{
        //    Student model = _studentRepository.GetStudent(1);
        //    //将PageTitle和Student模型对象存储在ViewBag
        //    //我们正在使用动态属性PageTitle和Student
        //    ViewBag.PageTitle = "学生详情";
        //    ViewBag.Student = model;
        //    return View();
        //}

        //[Route("Details_Model")]
        //public ViewResult Details_Model(int id)
        //{
        //    Student model = _studentRepository.GetStudent(id);
        //    ViewBag.PageTitle = "学生详情";
        //    return View(model);
        //}
        #endregion

        [Route("Details")]
        [AllowAnonymous]
        public ViewResult Details(int id)
        {
            #region 日志级别演示
            logger.LogTrace("LogTrace(跟踪)");
            logger.LogDebug("LogDebug(调试)");
            logger.LogInformation("LogInformation(信息)");
            logger.LogWarning("LogWarning(警告)");
            logger.LogError("LogError(错误)");
            logger.LogCritical("LogCritical(严重)");
            #endregion

            var student = _studentRepository.GetStudentById(id);

            if (student == null)
            {
                Response.StatusCode = 404;
                return View("StudentNotFound", id);
            }

            //实例化HomeDetailsViewModel并储存Student详细信息和PageTitle
            HomeDetailsViewModel viewModel = new HomeDetailsViewModel()
            {
                PageTitle = "学生详情",
                Student = student
            };
            return View(viewModel);
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(StudentCreateViewModel model)
        {

            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photos != null && model.Photos.Count > 0)
                {
                    foreach (IFormFile photo in model.Photos)
                    {
                        //必须将图片文件上传到wwwroot的images文件夹中，而要获取wwwroot文件夹的路径，
                        //我们需要注入ASP.net Core提供的WebHostEnvironment服务
                        string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars");
                        //为了确保文件名是惟一的，我们在文件名后附加一个新的GUID值和一个下划线
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                        string filePath = Path.Combine(uploadFolder, uniqueFileName);
                        //使用IFormFile接口提供的CopyTo（）方法将文件复制到wwwroot/images文件夹
                        photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }

                Student newStudent = new Student
                {
                    Name = model.Name,
                    Email = model.Email,
                    Major = model.Major,
                    //将文件名保存在Student对象中PhotoPath属性中
                    //它将被保存到数据库Students的表中
                    PhotoPath = uniqueFileName
                };

                _studentRepository.Insert(newStudent);
                return RedirectToAction("Details", new { id = newStudent.Id });
            }
            return View();
        }

        [HttpGet]
        [Route("Edit")]
        public ViewResult Edit(int id)
        {
            Student student = _studentRepository.GetStudentById(id);

            if (student == null)
            {
                Response.StatusCode = 404;
                return View("StudentNotFound", id);
            }

            StudentEditViewModel studentEditViewModel = new StudentEditViewModel
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Major = student.Major,
                ExistingPhotoPath = student.PhotoPath
            };
            return View(studentEditViewModel);
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit(StudentEditViewModel model)
        {
            //检查提供的数据是否有效，如果没有通过验证，需要重新编辑学生信息
            //这样用户就可以更正并重新提交编辑表单
            if (ModelState.IsValid)
            {
                //从数据库中查询正在编辑的学生信息
                Student student = _studentRepository.GetStudentById(model.Id);
                //用模型对象中的数据更新Student对象
                student.Name = model.Name;
                student.Email = model.Email;
                student.Major = model.Major;

                //如果用户想要更改图片，可以上传新图片文件，他会被模型对象上的Photos属性接收，如果用户没有上传图片
                //那么我们会保留现有的图片文件信息，因为兼容了多图上传，所以将这里的!=null判断修改为判断Photos的总数是否大于0
                if (model.Photos.Count > 0)
                {
                    //如果上传了新的图片则必须显示新的图片信息
                    //因此检查当前学生信息中是否有图片，如果有则会删除他
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    //我们将新的图片文件保存到wwwroot/images/avatars文件夹中，并且会更新Student对象中的PhotoPath属性
                    //最终都会将他们保存到数据库中
                    student.PhotoPath = ProcessUploadedFile(model);
                }

                //调用仓储服务中的Update()方法，保存Student对象中的数据，更新数据库表中的信息
                Student updatedstudent = _studentRepository.Update(student);
                return RedirectToAction("index");
            }
            return View(model);
        }

        /// <summary>
        /// 将图片保存在指定的路径中并返回唯一的文件名
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string ProcessUploadedFile(StudentEditViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photos.Count > 0)
            {
                foreach (var photo in model.Photos)
                {
                    //将新的图片文件保存到wwwroot/images/avatars文件夹中
                    //而要获取wwwroot文件夹的路径，我们需要注册.net core服务提供的webhostEnvironment服务
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars");
                    //为了确保文件名是唯一的，我们在文件名后附加一个新的GUID和一个下划线
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    //因为使用了非托管对象，所以需要手动进行释放
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        //使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/images/avatars文件夹
                        photo.CopyTo(fileStream);
                    };

                }
            }
            return uniqueFileName;
        }
    }
}
