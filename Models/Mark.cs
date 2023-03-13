using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace WebTestAssesment.Models;

public partial class Mark
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Course")]
    public  int CourseId { get; set; }
    [ForeignKey("Student")]
    public int StudentId { get; set; }

    public int Marks { get; set; }

    public virtual Course? Course { get; set; } 
    //public virtual List<Course>? Courses { get; set; }
    public virtual Student? Student { get; set; }
}
