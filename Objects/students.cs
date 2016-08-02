using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Register
{
  public class Student
  {
    private int _id;
    private string _name;
    private int _date;

  public Student(string Name, int Date, int Id = 0)
  {
    _id = Id;
    _name = Name;
    _date = Date;
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
  public int GetDate()
  {
    return _date;
  }
  public void SetDate(int newDate)
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
}
}
