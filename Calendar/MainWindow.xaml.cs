using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
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
        ListView payDatesListCtrl;
        enum EditMode {
            None,
            BreakDays,
            AddPayment,
            ModifyPayment,
            DeletePayment
        }
        EditMode editMode = EditMode.None;

        // Variables for sorting paylist ListView
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;


        public class Payment
        {
            public string payDateString { get; set; }
            public DateTime payDate { get; set; }
            public double paidAmount { get; set; }
            


            public Payment(DateTime payDate, double paidAmount)
            {
                this.payDate = payDate.Date;
                this.paidAmount = paidAmount;
                this.payDateString = payDate.Date.ToString("yyyy/MM/dd");
               
            }
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
                this.paymentList.Add(new Payment(new DateTime(2022, 07, 01), 30000));
                this.paymentList.Add(new Payment(new DateTime(2022, 08, 02), 30000));
                this.paymentList.Add(new Payment(new DateTime(2022, 09, 10), 15000));
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
                //return this.paid;
                return this.paymentList.Sum(x => x.paidAmount);
            }

            public void SetPaidAmount(double paidSum) //obsolete
            {
                this.paid = paidSum;
            }

            public Payment? GetFirstPayDate()
            {
                return this.paymentList.FirstOrDefault();
            }
            public void AddPayment(Payment payment)
            {
                this.paymentList.Add(payment);
            }

            public void ModifyPayment(Payment originalPayment, Payment newPayment)
            {
                Payment? modifiedPayment = this.paymentList.Find(x => (x.payDate.Equals(originalPayment.payDate)));
                if (modifiedPayment != null)
                {
                    if (modifiedPayment.paidAmount.Equals(originalPayment.paidAmount))
                    {
                        this.paymentList.Remove(originalPayment);
                        this.paymentList.Add(newPayment);
                    }
                    else { }
                }
                else
                {
                    Debug.WriteLine("Payment not found.");
                }
            }
            public void DeletePayment(Payment payment)
            {
                Payment? modifiedPayment = this.paymentList.Find(x => (x.payDate.Equals(payment.payDate)));
                if (modifiedPayment != null)
                {
                    if (modifiedPayment.paidAmount.Equals(payment.paidAmount))
                    {
                        this.paymentList.Remove(payment);
                        
                    }
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
            public void SetStudentPaidSum(string name, double paidSum) // obsolete
            {
                Student? foundStudent = this.studentList.Find(x => (x.name.Equals(name)));
                if (foundStudent != null)
                {
                    foundStudent.SetPaidAmount(paidSum);
                }
            }

            public void AddStudentPayment(string name, Payment payment) 
            {
                Student? foundStudent = this.studentList.Find(x => (x.name.Equals(name)));
                if (foundStudent != null)
                {
                    foundStudent.AddPayment(payment);
                }
            }

            public void ModifyStudentPayment(string name, Payment oldPayment, Payment newPayment)
            {
                Student? foundStudent = this.studentList.Find(x => (x.name.Equals(name)));
                if (foundStudent != null)
                {
                    foundStudent.ModifyPayment(oldPayment, newPayment);
                }
            }

            public void DeleteStudentPayment(string name, Payment payment)
            {
                Student? foundStudent = this.studentList.Find(x => (x.name.Equals(name)));
                if (foundStudent != null)
                {
                    foundStudent.DeletePayment(payment);
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

            var culture = new CultureInfo("hu-HU");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            
            
            

        }

        private void Calendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void Calendar_KeyUp(object sender, KeyEventArgs e)
        {
            
     
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          

        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
           
            
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
            payDatesListCtrl = (ListView)mainGrid.Children[9];


            


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
            else if (editMode.Equals(EditMode.AddPayment))
            {
                /*
                ds.SetStudentPaidSum(studentName.ToString(), Convert.ToDouble(payedAmountCtrl.Text));
                 */

                ds.AddStudentPayment(studentName.ToString(), new Payment(DateTime.Parse(payDateToSetCtrl.Text.ToString()), Convert.ToDouble(payedAmountCtrl.Text.ToString())));
                payedAmountCtrl.Visibility = Visibility.Hidden;
                payDateToSetCtrl.Visibility = Visibility.Hidden;
                RefreshDates();


            }
            else if (editMode.Equals(EditMode.ModifyPayment))
            {
                Payment newPayment = new Payment(DateTime.Parse(payDateToSetCtrl.Text.ToString()), Convert.ToDouble(payedAmountCtrl.Text.ToString()));
                Payment oldPayment = (Payment) payDatesCtrl.SelectedItem;

                ds.ModifyStudentPayment(studentName.ToString(), oldPayment, newPayment);

                payedAmountCtrl.Visibility = Visibility.Hidden;
                payDateToSetCtrl.Visibility = Visibility.Hidden;
                RefreshDates();
            }
            else if (editMode.Equals(EditMode.DeletePayment))
            {
                Payment paymentToDelete = (Payment)payDatesCtrl.SelectedItem;
                ds.DeleteStudentPayment(studentName.ToString(), paymentToDelete);

                payDatesCtrl.ItemContainerStyle = (Style)this.FindResource("dateHighlightStyleNormal");
                RefreshDates();
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

            DateTime firstPayDate = ds.GetStudent(studentName.ToString()).GetFirstPayDate().payDate;
            DateTime endDate = firstPayDate.AddDays(remainingDays-1);

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

            // Calculate account for selected month
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);


            for (int i=0; i<remainingDays; i++)
            {
                DateTime dayToHighlight = firstPayDate.AddDays(i);
                
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
                    else
                    {
                        calCtrl.SelectedDates.Add(dayToHighlight); // Highlight normal payed day
                        
                    }
                    
                }
            }


        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) // List payments of selected student
        {
            string selectedStudent = (string)studentListCtrl.SelectedItem.ToString(); // Selected Item cannot be null;
            List<Payment> paymentList = ds.GetStudent(selectedStudent).GetPaymentList();

            payDatesListCtrl.ItemsSource = paymentList;

            
            /*
            payDatesListCtrl.Items.Clear();
            foreach (Payment payment in paymentList)
            {
                payDatesListCtrl.Items.Add(payment);
            }

            var test = payDatesListCtrl.Items[2];
            */
            
      

            /*
            this.payDatesTableCtrl.View = System.Windows.Forms
            payDatesTableCtrl.Columns.Add("1st column", 75, HorizontalAlignment.Left);
            foreach (Payment payment in paymentList){
                ListViewItem item = new ListViewItem();
                item.Subitem
                payDatesTableCtrl.Items.Add(item);
            }
            */

        }

        private void addPaymentCtrl_Click(object sender, RoutedEventArgs e)
        {
            payedAmountCtrl.Visibility = Visibility.Visible;
            payDateToSetCtrl.Visibility = Visibility.Visible;
            payedAmountCtrl.Clear();
            payDateToSetCtrl.Clear();
            editMode = EditMode.AddPayment;

          
        }

        private void payDatesCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            //ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(payDatesListCtrl.ItemsSource);

        }

        private void RefreshDates()
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(payDatesListCtrl.ItemsSource);
            dataView.Refresh();
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(payDatesListCtrl.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        void payDatesCtrlColumnHeaderClicked(object sender,
                                            RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void editPaymentCtrl_Click(object sender, RoutedEventArgs e)
        {
            payedAmountCtrl.Visibility = Visibility.Visible;
            payDateToSetCtrl.Visibility = Visibility.Visible;
            
            Payment? selectedPayment = (Payment)payDatesCtrl.SelectedItem;
            if(selectedPayment != null)
            {
                payedAmountCtrl.Text = selectedPayment.paidAmount.ToString();
                payDateToSetCtrl.Text = selectedPayment.payDateString;

            }


            editMode = EditMode.ModifyPayment;
        }

        private void deletePaymentCtrl_Click(object sender, RoutedEventArgs e)
        {
            editMode = EditMode.DeletePayment;
            payDatesCtrl.ItemContainerStyle = (Style)this.FindResource("dateHighlightStyleDelete");
        }
    }

}
