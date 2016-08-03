using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using System;
namespace Register
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ =>{
        List<Student> allStudents = Student.GetAll();
        List<Course> allCourses = Course.GetAll();

        Dictionary<string,object> model = new Dictionary<string,object>{};
        model.Add("student",allStudents);
        model.Add("course",allCourses);
        return View["index.cshtml",model];
      };

      Post["/add/new/student"]=_=>{
        Student newStudent = new Student(Request.Form["new-name"], Request.Form["new-date"]);
        newStudent.Save();

        List<Student> allStudents = Student.GetAll();
        List<Course> allCourses = Course.GetAll();

        Dictionary<string,object> model = new Dictionary<string,object>{};
        model.Add("student",allStudents);
        model.Add("course",allCourses);
        return View["index.cshtml",model];
      };
      Post["/add/new/course"]=_=>{
        Course newCourse = new Course(Request.Form["course-name"], Request.Form["course-number"]);
        newCourse.Save();

        List<Student> allStudents = Student.GetAll();
        List<Course> allCourses = Course.GetAll();

        Dictionary<string,object> model = new Dictionary<string,object>{};
        model.Add("student",allStudents);
        model.Add("course",allCourses);
        return View["index.cshtml",model];
      };
    }
  }
}
