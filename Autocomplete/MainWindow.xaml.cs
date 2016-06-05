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

namespace Autocomplete
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IAutocompleteProvider _autocomplete;


        public List<Candidate> Suggestions
        {
            get { return (List<Candidate>)this.GetValue(SuggestionsProperty); }
            set { this.SetValue(SuggestionsProperty, value); }
        }
        public static readonly DependencyProperty SuggestionsProperty = DependencyProperty.Register(
            "Suggestions", typeof(List<Candidate>), typeof(MainWindow),
            new PropertyMetadata(new List<Candidate>()));

        public MainWindow()
        {
            InitializeComponent();

            _autocomplete = new AutocompleteProvider();
        }
        private void Train_Click(object sender, RoutedEventArgs e)
        {
            _autocomplete.Train(txtTrain.Text);
        }


        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            GenerateSuggestions(txtInput.Text);

       
        }

        private void GenerateSuggestions(string input)
        {            
            this.Suggestions = _autocomplete.GetSuggestions(input); 
        }
     
    }
}
