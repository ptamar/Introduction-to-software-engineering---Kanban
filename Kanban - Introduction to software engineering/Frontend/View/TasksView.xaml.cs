using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Frontend.Model;
using Frontend.ModelView;
using Task = IntroSE.Kanban.Backend.Buissnes_Layer.Task;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class TasksView : Page
    {
        Dictionary<int, string> boards;
        TasksVM _tasksVM;
        private string colId;
        private List<string> _backlog;
        private List<string> _inProgress;
        private List<string> _done;
        private string _email;
        private string _boardName;
        private ServiceFactory _serviceFactory = ServiceFactory.getServiceFactrory();
        public TasksView(string email, string boardName)
        {
            InitializeComponent();
            this._email = email;
            this._boardName = boardName;
            this._tasksVM = new TasksVM();
            this._backlog = _tasksVM.GetColumn(_email, _boardName, 0);
            this._inProgress = _tasksVM.GetColumn(_email, _boardName, 1);
            this._done = _tasksVM.GetColumn(_email, _boardName, 2);
            backlog1.ItemsSource = _backlog;
            inprogress1.ItemsSource = _inProgress;
            done1.ItemsSource = _done;
        }

        private void GetColumn(object sender, SelectionChangedEventArgs e)
        {
            
        }
        private void UserBoards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnRtrn_Click(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            this.NavigationService.Navigate(new BoardsView(_email));

        }
        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            string title = tbTaskTitle.Text;
            string description = tbTaskDescription.Text;
            DateTime dueDate = dpDueDate.SelectedDate.Value;
            _serviceFactory.boardService.AddTask(_email, _boardName, title, description, dueDate);
            // Refresh the screen after adding the task
            RefreshScreen();
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            _serviceFactory.userService.logout(_email);
            // Close the current window
            Window.GetWindow(this).Close();
            // Open a new instance of MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
        private void RefreshScreen()
        {
            // Clear the input fields
            tbTaskTitle.Text = "";
            tbTaskDescription.Text = "";
            dpDueDate.SelectedDate = null;

            // Reload the task lists
            ReloadTaskLists();
        }
        private void ReloadTaskLists()
        {
            // Retrieve the updated task lists from the backend
            List<string> updatedBacklog = _tasksVM.GetColumn(_email, _boardName, 0);
            List<string> updatedInProgress = _tasksVM.GetColumn(_email, _boardName, 1);
            List<string> updatedDone = _tasksVM.GetColumn(_email, _boardName, 2);

            // Update the item sources of the list boxes with the updated task lists
            backlog1.ItemsSource = updatedBacklog;
            inprogress1.ItemsSource = updatedInProgress;
            done1.ItemsSource = updatedDone;
        }
        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListView listView = (ListView)sender;
            var selectedTask = (string) listView.SelectedItem;
            Regex regex = new Regex(@"ID:\s+(\d+)");
            Match match = regex.Match(selectedTask);
            string idS = match.Groups[1].Value;
            int.TryParse(idS, out int id);
            if (listView.Name == "backlog1")
            {
                // Move task from Backlog to In Progress
                _serviceFactory.boardService.NextState(_email, _boardName, 0, id);
            }
            else if (listView.Name == "inprogress1")
            {
                // Move task from In Progress to Done
                _serviceFactory.boardService.NextState(_email, _boardName, 1, id);
            }

            // Refresh the task lists after moving the task
            ReloadTaskLists();
        }

    }
}
