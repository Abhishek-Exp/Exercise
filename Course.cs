using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RealStudentCourses
{
    public class Course
    {
        [Required]
        public string Name { get; set; }
        public string ID { get; set; }

        public int Hours { get; set; }

        public string Type { get; set; }

        public string Elective { get; set; }
    }
}
