using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Register
{
  public class Course
  {
    private int _id;
    private string _name;
    private int _number;
    public Course(string Name, int Number, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _number = Number;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string NewName)
    {
      _name = NewName;
    }
    public int GetNumber()
    {
      return _number;
    }
    public void SetNumber(int newNumber)
    {
      _number = newNumber;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO courses (name, course_number) OUTPUT INSERTED.id VALUES (@Name, @Number);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@Name";
      nameParameter.Value = this.GetName();

      SqlParameter courseNumberParameter = new SqlParameter();
      courseNumberParameter.ParameterName = "@Number";
      courseNumberParameter.Value = this.GetNumber();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(courseNumberParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        this._id =rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM courses;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
    public override bool Equals(System.Object otherCourse)
    {
      if(!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = this.GetId() == newCourse.GetId();
        bool nameEquality = this.GetName() == newCourse.GetName();
        bool numberEquality = this.GetNumber() == newCourse.GetNumber();
        return (idEquality && nameEquality && numberEquality);
      }
    }

    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses;",conn);

      SqlDataReader rdr = cmd.ExecuteReader();

      int courseId=0;
      string courseName=null;
      int courseNumber=0;

      while(rdr.Read())
      {
        courseId = rdr.GetInt32(0);
        courseName=rdr.GetString(1);
        courseNumber=rdr.GetInt32(2);

        Course newCourse = new Course(courseName,courseNumber,courseId);
        allCourses.Add(newCourse);
      }

      if( rdr!=null)
      {
        rdr.Close();
      }

      if(conn != null)
      {
        conn.Close();
      }

      return allCourses;
    }

    public void AddStudents(Student newStudent)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students_courses (student_id, course_id) VALUES (@StudentId,@CourseId)",conn);

      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName= "@StudentId";
      studentIdParameter.Value= newStudent.GetId();

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName= "@CourseId";
      courseIdParameter.Value= this.GetId();

      cmd.Parameters.Add(studentIdParameter);
      cmd.Parameters.Add(courseIdParameter);

      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }
    public List<Student> GetStudents()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT students.* FROM courses JOIN students_courses ON (courses.id = students_courses.course_id) JOIN students ON (students_courses.student_id = students.id) WHERE courses.id = @CourseId;", conn);
      SqlParameter CourseIdParameter = new SqlParameter();
      CourseIdParameter.ParameterName = "@CourseId";
      CourseIdParameter.Value = this.GetId().ToString();

      cmd.Parameters.Add(CourseIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Student> students = new List<Student>{};

      while (rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        DateTime studentDate = rdr.GetDateTime(2);
        Student newStudent = new Student(studentName, studentDate, studentId);
        students.Add(newStudent);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return students;
    }

    public static Course Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses WHERE id = @NewId;",conn);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@NewId";
      courseIdParameter.Value=id;
      cmd.Parameters.Add(courseIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundId=0;
      string foundName=null;
      int foundNumber=0;

      while(rdr.Read())
      {
        foundId=rdr.GetInt32(0);
        foundName=rdr.GetString(1);
        foundNumber=rdr.GetInt32(2);

      }
      Course foundCourse = new Course(foundName,foundNumber,foundId);

      if(rdr != null)
      {
        rdr.Close();
      }

      if(conn != null)
      {
        conn.Close();
      }
      return foundCourse;
    }

  }
}
