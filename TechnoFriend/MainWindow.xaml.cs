using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TechnoFriend.IRepository;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;
using ProgressBar = System.Windows.Controls.ProgressBar;
using TextBox = System.Windows.Controls.TextBox;
using ToolBar = System.Windows.Controls.ToolBar;

namespace TechnoFriend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataClasses1DataContext d;
        public static bool startpressed = false;
        ToolBar tb = null;


        public static bool stopEnabled = false;//used to disable stop functions

        public static void enableStart(ToolBar tb)
        {
            //Disable Stop
            (tb.Items[0] as Button).IsEnabled = false;

            //Enable Start
            (tb.Items[1] as Button).IsEnabled = true;
            //Press Color
            (tb.Items[1] as Button).Background = Brushes.Black;
        }

        public static void enableStop(ToolBar tb)
        {
            //Enable Stop
            (tb.Items[1] as Button).IsEnabled = false;

            //Enable Start
            (tb.Items[0] as Button).IsEnabled = true;
            //UnColor
            (tb.Items[1] as Button).Background = Brushes.Red;
        }



        public static ConcurrentDictionary<string, taskWork> tasks;
        public static ConcurrentDictionary<string, taskWork> runningArray;
        public static ConcurrentDictionary<string, taskWork> pendingArray;

        public MainWindow()
        {


            //try
            //{
            //DateTime dt = new DateTime(2019, 12, 23);
            //if (DateTime.Compare(DateTime.Now, dt) > 0)
            //{
            //    this.Close();

            //}
            InitializeComponent();
            d = new DataClasses1DataContext();
            define_list.ItemsSource = from m in d.Defines select m;
            tasks = new ConcurrentDictionary<string, taskWork>();
            runningArray = new ConcurrentDictionary<string, taskWork>();
            pendingArray = new ConcurrentDictionary<string, taskWork>();

            statustab.Visibility = Visibility.Hidden;
            definetab.Visibility = Visibility.Hidden;
            settingstab.Visibility = Visibility.Hidden;
            schedtab.Visibility = Visibility.Hidden;
            schedcombo.SelectedIndex = 2; //load the comboBox with the last item

            //}
            //catch (Exception tr)
            //{ MessageBox.Show(tr.Message); }


            #region section 1
            //Add all the already defined servers to the tasks dictionary
            foreach (Define def in define_list.Items)
            {
                taskWork ts = new taskWork();
                tasks.TryAdd(def.Id.ToString(), ts);
            }

            th.SetApartmentState(ApartmentState.STA);
            #endregion


        }



        public static ConcurrentDictionary<int, ProgressBar> progressBars = new ConcurrentDictionary<int, ProgressBar>();
        #region LoadStatuses
        public void LoadStatuses()
        {
            //status_list.ItemsSource = d.Defines.ToList();

        }
        #endregion



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Define de = new Define();
                List<int> position = (from m in d.Defines select m.Id).ToList<int>();
                position.Sort();
                int k = position[position.Count - 1];

                de.Server_Name = k++.ToString();
                de.isStarted = false;
                de.isChecked = false;
                de.isChecked = false;
                de.progress = 0.0;

                d.Defines.InsertOnSubmit(de);
                d.SubmitChanges();
                define_list.ItemsSource = from m in d.Defines select m;
                //Create the new task
                taskWork mytask = new taskWork();
                //add the new created task to the dictionary
                tasks.TryAdd(de.Id.ToString(), mytask);

            }
            catch (Exception gg)
            {
                MessageBox.Show(gg.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Select the focused on item
            DependencyObject parent = VisualTreeHelper.GetParent(sender as Button);
            while (!(parent is ListBoxItem))
                parent = VisualTreeHelper.GetParent(parent);
            ListBoxItem lbi = parent as ListBoxItem;
            lbi.IsSelected = true;



        }



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {



            #region  Select the focused on item
            DependencyObject parent = VisualTreeHelper.GetParent(sender as Button);
            while (!(parent is ListBoxItem))
                parent = VisualTreeHelper.GetParent(parent);
            ListBoxItem lbi = parent as ListBoxItem;
            lbi.IsSelected = true;
            #endregion

            #region Delete it from tasks
            taskWork del;
            tasks[(define_list.SelectedItem as Define).Id.ToString()].Close();
            tasks.TryRemove((define_list.SelectedItem as Define).Server, out del);
            #endregion

            #region Delete it from database first
            d.Defines.DeleteOnSubmit(define_list.SelectedItem as Define);
            d.SubmitChanges();
            #endregion

            #region Delete it from the list
            define_list.ItemsSource = null;
            define_list.ItemsSource = from k in d.Defines select k;
            #endregion




        }




        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (statustab.IsSelected && startpressed == false)
            //    Dispatcher.BeginInvoke((Action)(() => LoadStatuses()));
            if (definetab.IsSelected)
            {
                noofitems.Content = define_list.Items.Count.ToString();
            }
        }


        #region Find Child
        private ChildControl FindVisualChild<ChildControl>(DependencyObject DependencyObj)
        where ChildControl : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
            {
                DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

                if (Child != null && Child is ChildControl)
                {
                    return (ChildControl)Child;
                }
                else
                {
                    ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child);

                    if (ChildOfChild != null)
                    {
                        return ChildOfChild;
                    }
                }
            }
            return null;
        }
        #endregion

        #region Find Parent ListBoxItem
        private ListBoxItem GetParent(object sender)
        {
            DependencyObject parent;

            //Get the parent of that control
            parent = VisualTreeHelper.GetParent(sender as Button);
            while (!(parent is ListBoxItem))
                parent = VisualTreeHelper.GetParent(parent);
            ListBoxItem lbi = parent as ListBoxItem;

            return lbi;
        }
        #endregion

        #region Find Parent ProgressBar
        private ListBoxItem GetParentPB(object sender)
        {
            DependencyObject parent;

            //Get the parent of that control
            parent = VisualTreeHelper.GetParent(sender as ProgressBar);
            while (!(parent is ListBoxItem))
                parent = VisualTreeHelper.GetParent(parent);
            ListBoxItem lbi = parent as ListBoxItem;

            return lbi;
        }
        #endregion


        #region Find Custom Parent
        private ListBoxItem GetCustomParent(object sender)
        {
            DependencyObject parent;

            //Get the parent of that control
            parent = VisualTreeHelper.GetParent(sender as TextBox);
            while (!(parent is ListBoxItem))
                parent = VisualTreeHelper.GetParent(parent);
            ListBoxItem lbi = parent as ListBoxItem;

            return lbi;
        }
        #endregion




        public static string ServerPasser = "";


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            //Progress bar pointer
            ProgressBar prg = null;

            //Object to represent the selected item
            Define temp = null;
            //To protect from other events
            startpressed = true;

            //Toolbar pointer (to access buttons)


            //  Task.Factory.StartNew(() =>//Begining of the main task
            //{
            #region Subtask1 to initiate variables
            //Task subTask1 = Task.Factory.StartNew(() =>
            //  {
            //      Dispatcher.Invoke(() =>
            // {
            //     ListBoxItem lbi = GetParent(sender);
            //     lbi.IsSelected = true;

            //     //Get the selected item info and start the downloading process (from status because it is the last version of info)
            //     Define def = (status_list.SelectedItem as Define);

            //     //Now get the Child (Control)
            //     prg = FindVisualChild<ProgressBar>(lbi);
            //     temp = def;


            // });//End of dispatcher
            //  });
            #endregion End Subtask1

            //Task.WaitAll(new Task[] { subTask1 });

            #region  Subtask2 starting the subtask
            Task subTask2 = Task.Factory.StartNew(() =>
             {



                 //Get the ListBoxItem (the parent of the start button)
                 Dispatcher.Invoke(() =>
                 {
                     ListBoxItem lbi = GetParent(sender);
                     lbi.IsSelected = true;

                     //Get the selected item info and start the downloading process (from status because it is the last version of info)
                     Define def = (status_list.SelectedItem as Define);
                     //Set as started
                     def.isStarted = true;
                     d.SubmitChanges();
                     //Now get the Child (Control)
                     prg = FindVisualChild<ProgressBar>(lbi);
                     temp = def;

                     //Get the ToolBar that contains the button, to get reference on start and stop buttons
                     //Now get the Child (Control)
                     tb = FindVisualChild<ToolBar>(lbi);

                     enableStop(tb);
                     stopEnabled = true;

                 });//End of dispatcher


                 #region Add and Start
                 //Add the task to the dictionary and start it
                 try
                 {
                     tasks[temp.Id.ToString()].Start(temp, ref prg, prg.Dispatcher, "internal");
                 }
                 catch (System.Collections.Generic.KeyNotFoundException notfoundEx)
                 {
                     //Create the new task
                     taskWork mytask = new taskWork();

                     //add the new created task to the dictionary
                     tasks.TryAdd(temp.Id.ToString(), mytask);


                     //Start it
                     tasks[temp.Id.ToString()].Start(temp, ref prg, prg.Dispatcher, "internal");


                 }
                 #endregion

                 startpressed = false;

             }
                  );//End of subtask2

            #endregion // End of subtask

            //}); // End of the main Task

        }

        private int IfPartialDownload()
        {

            return 0;
        }


        private void Password_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
        e.Command == ApplicationCommands.Cut ||
        e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }


        private void TabLine(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            tabline.Margin = new Thickness(10 + (70 * index), 0, 0, -30);

            if ((sender as Button).Uid.ToString() == "0")
            {
                statustab.IsSelected = true;
            }
            else if ((sender as Button).Uid.ToString() == "1")
            {
                definetab.IsSelected = true;
            }
            else if ((sender as Button).Uid.ToString() == "2")
            {
                settingstab.IsSelected = true;
            }
            else if ((sender as Button).Uid.ToString() == "3")
            {
                schedtab.IsSelected = true;
            }

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Environment.Exit(0);
        }

        private void Window1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception fa)
            { }
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            //Get the ListBoxItem (the parent of the start button)
            Dispatcher.Invoke(() =>
            {
                ListBoxItem lbi = GetParent(sender);
                lbi.IsSelected = true;

                //Get the ToolBar that contains the button, to get reference on stop button
                //Now get the Child (Control)
                tb = FindVisualChild<ToolBar>(lbi);

                if ((tb.Items[1] as Button).IsEnabled != true || (tb.Items[1] != null))
                    enableStart(tb);
                stopEnabled = false;

            });//End of dispatcher


            //if (stopEnabled == true)
            try
            {
                taskWork t = tasks[(status_list.SelectedItem as Define).Id.ToString()];
                t.Cancel();
                (status_list.SelectedItem as Define).isStarted = false;
                tasks.TryRemove((status_list.SelectedItem as Define).Id.ToString(), out var none);

            }
            catch (Exception fff) { MessageBox.Show(fff.Message); }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (Define d in define_list.Items)
            {
                d.Speed = Convert.ToInt32(speedTextBox.Text);

            }
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            if (startSched == true)
                (sender as Button).IsEnabled = false;

            //(sender as Button).IsEnabled = false;
            ListBoxItem lbi = GetParent(sender);
            lbi.IsSelected = true;
            Define def = (status_list.SelectedItem as Define);
            if (status_list.SelectedItem != null)
            {
                if (def.isStarted == true)
                {
                    (sender as Button).Background = Brushes.Black;
                    (sender as Button).IsEnabled = true;
                }
            }


        }

        private void Button_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (startSched == true)
                (sender as Button).IsEnabled = false;

            ListBoxItem lbi = GetParent(sender);
            lbi.IsSelected = true;
            Define def = (status_list.SelectedItem as Define);
            if (status_list.SelectedItem != null)
                if (def.isStarted == true)
                {
                    (sender as Button).Background = Brushes.Red;
                    (sender as Button).IsEnabled = false;
                }


        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //try
            //{
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();
            //SettingsTable dbpath = (from k in d.SettingsTables
            //                 select k).Single();

            var dbpath = (from k in d.SettingsTables
                          select k).Single();
            d.SettingsTables.DeleteOnSubmit(dbpath);
            d.SubmitChanges();
            SettingsTable st = new SettingsTable();
            taskWork.filesPath = path.Text = st.path = fbd.SelectedPath;
            d.SettingsTables.InsertOnSubmit(st);
            d.SubmitChanges();


            //}
            //catch (Exception gg)
            //{ MessageBox.Show(gg.Message); }


        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (schedcombo.SelectedIndex == 0)
            {
                fromtb.Text = "First ";
                totb.Visibility = Visibility.Hidden;
                totx.Visibility = Visibility.Hidden;
            }
            else if (schedcombo.SelectedIndex == 1)
            {
                fromtb.Text = "Last ";
                totb.Visibility = Visibility.Hidden;
                totx.Visibility = Visibility.Hidden;
            }
            else
            {
                fromtb.Text = "From ";
                totb.Visibility = Visibility.Visible;
                totx.Visibility = Visibility.Visible;
            }
        }

        private void Path_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Path_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                path.Text = (from k in d.SettingsTables
                             select k.path).Single();
            }
            catch (Exception ff)
            {


            }

        }

        private void Port_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListBoxItem parent = GetCustomParent(sender);
            parent.IsSelected = true;
            Define f = define_list.SelectedItem as Define;
            f.Port = Convert.ToInt32((sender as TextBox).Text);
            d.SubmitChanges();
        }

        private void Server_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListBoxItem parent = GetCustomParent(sender);
            parent.IsSelected = true;
            Define f = define_list.SelectedItem as Define;
            f.Server_Name = (sender as TextBox).Text;
            d.SubmitChanges();
        }

        private void Server_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListBoxItem parent = GetCustomParent(sender);
            parent.IsSelected = true;
            Define f = define_list.SelectedItem as Define;
            f.Server = (sender as TextBox).Text;
            d.SubmitChanges();
        }

        private void File_path_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListBoxItem parent = GetCustomParent(sender);
            parent.IsSelected = true;
            Define f = define_list.SelectedItem as Define;
            f.File_Path = (sender as TextBox).Text;
            d.SubmitChanges();
        }

        private void Username_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListBoxItem parent = GetCustomParent(sender);
            parent.IsSelected = true;
            Define f = define_list.SelectedItem as Define;
            f.Username = (sender as TextBox).Text;
            d.SubmitChanges();
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListBoxItem parent = GetCustomParent(sender);
            parent.IsSelected = true;
            Define f = define_list.SelectedItem as Define;
            f.Password = (sender as TextBox).Text;
            d.SubmitChanges();
        }

        private void Speed_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListBoxItem parent = GetCustomParent(sender);
            parent.IsSelected = true;
            Define f = define_list.SelectedItem as Define;
            f.Speed = Convert.ToInt32((sender as TextBox).Text);
            d.SubmitChanges();
        }

        private void Tasksselector_Loaded(object sender, RoutedEventArgs e)
        {
            tasksselector.ItemsSource = d.Defines.ToList();
            selectall.IsChecked = true;//This line here because the listBox (TasksSelector) should load items first because IsChecked will triggrt the Checked event that needs the listBox to be loaded first

        }

        private void Selectall_Checked(object sender, RoutedEventArgs e)
        {
            selection.IsEnabled = false;
            foreach (Define c in tasksselector.ItemsSource)
                c.isChecked = true;
        }

        private void Selectall_Unchecked(object sender, RoutedEventArgs e)
        {
            selection.IsEnabled = true;
            foreach (Define c in tasksselector.ItemsSource)
                c.isChecked = false;
        }


        public static bool startSched = false;
        public static bool cancelSched = false;
        public static DataClasses1DataContext staticd = new DataClasses1DataContext();
        public static Thread th = new Thread(new ParameterizedThreadStart(startSchedular));



        public static void startSchedular(object ll)
        {
            System.Windows.Controls.ListBox l = ll as System.Windows.Controls.ListBox;//it is one way to pass reference to the listBox items which are Define, that any change will reflect directly to the listboxitem (bound textbox)

            //DataClasses1DataContext d = new DataClasses1DataConte0xt();
            ProgressBar garbage = new ProgressBar();


            #region section 2
            startSched = true;
            //Run code over all the selected items
            foreach (Define def in l.Items)
            {
                if (def.isChecked == true)
                {
                    taskWork t = new taskWork();
                    runningArray.TryAdd(def.Id.ToString(), t);
                    //tasks[def.Id.ToString()].Start(def,ref garbage, garbage.Dispatcher,"external");        
                    runningArray[def.Id.ToString()].Start(def, ref garbage, garbage.Dispatcher, "external");
                }
            }

            #endregion

            #region Wait for tasks to finish
            //Very important section Wait for all Tasks to finish then it would be possible checking the schedule again
            List<Task> taskArr = new List<Task>();
            foreach (taskWork t in tasks.Values)
            {
                taskArr.Add(t.getTask());
            }
            // Task.WaitAll(taskArr.ToArray());
            #endregion
            for (; ; )
            {
                if (cancelSched == true)
                {
                    foreach (taskWork tsk in tasks.Values)
                    {
                        if (tsk.started == true)
                            tsk.Cancel();
                    }
                    cancelSched = false;
                    break;
                }


            }

            startSched = false;
        }

        public static bool CheckTime(DateTime dt)
        {
            return Math.Abs((dt - DateTime.Now).Hours) == 0 && Math.Abs((dt - DateTime.Now).Minutes) <= 3 && Math.Abs((dt - DateTime.Now).Seconds) <= 59;
        }


        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            startSched = true;
            if (datecb.IsChecked == false)
            {
                MessageBox.Show("Please choose a date and time...");
                return;
            }
            if (dateofrun.SelectedDate == null || dateofrun.SelectedDate < DateTime.Now)
            {
                MessageBox.Show("Please select a date....");
                return;
            }
            else if (timeofrun.Value == null)
            {
                MessageBox.Show("Please choose time...");
                return;
            }
            if (overridecb.IsChecked == true && overridespeed.Text != "")
                foreach (Define item in d.Defines)
                {
                    item.Speed = Convert.ToInt32(overridespeed.Text);
                    d.SubmitChanges();
                }

            startButton.IsEnabled = false;
            stopButton.IsEnabled = true;
            //ProgressBar Garbage = new ProgressBar();


            #region Schedular

            #region Take all selected Defs, add them to tasks array
            //all defs took in the constructor and added to the array #section 1
            //now must choose only those were selected in the status_list #section 2
            //Run code over all the selected items
            //ProgressBar garbage = new ProgressBar();
            foreach (Define def in status_list.Items)
            {
                if (def.isChecked == true)
                {
                    taskWork t = new taskWork();
                    runningArray.TryAdd(def.Id.ToString(), t);
                    //tasks[def.Id.ToString()].Start(def,ref garbage, garbage.Dispatcher,"external");        
                    //runningArray[def.Id.ToString()].Start(def, ref garbage, garbage.Dispatcher, "external");
                    runningArray[def.Id.ToString()].TaskStart(def);
                }
            }

            #endregion

            //#region add them to runningArray & Run each one & set isRunning = true
            ////#section 2
            ////th.Start(status_list);
            //#endregion

            #region Check all if isFinished and & due date
            //if not finished put in pending array
            //function receives array of tasks and return pending array
            //check pending continuosly and start finished ones
            Task tsk = new Task(() =>
           {
               while (true)
               {
                   if (cancelSched == true)
                   {
                       foreach (var item in runningArray.Values)
                       {
                           item.Cancel();
                       }
                       cancelSched = false;
                       break;
                   }
                   CheckNewListBoxItems();//if new add them to runningArray
                   runningArray = CheckFinished(runningArray);
                   DateTime? startDate = (from dt in d.SettingsTables select dt.startDate).FirstOrDefault();
                   DateTime? dueDate = (from dt in d.SettingsTables select dt.dueDate).FirstOrDefault();

                   if (runningArray.Count == 0 && DateTime.Now < dueDate)
                   {
                       //do nothing
                   }
                   else if (runningArray.Count > 0 && DateTime.Now < dueDate)
                   {
                       //do nothing
                   }
                   //else if (runningArray.Count > 0 && DateTime.Now == dueDate)
                   else if (runningArray.Count > 0 && CheckTime(dueDate.Value))
                   {
                       foreach (Define def in status_list.Items)
                       {
                           if (def.isChecked == true && tasks[def.Id.ToString()].isFinished == false) //still this def is not finished then add it to pendingArray to be run again
                           {
                               taskWork t = new taskWork();
                               pendingArray.TryAdd(def.Id.ToString(), t);

                           }
                           else if (def.isChecked == true && tasks[def.Id.ToString()].isFinished == true)
                           {
                               //ProgressBar garbage = new ProgressBar();
                               taskWork t = new taskWork();
                               runningArray.TryAdd(def.Id.ToString(), t);
                               //tasks[def.Id.ToString()].Start(def,ref garbage, garbage.Dispatcher,"external");        
                               //runningArray[def.Id.ToString()].Start(def, ref garbage, garbage.Dispatcher, "external");
                               runningArray[def.Id.ToString()].TaskStart(def);
                           }

                       }
                       startDate = dueDate;
                       dueDate = dueDate.Value.AddDays(1);
                       d.SubmitChanges();
                   }

               }
           });
            tsk.Start();

            #endregion


            #endregion End Schedular

            #region Selection
            /*
            #region Select All Unchecked
            if (selectall.IsChecked == false)
            {
                int fromcount = 0, tocount = 0, allcount = define_list.Items.Count;

                if (schedcombo.Text == "First" && Regex.IsMatch(fromtx.Text, @"^[1-9]+[0-9]*$"))
                {
                    fromcount = Convert.ToInt32(fromtx.Text);//Get the value
                    if (fromcount <= allcount)
                        for (int i = 0; i < fromcount; i++)
                        {
                            //Get the item as Define
                            Define def = tasksselector.Items[i] as Define;
                            def.isChecked = true;
                            //Override the speed
                            if (overridecb.IsChecked == true && Regex.IsMatch(overridespeed.Text, @"^[1-9]+[0-9]*$"))
                                def.Speed = Convert.ToInt32(overridespeed.Text);
                            d.SubmitChanges();
                            //item id passed to the tasks dictionary to retrieve the task to start
                            //tasks[def.Id.ToString()].Start(def, ref Garbage, Garbage.Dispatcher);

                        }
                    else if (fromcount > allcount)
                        for (int i = 0; i < allcount; i++)
                        {
                            // Get the item as Define
                            Define def = tasksselector.Items[i] as Define;
                            def.isChecked = true;
                            //Override the speed
                            if (overridecb.IsChecked == true && Regex.IsMatch(overridespeed.Text, @"^[1-9]+[0-9]*$"))
                                def.Speed = Convert.ToInt32(overridespeed.Text);
                            d.SubmitChanges();
                            //item id passed to the tasks dictionary to retrieve the task to start
                            //tasks[def.Id.ToString()].Start(def, ref Garbage, Garbage.Dispatcher);

                        }
                }
                else if (schedcombo.Text == "Last" && Regex.IsMatch(fromtx.Text, @"^[1-9]+[0-9]*$"))
                {
                    fromcount = Convert.ToInt32(fromtx.Text);//Get the value
                    if (fromcount <= allcount)
                        for (int i = allcount - 1; i > allcount - fromcount - 1; i--)
                        {
                            // Get the item as Define
                            Define def = tasksselector.Items[i] as Define;
                            def.isChecked = true;
                            //Override the speed
                            if (overridecb.IsChecked == true && Regex.IsMatch(overridespeed.Text, @"^[1-9]+[0-9]*$"))
                                def.Speed = Convert.ToInt32(overridespeed.Text);
                            //item id passed to the tasks dictionary to retrieve the task to start
                            d.SubmitChanges();
                            tasks[def.Id.ToString()].Start(def, ref Garbage, Garbage.Dispatcher, "external");

                        }
                    else if (fromcount > allcount)
                        for (int i = 0; i < allcount; i++)
                        {
                            // Get the item as Define
                            Define def = tasksselector.Items[i] as Define;
                            def.isChecked = true;
                            //Override the speed
                            if (overridecb.IsChecked == true && Regex.IsMatch(overridespeed.Text, @"^[1-9]+[0-9]*$"))
                                def.Speed = Convert.ToInt32(overridespeed.Text);
                            d.SubmitChanges();
                            //item id passed to the tasks dictionary to retrieve the task to start
                            tasks[def.Id.ToString()].Start(def, ref Garbage, Garbage.Dispatcher,"external");

                        }
                }
                else if (schedcombo.Text == "From" && Regex.IsMatch(fromtx.Text, @"^[1-9]+[0-9]*$") && Regex.IsMatch(totx.Text, @"^[1-9]+[0-9]*$"))
                {
                    fromcount = Convert.ToInt32(fromtx.Text) - 1;//Get the value
                    tocount = Convert.ToInt32(totx.Text);//Get the value
                    if (allcount < tocount && allcount > fromcount)
                        for (int i = fromcount; i < allcount; i++)
                        {
                            // Get the item as Define
                            Define def = tasksselector.Items[i] as Define;
                            def.isChecked = true;
                            //Override the speed
                            if (overridecb.IsChecked == true && Regex.IsMatch(overridespeed.Text, @"^[1-9]+$"))
                                def.Speed = Convert.ToInt32(overridespeed.Text);
                            //item id passed to the tasks dictionary to retrieve the task to start
                            //tasks[def.Id.ToString()].Start(def, ref Garbage, Garbage.Dispatcher);

                        }
                    else if (allcount < fromcount)
                    {
                        //do nothing
                    }
                    else if (allcount == tocount)
                        for (int i = fromcount; i < allcount; i++)
                        {
                            // Get the item as Define
                            Define def = tasksselector.Items[i] as Define;
                            def.isChecked = true;
                            //Override the speed
                            if (overridecb.IsChecked == true && Regex.IsMatch(overridespeed.Text, @"^[1-9]+$"))
                                def.Speed = Convert.ToInt32(overridespeed.Text);
                            //item id passed to the tasks dictionary to retrieve the task to start
                            //tasks[def.Id.ToString()].Start(def, ref Garbage, Garbage.Dispatcher);

                        }
                    else if (allcount > tocount)
                        for (int i = fromcount; i < tocount; i++)
                        {
                            // Get the item as Define
                            Define def = tasksselector.Items[i] as Define;
                            def.isChecked = true;
                            //Override the speed
                            if (overridecb.IsChecked == true && Regex.IsMatch(overridespeed.Text, @"^[1-9]+$"))
                                def.Speed = Convert.ToInt32(overridespeed.Text);
                            //item id passed to the tasks dictionary to retrieve the task to start
                            //tasks[def.Id.ToString()].Start(def, ref Garbage, Garbage.Dispatcher);

                        }
                }
            }
            #endregion

            #region Select All Checked
            if (selectall.IsChecked == true)
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    // Get the item as Define
                    Define def = tasksselector.Items[i] as Define;
                    def.isChecked = true;

                    //Override the speed
                    if (overridecb.IsChecked == true && Regex.IsMatch(overridespeed.Text, @"^[1-9]+[0-9]*$"))
                        def.Speed = Convert.ToInt32(overridespeed.Text);
                    def.isStarted = true;
                    d.SubmitChanges();
                    //item id passed to the tasks dictionary to retrieve the task to start
                    //tasks[def.Id.ToString()].Start(def, ref Garbage, Garbage.Dispatcher);

                }
            }
            #endregion
            */
            #endregion Selection


        }

        private void CheckNewListBoxItems()
        {

            foreach (Define item in status_list.Items)
            {
                if (item.isChecked == true && !runningArray.Keys.Contains(item.Id.ToString()))
                {
                    taskWork t = new taskWork();
                    runningArray.TryAdd(item.Id.ToString(), t);
                }
            }
        }

        private ConcurrentDictionary<string, taskWork> CheckFinished(ConcurrentDictionary<string, taskWork> runningArray)
        {
            //ProgressBar garbage = new ProgressBar();
            for (int i = 0; i < runningArray.Count; i++)
            {
                taskWork current = runningArray.ElementAt(i).Value;
                taskWork temp = new taskWork();

                if (current.isRunning == false && current.isFinished == true)
                {
                    //runningArray.TryRemove(runningArray.ElementAt(i).Key, out temp);//get it from pendingArray if exist
                    foreach (var pendingItem in pendingArray)
                    {
                        if (pendingItem.Key == runningArray.ElementAt(i).Key)
                        {
                            foreach (Define def in status_list.Items)
                            {
                                if (def.Id.ToString() == pendingItem.Key)
                                {
                                    //runningArray[runningArray.ElementAt(i).Key].Start(def, ref garbage, garbage.Dispatcher, "external");
                                    runningArray[runningArray.ElementAt(i).Key].TaskStart(def);
                                    pendingArray.TryRemove(pendingItem.Key, out var tmp);
                                }
                            }

                        }
                    }

                }
            }
            return runningArray;

        }



        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            overridespeed.IsEnabled = true;

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            overridespeed.IsEnabled = false;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            #region  Select the focused on item
            DependencyObject parent = VisualTreeHelper.GetParent(sender as Button);
            while (!(parent is ListBoxItem))
                parent = VisualTreeHelper.GetParent(parent);
            ListBoxItem lbi = parent as ListBoxItem;
            lbi.IsSelected = true;
            #endregion

            #region Delete it from tasks
            taskWork del;
            tasks[(status_list.SelectedItem as Define).Id.ToString()].Close();
            tasks.TryRemove((status_list.SelectedItem as Define).Server, out del);
            #endregion

            #region Delete it from database first
            d.Defines.DeleteOnSubmit(status_list.SelectedItem as Define);
            d.SubmitChanges();
            #endregion

            #region Delete it from the list
            define_list.ItemsSource = null;
            define_list.ItemsSource = from k in d.Defines select k;
            #endregion
        }

        private void Datecb_Checked(object sender, RoutedEventArgs e)
        {
            dateofrun.IsEnabled = true;

        }

        private void Dateofrun_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            timeofrun.IsEnabled = true;
        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;

        }

        NotifyIcon ni = new NotifyIcon();


        private void Window1_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                ni.Icon = new System.Drawing.Icon("../../Images/logo.ico");
                ni.Visible = true;
                ni.BalloonTipText = "TechnoFriend in Tray..";
                ni.ShowBalloonTip(1000);
                ni.DoubleClick += Ni_DoubleClick; ;

                System.Windows.Forms.ContextMenu conmen = new System.Windows.Forms.ContextMenu();
                conmen.MenuItems.Add("Open", new System.EventHandler(open));
                conmen.MenuItems.Add("Exit", new System.EventHandler(exit));
                ni.ContextMenu = conmen;

                this.Hide();
            }
        }

        private void exit(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void open(object sender, EventArgs e)
        {
            this.Show();
            ni.Visible = false;
            this.WindowState = WindowState.Normal;
        }

        private void Ni_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            ni.Visible = false;
            this.WindowState = WindowState.Normal;
        }

        private void Datecb_Unchecked(object sender, RoutedEventArgs e)
        {
            dateofrun.IsEnabled = true;
            checkpic.Visibility = Visibility.Hidden;
        }

        private void Timeofrun_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            checkpic.Visibility = Visibility.Visible;
            if (dateofrun.SelectedDate == null)
                MessageBox.Show("Please select a date...");
            else
            {
                DateTime? dtofRun = new DateTime(dateofrun.SelectedDate.Value.Year,
                                                dateofrun.SelectedDate.Value.Month,
                                                dateofrun.SelectedDate.Value.Day,
                                                timeofrun.Value.Value.Hour,
                                                timeofrun.Value.Value.Minute,
                                                timeofrun.Value.Value.Second);

                var settings = (from set in d.SettingsTables
                                select set).FirstOrDefault();
                settings.startDate = DateTime.Now;
                settings.dueDate = dtofRun;
                d.SubmitChanges();
            }


        }

        private void Checkpic_MouseDown(object sender, MouseButtonEventArgs e)
        {

            DateTime dateofrunVal = dateofrun.SelectedDate.Value;
            DateTime? time = timeofrun.Value;


            DateTime dt = new DateTime(dateofrunVal.Year, dateofrunVal.Month, dateofrunVal.Day,
                time.Value.Hour, time.Value.Minute, time.Value.Second);

            SettingsTable strtDate = (from s in d.SettingsTables
                                      select s).ToList<SettingsTable>()[0];
            strtDate.startDate = dt;
            strtDate.dueDate = dt.AddDays(1.0);
            d.SubmitChanges();



        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            startSched = false;
            stopButton.IsEnabled = false;
            startButton.IsEnabled = true;
            cancelSched = true;
        }

        private void Pbar_Loaded(object sender, RoutedEventArgs e)
        {
            //ListBoxItem lbi = GetParentPB(sender);
            ////ListBoxItem lbi = GetParent(sender);//get the listBoxItem
            //lbi.IsSelected = true;
            //Define def = status_list.SelectedItem as Define;
            //progressBars.TryAdd(def.Id, sender as ProgressBar);



        }

        private void Status_list_Loaded(object sender, RoutedEventArgs e)
        {
           
            status_list.ItemsSource = d.Defines.ToList();
        }

        private void Button_Loaded_2(object sender, RoutedEventArgs e)
        {

        }

        private void Overridespeed_TextChanged(object sender, TextChangedEventArgs e)
        {

            int count = overridespeed.Text.ToCharArray().Count();
            string temp = "";

            for (int i = 0; i < count; i++)
            {
                if (Char.IsNumber(overridespeed.Text[i]))
                    temp += overridespeed.Text[i];

            }
            overridespeed.Text = temp;

            overridespeed.Focus();
            overridespeed.SelectionStart = overridespeed.Text.Length;


        }

        private void Dateofrun_Loaded(object sender, RoutedEventArgs e)
        {
            dateofrun.BlackoutDates.AddDatesInPast();
            dateofrun.BlackoutDates.Add(new CalendarDateRange(DateTime.Now));
            dateofrun.SelectedDate = DateTime.Now.AddDays(1);
        }

        private void Button_Loaded_3(object sender, RoutedEventArgs e)
        {
            if (status_list.SelectedItem != null)
                if (startSched == true)
                    (sender as Button).IsEnabled = false;

        }

        
    }
}
