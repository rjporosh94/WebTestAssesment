
USE [TestAssesmentDB]
GO
/****** Object:  StoredProcedure [dbo].[Sp_CreateMarks]    Script Date: 3/14/2023 1:10:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Procedure [dbo].[Sp_CreateMarks]   
    @StudentId int,
    @CourseId int,
	@Marks int

AS
BEGIN
    SET NOCOUNT ON;
    Insert into Marks(
           [StudentId]
           ,[CourseId]
		   ,[Marks]
           )
 Values (@StudentId, @CourseId,@Marks)
END

GO

Create Procedure Sp_CreateMarks   
    @StudentId int,
    @CourseId int,
	@Marks int

AS
BEGIN
    SET NOCOUNT ON;
	SET IDENTITY_INSERT Marks ON;
    Insert into Marks(
           [StudentId]
           ,[CourseId]
		   ,[Marks]
           )
 Values (@StudentId, @CourseId,@Marks)
 SET IDENTITY_INSERT Marks OFF;
END
GO

CREATE OR ALTER  Procedure Sp_GetMarks   
    @StudentId int

AS
BEGIN
Select FullName = Students.FullName,CourseName = Courses.CouseName, TotalMarks = SUM(Marks)  , Average = AVG(Marks) From Marks 
JOIN Students
  ON Students.Id = Marks.StudentId
JOIN Courses
  ON Courses.Id = Marks.CourseId
  GROUP BY StudentId
  ;
END 
GO

CREATE OR ALTER VIEW marksview
AS
SELECT  a.FullName, b.CourseName , c.Marks
FROM Students a, Courses b, Marks c
WHERE a.Id=c.StudentId
AND b.Id=c.CourseId;

