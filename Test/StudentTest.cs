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
     Student.DeleteAll();
   }

  }
}
