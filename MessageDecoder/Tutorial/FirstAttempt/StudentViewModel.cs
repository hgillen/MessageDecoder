using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using MessageDecoder.Models;

namespace MessageDecoder.ViewModels
{
   public class StudentViewModel
   {
      public StudentViewModel()
      {
         LoadStudents();
      }

      public ObservableCollection<Student> Students
      {
         get;
         set;
      }

      public void LoadStudents()
      {
         ObservableCollection<Student> students = new ObservableCollection<Student>();

         students.Add(new Student { FirstName = "Alana", LastName = "Artex" });
         students.Add(new Student { FirstName = "Brent", LastName = "Berkins" });
         students.Add(new Student { FirstName = "Charlie", LastName = "Clanaugh" });

         Students = students;
      }


   }
}
