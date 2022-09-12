using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Calendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Student
        {
            public string name;
            public double payed;
            List<DateTime> breakList = new List<DateTime>();

            /// <summary>
            /// Constructor for Student object.
            /// </summary>
            /// <param name="name">Full name of student.</param>
            /// <param name="payed">Payed sum in HUF.</param>
            /// <param name="breakList">List of days not eating.</param>
            public Student(string name, double payed, List<DateTime> breakList)
            {
                this.name = name;
                this.payed = payed;
                this.breakList = breakList;
            }

            public void AddBreakDay(DateTime breakDay)
            {
                this.breakList.Add(breakDay);
            }

            public void RemoveBreakDay(DateTime breakDay)
            {
                this.breakList.Remove(breakDay);
            }
        }

        public class DataSet
        {
            List<Student> studentList = new List<Student>();
            List<DateTime> holidayList = new List<DateTime>();

            public DataSet(List<Student> studentList, List<DateTime> holidayList)
            {
                this.studentList = studentList;
                this.holidayList = holidayList;
            }

            public DataSet()
            {
                
            }


            public void AddHoliday(DateTime holiday)
            {
                this.holidayList.Add(holiday);
            }

            public void RemoveHoliday(DateTime holiday)
            {
                this.holidayList.Remove(holiday);
            }

            /// <summary>
            /// Add a student to the dataset.
            /// </summary>
            /// <param name="name">Full name of the student.</param>
            /// <param name="payed">Payed sum in HUF.</param>
            /// <param name="breaks">List of days not eating.</param>
            public void AddStudent(string name, double payed, List<DateTime> breaks)
            {
                this.studentList.Add(new Student(name, payed, breaks));
            }

            public void RemoveStudent(string name)
            {
                Student? removedStudent = this.studentList.Find(x => (x.name.Equals(name)));
                if(removedStudent != null)
                {
                    this.studentList.Remove(removedStudent);
                }
                else
                {
                    Debug.WriteLine("Student not found.");
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();


            
            
            

        }

        private void Calendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                CalendarItem c = sender as CalendarItem;
                c.FontSize = 50;
                Debug.WriteLine(sender);
                Debug.WriteLine(FindName("PART_HeaderButton"));
            }
        }

        private void Calendar_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.B)
            {
                return;
            }
     
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          

        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender.GetType().Equals(typeof(Button)))
            {
                Button g = (Button) sender;
                g.Visibility = Visibility.Hidden;
            }
            else if (sender.GetType().Equals(typeof(Grid)))
            {
                Grid g = (Grid) sender;
                g.Visibility = Visibility.Hidden;
            }
            
            //Button b = g.Parent as Button;
            //ContentPresenter? cp = g.Children[0] as ContentPresenter;
            //cp.Visibility = Visibility.Hidden;
            //cp.Content = "Kaki";
        

        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
          
        }

        private void n(object sender, KeyEventArgs e)
        {
         
        }

        private void Calendar_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Controls.Calendar cal = (System.Windows.Controls.Calendar) sender;
            //SelectedDatesCollection selectedDates = cal.SelectedDates;
            //cal.SelectedDates.Add(new DateTime(2022, 09, 16));
            cal.SelectionMode = CalendarSelectionMode.MultipleRange;
            cal.SelectedDates.AddRange(new DateTime(2022, 09, 5), new DateTime(2022, 09, 22));
            
            // Create Dataset
            DataSet ds = new DataSet();
            ds.AddStudent("Petike", 15000, new List<DateTime> { new DateTime(2022, 09, 13), new DateTime(2022, 09, 18), new DateTime(2022, 09, 19) });
            ds.RemoveStudent("Petike");



        }

        private void PART_MonthView_Loaded(object sender, RoutedEventArgs e)
        {
            Grid calendarDays = (Grid) sender;
            CalendarDayButton testDay = (CalendarDayButton) calendarDays.Children[10];
            //testDay.Visibility = Visibility.Hidden;
            testDay.Focus();
            //SelectedDatesCollection selectedDates;
            //selectedDates.Insert(new DateTime(2022, 09, 15));
            //testDay.SetValue(SelectedDatesCollection)
            
        }
    }

}
