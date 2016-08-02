using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Register
{
  public class CourseTest : IDisposable
  {
    public CourseTest()
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
     List<Course> allCourses = Course.GetAll();
     Assert.Equal(0,allCourses.Count);
   }
    [Fact]
    public void T2_ObjectandObject_returnEqual()
    {
      Course firstCourse = new Course("Biology", 101);
      Course secondCourse = new Course("Biology", 101);

      Assert.Equal(firstCourse, secondCourse);
    }
    [Fact]
    public void T3_SaveInDatabase_True()
    {
    Course firstCourse = new Course("Biology", 101);
    firstCourse.Save();
    List<Course> newList = Course.GetAll();
    List<Course> testCourse = new List<Course>{firstCourse};

    Assert.Equal(testCourse, newList);

    }

    [Fact]
    public void T4_Find_inDataBase()
    {
      Course firstCourse = new Course("Biology", 101);
      firstCourse.Save();
      Course resultCourse = Course.Find(firstCourse.GetId());
      Assert.Equal(firstCourse,resultCourse);
    }

    [Fact]
    public void T5_AddStudentsToCourse()
    {
      Course firstCourse = new Course("Biology", 101);
      Course secondCourse = new Course("Physics", 102);
      firstCourse.Save();
      secondCourse.Save();

      DateTime fakeTime=new DateTime(2016,08,02);
      Student testStudent = new Student("Steve", fakeTime);
      testStudent.Save();

      testStudent.AddCourses(firstCourse);
      testStudent.AddCourses(secondCourse);
      List<Course> temp = Course.GetAll();

      List<Course> resultCourse = testStudent.GetCourses();
      List<Course> testCourses=new List<Course>{firstCourse,secondCourse};
      Assert.Equal(testCourses,temp);
    }
 }
}
