using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
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
        // Create Dataset
        DataSet ds = new DataSet();
        Grid mainGrid;
        ListView studentListCtrl;
        System.Windows.Controls.Calendar calCtrl;
        TextBox payedAmountCtrl;
        enum EditMode {
            None,
            BreakDays,
            PayedAmount
        }
        EditMode editMode = EditMode.None;

        public class Payment
        {
            public DateTime payDate;
            public double paidAmount;

        }


        public class Student
        {
            public string name;
            public double paid;
            public DateTime firstPayDate;
            List<DateTime> breakList = new List<DateTime>();
            List<Payment> paymentList = new List<Payment>();


            /// <summary>
            /// Constructor for Student object.
            /// </summary>
            /// <param name="name">Full name of student.</param>
            /// <param name="payed">Payed sum in HUF.</param>
            /// <param name="breakList">List of days not eating.</param>
            public Student(string name, double paid, DateTime payDate, List<DateTime> breakList)
            {
                this.name = name;
                this.paid = paid;
                this.firstPayDate = payDate;
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

            public List<DateTime>? GetDateTimes()
            {
                return this.breakList;
            }

            public void SetBreakList(List<DateTime> breakList)
            {
                this.breakList = breakList;
            }
            public double GetPayedAmount()
            {
                return this.paid;
            }

            public void SetPaidAmount(double paidSum)
            {
                this.paid = paidSum;
            }

            public DateTime GetFirstPayDate()
            {
                return this.firstPayDate;
            }
            public void AddPayment(Payment payment)
            {
                this.paymentList.Add(payment);
            }

            public void ModifyPayment(Payment payment)
            {
                Payment? modifiedPayment = this.paymentList.Find(x => (x.payDate.Equals(payment.payDate)));
                if (modifiedPayment != null)
                {
                    if (modifiedPayment.paidAmount.Equals(payment.paidAmount))
                        this.paymentList.Remove(payment);
                    else { }
                }
                else
                {
                    Debug.WriteLine("Payment not found.");
                }
            }

            public List<Payment> GetPaymentList()
            {
                return this.paymentList;
            }

        }

        public class DataSet
        {
            List<Student> studentList = new List<Student>();
            List<DateTime> holidayList = new List<DateTime>();
            double mealPrice;

            public DataSet(List<Student> studentList, List<DateTime> holidayList, double mealPrice)
            {
                this.studentList = studentList;
                this.holidayList = holidayList;
                this.mealPrice = mealPrice; 
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
            public void AddStudent(string name, double payed, DateTime payDate, List<DateTime> breaks)
            {
                this.studentList.Add(new Student(name, payed, payDate, breaks));
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

            public Student? GetStudent(string name)
            {
                Student? foundStudent = this.studentList.Find(x => (x.name.Equals(name)));
                return foundStudent;
            }
            public List<Student>? GetStudents()
            {
                return this.studentList;
            }

            public void SetStudentBreaks(string name, List<DateTime> breakList)
            {
                Student? foundStudent = this.studentList.Find(x => (x.name.Equals(name)));
                if (foundStudent != null)
                {
                    foundStudent.SetBreakList(breakList);
                }
            }
            public void SetStudentPaidSum(string name, double paidSum)
            {
                Student? foundStudent = this.studentList.Find(x => (x.name.Equals(name)));
                if (foundStudent != null)
                {
                    foundStudent.SetPaidAmount(paidSum);
                }
            }

            public void SetMealPrice(double price)
            {
                this.mealPrice = price;
            }

            public double GetMealPrice()
            {
                return this.mealPrice;
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
            // Save references to most used controls.
            calCtrl = (System.Windows.Controls.Calendar) sender;
            mainGrid = (Grid)calCtrl.Parent;
            studentListCtrl = (ListView)mainGrid.Children[3];
            payedAmountCtrl = (TextBox)mainGrid.Children[8];



            //SelectedDatesCollection selectedDates = cal.SelectedDates;
            //cal.SelectedDates.Add(new DateTime(2022, 09, 16));
            calCtrl.SelectionMode = CalendarSelectionMode.MultipleRange;
            //cal.SelectedDates.AddRange(new DateTime(2022, 09, 5), new DateTime(2022, 09, 22));
            
            // Fill Dataset
            
            ds.AddStudent("Petike", 15000, new DateTime(2022,09,01), new List<DateTime> { new DateTime(2022, 09, 2), new DateTime(2022, 09, 6), new DateTime(2022, 09, 19) });
            ds.AddStudent("Janika", 3000, new DateTime(2022, 09, 02), new List<DateTime> { new DateTime(2022, 09, 01), new DateTime(2022, 09, 02), new DateTime(2022, 09, 03) });
            ds.AddStudent("Józsika", 9000, new DateTime(2022, 09, 03), new List<DateTime> { new DateTime(2022, 09, 10), new DateTime(2022, 09, 11), new DateTime(2022, 09, 14) });
            ds.AddStudent("Gerzsonka", 7777, new DateTime(2022, 09, 04), new List<DateTime> { new DateTime(2022, 09, 20), new DateTime(2022, 09, 21), new DateTime(2022, 09, 30) });
            ds.SetMealPrice(1500);
            //ds.RemoveStudent("Petike");



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

    
        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            //ListView studentListCtrl = (ListView)sender;
            
            foreach (Student student in ds.GetStudents())
            {
                studentListCtrl.Items.Add(student.name);
            }
            
        }

        private void breakSelectCtrl_Click(object sender, RoutedEventArgs e)
        {
            calCtrl.SelectedDates.Clear();
            calCtrl.BlackoutDates.Clear();
            editMode = EditMode.BreakDays;
            

            TextBox inputNameCtrl = (TextBox)mainGrid.Children[2];
            calCtrl.SelectedDates.Clear();
            calCtrl.BlackoutDates.Clear();




            if (studentListCtrl.SelectedItem != null)
            {
                calCtrl.CalendarDayButtonStyle = (Style)Resources["AlternativeCalendarDayButtonStyle"];
                foreach (DateTime breakDay in ds.GetStudent(studentListCtrl.SelectedItem.ToString()).GetDateTimes())
                {
                    calCtrl.SelectedDates.Add(breakDay);

                }

                /*List<DateTime> breakDaysList = (List < DateTime >) ds.GetStudent(studentListCtrl.SelectedItem.ToString()).GetDateTimes();
                CalendarDateRange calBlackoutRange = new CalendarDateRange(breakDaysList[0], breakDaysList[breakDaysList.Count-1]);
                calCtrl.BlackoutDates.Add(calBlackoutRange);*/
            }
        }

        private void doneCtrl_Click(object sender, RoutedEventArgs e)
        {
            var studentName = studentListCtrl.SelectedItem;
            if (studentName == null)
            {
                // TODO: message to user to select student
                return;
            }

            
            if (editMode.Equals(EditMode.BreakDays))
            {
                SelectedDatesCollection breakList = (SelectedDatesCollection)calCtrl.SelectedDates;
                List<DateTime> breaks = breakList.ToList();

                ds.SetStudentBreaks(studentName.ToString(), breaks);
                
            }
            else if (editMode.Equals(EditMode.PayedAmount))
            {
                ds.SetStudentPaidSum(studentName.ToString(), Convert.ToDouble(payedAmountCtrl.Text));
                payedAmountCtrl.Visibility = Visibility.Hidden;

            }
            
            
        }

        private void showBreaksCtrl_Click(object sender, RoutedEventArgs e)
        {
            Button buttonShowCal = (Button)sender;
            //Grid mainGrid = (Grid) buttonShowCal.Parent;

            TextBox inputNameCtrl = (TextBox)mainGrid.Children[2];
            calCtrl.SelectedDates.Clear();
            calCtrl.BlackoutDates.Clear();
            
  
            

            if(studentListCtrl.SelectedItem != null)
            {
                calCtrl.CalendarDayButtonStyle = (Style)Resources["AlternativeCalendarDayButtonStyle"];
                foreach (DateTime breakDay in ds.GetStudent(studentListCtrl.SelectedItem.ToString()).GetDateTimes())
                {
                    calCtrl.SelectedDates.Add(breakDay);
                    
                }
                
                /*List<DateTime> breakDaysList = (List < DateTime >) ds.GetStudent(studentListCtrl.SelectedItem.ToString()).GetDateTimes();
                CalendarDateRange calBlackoutRange = new CalendarDateRange(breakDaysList[0], breakDaysList[breakDaysList.Count-1]);
                calCtrl.BlackoutDates.Add(calBlackoutRange);*/
            }
            


        }

        private void showPayedDaysCtrl_Click(object sender, RoutedEventArgs e)
        {
            var studentName = studentListCtrl.SelectedItem;
            if (studentName == null)
            {
                // TODO: message to user to select student
                return;
            }
            //TODO: skip weekends and break days
            calCtrl.SelectedDates.Clear();
            calCtrl.BlackoutDates.Clear();
            calCtrl.CalendarDayButtonStyle = (Style)Resources["CalendarCalendarDayButtonStyle1"];
            double payedAmount = ds.GetStudent(studentName.ToString()).GetPayedAmount();
            int remainingDays = (int) Math.Floor(payedAmount / ds.GetMealPrice());

            DateTime payDate = ds.GetStudent(studentName.ToString()).GetFirstPayDate();
            DateTime endDate = payDate.AddDays(remainingDays-1);

            /*
            // Skipping breakdays
            DateTime startDate = payDate;
            foreach (DateTime breakDay in ds.GetStudent(studentName.ToString()).GetDateTimes())
            {
                CalendarDateRange calBlackoutRange = new CalendarDateRange(breakDay, breakDay);
                calCtrl.BlackoutDates.Add(calBlackoutRange);
                if (breakDay < endDate)
                {
                    endDate = endDate.AddDays(1);
                    calCtrl.SelectedDates.AddRange(startDate, breakDay.AddDays(-1));
                    startDate = breakDay.AddDays(1);
                }
                
            }
            calCtrl.SelectedDates.AddRange(startDate, endDate);
            */
            //calCtrl.SelectedDates.AddRange(payDate, endDate);

            // New method
            // List breakDays
            // TODO: szombati munkanapok speckó kezelése
            List<DateTime> breakDayList = new List<DateTime>();
            foreach (DateTime breakDay in ds.GetStudent(studentName.ToString()).GetDateTimes())
            {
                breakDayList.Add(breakDay);

            }
            for(int i=0; i<remainingDays; i++)
            {
                DateTime dayToHighlight = new DateTime(payDate.Year, payDate.Month, payDate.Day + i);
                if(dayToHighlight.DayOfWeek == DayOfWeek.Sunday || dayToHighlight.DayOfWeek == DayOfWeek.Saturday) // Skip weekends
                {
                    remainingDays++;
                }
                else
                {
                    if (breakDayList.Find(x => x.Date.Equals(dayToHighlight.Date)) != new DateTime()) // Skip breakdays
                    {
                        remainingDays++;
                        CalendarDateRange calBlackoutRange = new CalendarDateRange(dayToHighlight, dayToHighlight);
                        calCtrl.BlackoutDates.Add(calBlackoutRange);
                    }
                    else calCtrl.SelectedDates.Add(dayToHighlight); // Highlight normal payed day
                    
                }
            }


        }

        private void setPayedAmountCtrl_Click(object sender, RoutedEventArgs e)
        {
            payedAmountCtrl.Visibility = Visibility.Visible;
            // TODO: Input filter, that only numbers are allowed.
            editMode = EditMode.PayedAmount;
            // TODO: Clear ListView selection .
        }
    }

}
