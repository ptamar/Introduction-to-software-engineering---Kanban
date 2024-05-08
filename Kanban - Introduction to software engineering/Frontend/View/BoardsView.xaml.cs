using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Frontend.ModelView;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for Boards.xaml
    /// </summary>
    public partial class BoardsView : Page
    {
        private BoardsVM _boardsVM;
        private string _email;
        private string selectedBoardName;
        private List<string> _boardNames;
        private Dictionary<int, string> _boardsDictionary;
    

        public BoardsView(string userEmail)
        {
            InitializeComponent();
            this._email = userEmail;
            this._boardsVM = new BoardsVM(_email);
            this._boardsDictionary = new Dictionary<int, string>();
            this._boardsDictionary = _boardsVM.GetBoards(_email);
            this._boardNames = _boardsDictionary.Values.ToList();
            UserBoards.ItemsSource = _boardNames;
            // this.DataContext = this._boardsDictionary;
        }
        /// <summary>
        /// This method search the name of the board .
        /// </summary>
        /// <returns>void . </returns>
        private void Search_Board(object sender, RoutedEventArgs e)
        {
            try
            {
                TasksView tx = new TasksView(_email, selectedBoardName);
                // Note that the board is searched using board name and email only!
                // I deem the ID box is redundant
                this.Content = new Frame() {Content = tx};
            }
            catch (Exception ex)
            {
                MessageBox.Show("You do not have a board by that name!");
            }
        }
        // private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        // {
        // What is this nonsense of a name?!
        // }

        /// <summary>
        /// This method change the name of the board to text .
        /// </summary>
        /// <returns>void.</returns>
        private void BoardNameT_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.selectedBoardName = BoardNameText.Text;
        }

        private void Search_Board(object sender, SelectionChangedEventArgs e)
        {
            
        }
      
        private void UserBoards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void BoardNameText_GotFocus(object sender, RoutedEventArgs e)
        {
            // Clear the text when the TextBox is touched
            BoardNameText.Text = "";
        }
    }
}
