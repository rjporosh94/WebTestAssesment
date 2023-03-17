
USE [TestAssesmentDB]
GO
/****** Object:  StoredProcedure [dbo].[Sp_CreateMarks]    Script Date: 3/17/2023 9:58:12 PM ******/
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

--Create Procedure Sp_CreateMarks   
--    @StudentId int,
--    @CourseId int,
--	@Marks int

--AS
--BEGIN
--    SET NOCOUNT ON;
--	SET IDENTITY_INSERT Marks ON;
--    Insert into Marks(
--           [StudentId]
--           ,[CourseId]
--		   ,[Marks]
--           )
-- Values (@StudentId, @CourseId,@Marks)
-- SET IDENTITY_INSERT Marks OFF;
--END
--GO

CREATE OR ALTER  Procedure Sp_GetMarks   
AS
BEGIN
Select FullName = Student.FullName,CourseName = STRING_AGG(Courses.CourseName,', '), TotalMarks = SUM(Marks)  , Average = AVG(Marks) From Marks m
LEFT JOIN Student
  ON Student.Id = m.StudentId
LEFT JOIN Courses
  ON Courses.Id = m.CourseId
  GROUP BY FullName,m.Id
END 
GO

CREATE OR ALTER VIEW marksview
AS
SELECT  Id= c.Id, a.FullName,b.CourseName
--,STRING_AGG(b.CourseName,', ')
, c.Marks
--, TotalMarks = SUM(Marks)  , Average = AVG(Marks)
FROM Student a, Courses b, Marks c
WHERE a.Id=c.StudentId
AND b.Id=c.CourseId 
--GROUP BY FullName,c.Id
GO


