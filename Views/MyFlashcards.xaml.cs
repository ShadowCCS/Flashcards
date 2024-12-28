using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using Path = System.IO.Path;
using FlashcardsMVP.Logs;
using FlashcardsMVP.ViewModels;
using FlashcardsMVP.Services;
namespace FlashcardsMVP.Views
{
    /// <summary>
    /// Interaction logic for MyFlashcards.xaml
    /// </summary>
    public partial class MyFlashcards : Window
    {

        public MyFlashcards()
        {
            InitializeComponent();
            this.DataContext = new MyFlashcardsViewModel();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is MyFlashcardsViewModel viewModel && sender is ListBox listBox)
            {
                if (listBox.SelectedItem is Deck selectedDeck)
                {
                    viewModel.SelectedDeck = selectedDeck;
                }
            }
        }



    }
}