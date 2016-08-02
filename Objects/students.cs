using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Register
{
  public class Student
  {
    private int _id;
    private string _name;
    private DateTime _date;
  public Student(string Name, DateTime Date, int Id = 0)
  {
    _id = Id;
    _name = Name;
    _date = Date;
  }

  public override bool Equals(System.Object otherStudent)
  {
    if(!(otherStudent is Student))
    {
      return false;
    }
    else
    {
      Student newStudent = (Student) otherStudent;
      bool idEquality = this.GetId() == newStudent.GetId();
      bool nameEquality = this.GetName() == newStudent.GetName();
      bool dateEquality = this.GetDate() == newStudent.GetDate();
      return (idEquality && nameEquality && dateEquality);
    }
  }
  public void Save()
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("INSERT INTO students (name, enroll_date) OUTPUT INSERTED.id VALUES (@Name, @Date);", conn);

    SqlParameter nameParameter = new SqlParameter();
    nameParameter.ParameterName = "@Name";
    nameParameter.Value = this.GetName();

    SqlParameter dateParameter = new SqlParameter();
    dateParameter.ParameterName = "@Date";
    dateParameter.Value = this.GetDate();

    cmd.Parameters.Add(nameParameter);
    cmd.Parameters.Add(dateParameter);

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
  public void AddCourses(Course newCourse)
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("INSERT INTO students_courses (student_id, course_id) VALUES (@StudentId,@CourseId)",conn);

    SqlParameter studentIdParameter = new SqlParameter();
    studentIdParameter.ParameterName= "@StudentId";
    studentIdParameter.Value= this.GetId();

    SqlParameter courseIdParameter = new SqlParameter();
    courseIdParameter.ParameterName= "@CourseId";
    courseIdParameter.Value= newCourse.GetId();

    cmd.Parameters.Add(studentIdParameter);
    cmd.Parameters.Add(courseIdParameter);

    cmd.ExecuteNonQuery();

    if(conn != null)
    {
      conn.Close();
    }
  }

  public List<Course> GetCourses()
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("SELECT courses.* FROM students JOIN students_courses ON (students.id=students_courses.student_id) JOIN courses ON (students_courses.id=courses.id) WHERE students.id=@StudentId;",conn);

    SqlParameter studentIdParameter = new SqlParameter();
    studentIdParameter.ParameterName="@StudentId";
    studentIdParameter.Value=this.GetId();

    cmd.Parameters.Add(studentIdParameter);
    SqlDataReader rdr = cmd.ExecuteReader();

    List<Course> foundCourses= new List<Course>{};

    while(rdr.Read())
    {
      int courseId=rdr.GetInt32(0);
      string courseName=rdr.GetString(1);
      int courseNumber=rdr.GetInt32(2);
      Course foundCourse = new Course(courseName,courseNumber,courseId);
      foundCourses.Add(foundCourse);
    }

    if(rdr != null)
    {
      rdr.Close();
    }

    if(conn !=null)
    {
      conn.Close();
    }
    return foundCourses;
  }




  public static Student Find(int id)
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentID;", conn);
    SqlParameter studentIdParameter = new SqlParameter();
    studentIdParameter.ParameterName = "@StudentID";
    studentIdParameter.Value = id.ToString();
    cmd.Parameters.Add(studentIdParameter);
    SqlDataReader rdr = cmd.ExecuteReader();

    int foundStudentId = 0;
    string foundNameStudent = null;
    DateTime foundDateStudent = new DateTime(2000,01,01);

    while (rdr.Read())
    {
      foundStudentId = rdr.GetInt32(0);
      foundNameStudent = rdr.GetString(1);
      foundDateStudent = rdr.GetDateTime(2);
    }
    Student foundStudent = new Student(foundNameStudent, foundDateStudent, foundStudentId);

    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }
    return foundStudent;
  }

  public int GetId()
  {
    return _id;
  }
  public string GetName()
  {
    return _name;
  }
  public void SetName(string newName)
  {
    _name = newName;
  }
  public DateTime GetDate()
  {
    return _date;
  }
  public void SetDate(DateTime newDate)
  {
    _date = newDate;
  }
  public static void DeleteAll()
  {
    SqlConnection conn = DB.Connection();
    conn.Open();
    SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
    cmd.ExecuteNonQuery();
    conn.Close();
  }
  public static List<Student> GetAll()
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
    SqlDataReader rdr = cmd.ExecuteReader();

    List<Student> AllStudents = new List<Student>{};

    while (rdr.Read())
    {
      int studentId = rdr.GetInt32(0);
      string studentName = rdr.GetString(1);
      DateTime studentDate = rdr.GetDateTime(2);
      Student newStudent = new Student(studentName,studentDate, studentId);
      AllStudents.Add(newStudent);
    }
    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }
    return AllStudents;
  }
}
}
