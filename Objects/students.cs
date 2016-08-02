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
