using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RealStudentCourses.Controllers
{
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private static List<Course> _courses = new List<Course>();
        private static List<Student> _students = new List<Student>();
        private static List<StudentName> _studentsName = new List<StudentName>();
        private static List<StudentCount> _studentsCount = new List<StudentCount>();




        [HttpPost("api/course")]
        public IActionResult CreateCourse(Course C)
        {
            foreach (var item in _courses)
            {
                if (item.ID == C.ID)
                    return Conflict();
            }

            var CourseToBeAdded = new Course
            {
                Name = C.Name,
                ID = C.ID,
                Hours = C.Hours,
                Type = C.Type,
                Elective = C.Elective
            };
            _courses.Add(CourseToBeAdded);
            return Ok(CourseToBeAdded.ID);
        }

        [HttpGet("api/course")]
        public IActionResult DisplayCourses()
        {
            return Ok(_courses);
        }

        [HttpGet("api/course/{id}")]
        public IActionResult DisplaySingleCourse(string id)
        {
            foreach (var item in _courses)
            {
                if (item.ID == id)
                    return Ok(item);
            }
            return NotFound();
        }

        [HttpDelete("api/course/{id}")]
        public IActionResult DeleteCourse(string id)
        {
            foreach (var item in _courses)
            {
                if (item.ID == id)
                {
                    _courses.Remove(item);
                    return Ok();
                }
            }
            return NotFound();

        }

        [HttpPut("api/course/")]
        public IActionResult EditCourse(Course C)
        {
            foreach (var item in _courses)
            {
                if (item.ID == C.ID)
                {
                    item.Name = C.Name;
                    item.ID = C.ID;
                    item.Hours = C.Hours;
                    item.Type = C.Type;
                    item.Elective = C.Elective;
                    return Ok(item);
                }
            }
            return NotFound();

        }

        [HttpPost("api/student/")]
        public IActionResult AddStudent(Student S)
        {
            bool flag = false;
            DateTime date1, date2;
           

            if (DateTime.TryParseExact(S.DateOfBirth, new[] { "dd-MMM-yyyy" }, null, DateTimeStyles.None, out date1))
                String.Format("{0:dd-mmm-yyyy}", date1);
            else
                return Conflict();
            if (DateTime.TryParseExact(S.EnrollmentDate, new[] { "dd-MMM-yyyy" }, null, DateTimeStyles.None, out date2))
                String.Format("{0:dd-mmm-yyyy}", date2);
            else
                return Conflict();
         
            if (date1 > DateTime.Now)
                return Conflict();
            if (date2 > DateTime.Now)
                return Conflict();


            foreach (var item in _courses)
            {
                if (item.Name == S.Course)
                {
                    flag = true;
                    break;
                }
            }

            if (flag == false)
                return Conflict();


            var lastStudent = _students.OrderByDescending(x => x.ID).FirstOrDefault();

            int id = lastStudent == null ? 1 : lastStudent.ID + 1;

            var StudentToBeAdded = new Student
            {
                ID = id,
                FirstName = S.FirstName,
                LastName = S.LastName,
                DateOfBirth = S.DateOfBirth,
                Address = S.Address,
                PhoneNumber = S.PhoneNumber,
                Course = S.Course,
                EnrollmentDate = S.EnrollmentDate
            };
            _students.Add(StudentToBeAdded);
            return Ok(StudentToBeAdded.FirstName);

        }

        [HttpGet("api/student/")]
        public IActionResult ViewStudents()
        {
            return Ok(_students);
        }

        [HttpPut("api/student/")]
        public IActionResult EditStudent(Student S)
        {

            bool flag = false;
            if ((Convert.ToDateTime(S.DateOfBirth) > DateTime.Now) || (Convert.ToDateTime(S.EnrollmentDate) > DateTime.Now))
                return Conflict();

            foreach (var item in _courses)
            {
                if (item.Name == S.Course)
                {
                    flag = true;
                    break;
                }
            }

            if (flag == false)
                return Conflict();

            foreach (var item in _students)
            {
                if (item.ID == S.ID)
                {
                    item.FirstName = S.FirstName;
                    item.LastName = S.LastName;
                    item.DateOfBirth = S.DateOfBirth;
                    item.Address = S.Address;
                    item.PhoneNumber = S.PhoneNumber;
                    item.Course = S.Course;
                    item.EnrollmentDate = S.EnrollmentDate;
                    return Ok(item);
                }
            }
            return NotFound();
        }

        [HttpGet("api/studentcount")]
        public IActionResult StudentCount()
        {
            var qry = _courses.GroupJoin(_students, c => c.Name, s => s.Course, (c, s) => new { CourseName = c.Name , StudentCount = s.Count() } );
           
            return Ok(qry);
        }

        [HttpGet("api/studentlist")]
        public IActionResult StudentList()
        {
            foreach (var item in _students)
            {
                var StudentNameToBeAdded = new StudentName()
                {
                    Name = item.FirstName + " " + item.LastName
                };
                _studentsName.Add(StudentNameToBeAdded);
            }
            return Ok(_studentsName);
        }
    }
}

