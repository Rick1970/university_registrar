using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Register
{
  public class StudentTest : IDisposable
  {
    public StudentTest()
    {
      DBConfiguration.ConnectionString = "Data source=(localdb)\\mssqllocaldb;Initial Catalog=university_registrar_test; Integrated Security=SSPI;";
    }
    public void Dispose()
   {
     Course.DeleteAll();
     Student.DeleteAll();
    }
   [Fact]
   public void T1_VerifyDatabaseIsEmpty_True()
   {
     List<Student> allStudent= Student.GetAll();
     Assert.Equal(0,allStudent.Count);
   }

   [Fact]
   public void T2_ObjectandObject_returnEqual()
   {
     DateTime fakeTime=new DateTime(2016,08,02);
     Student firstStudent= new Student("Steve",fakeTime);
     Student secondStudent= new Student("Steve",fakeTime);
     Assert.Equal(firstStudent,secondStudent);
   }

   [Fact]
   public void T3_ObjectandObject_returnEqual_id()
   {
     DateTime fakeTime=new DateTime(2016,08,02);
     Student firstStudent= new Student("Steve",fakeTime,1);
     Student secondStudent= new Student("Steve",fakeTime,1);
     Assert.Equal(firstStudent,secondStudent);
   }
   [Fact]
   public void T4_SaveToDatabase_True()
   {
     DateTime fakeTime=new DateTime(2016,08,02);
     Student newStudent = new Student("Steve", fakeTime);
     newStudent.Save();

     List<Student>allStudent = Student.GetAll();
     List<Student>testStudent = new List <Student>{newStudent};

     Assert.Equal(testStudent, allStudent);

   }
   [Fact]
   public void T5_FindStudentInDatabase()
   {
     DateTime fakeTime=new DateTime(2016,08,02);
     Student newStudent = new Student("Steve", fakeTime);
     newStudent.Save();

     Student foundStudent = Student.Find(newStudent.GetId());
     Assert.Equal(newStudent, foundStudent);
   }
   [Fact]
   public void T6_AddStudentTo_Course_True()
   {
     Course testCourse = new Course("Biology", 101);
     testCourse.Save();

     DateTime fakeTime=new DateTime(2016,08,02);
     Student testStudent = new Student("Steve", fakeTime);
     testStudent.Save();

     Student testStudent2 = new Student("Mike", fakeTime);
     testStudent2.Save();

     testCourse.AddStudents(testStudent);
     testCourse.AddStudents(testStudent2);
     List<Student> allStudent= Student.GetAll();
     List<Student> result = testCourse.GetStudents();
     List<Student> testList = new List<Student>{testStudent,testStudent2};

     Assert.Equal(testList, result);
   }
  }
}
