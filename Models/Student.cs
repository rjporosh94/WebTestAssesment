using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebTestAssesment.Models;

public partial class Student
{
    [Key]
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public int? Age { get; set; }

    public string? Roll { get; set; }
    //public virtual ICollection<Course>? Courses { get; set; }
    public virtual ICollection<Mark> Marks { get; } = new List<Mark>();
}
