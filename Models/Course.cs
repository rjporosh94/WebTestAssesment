using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTestAssesment.Models;

public partial class Course
{
    [Key]
    public int Id { get; set; }

    public string CourseName { get; set; } = null!;

    public virtual ICollection<Mark> Marks { get; } = new List<Mark>();
    //public virtual ICollection<Student> Students { get; } = new List<Student>();
}
