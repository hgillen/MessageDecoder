using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using MessageDecoder.Models;

namespace MessageDecoder.ViewModels
{
    class StudentViewModel
    {

        public MyICommand DeleteCommand { get; set; }

        public StudentViewModel()
        {
            LoadStudents();
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
        }

        public ObservableCollection<Student> Students
        {
            get;
            set;
        }

        public void LoadStudents()
        {
            ObservableCollection<Student> students = new ObservableCollection<Student>();

            students.Add(new Student { FirstName = "Mark", LastName = "Allain" });
            students.Add(new Student { FirstName = "Allen", LastName = "Brown" });
            students.Add(new Student { FirstName = "Linda", LastName = "Litzke" });

            Students = students;
        }

        private Student _selectedStudent;

        public Student SelectedStudent
        {
            get
            {
                return _selectedStudent;
            }
            set
            {
                _selectedStudent = value;
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }


        private void OnDelete()
        {
            Students.Remove(SelectedStudent);
        }

        private bool CanDelete()
        {
            return SelectedStudent != null;
        }

    }
}
