using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace EntityFramevork_car
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        CarDbContext CarContext = new CarDbContext();
        public ObservableCollection<Car> cars { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            cars = new ObservableCollection<Car>();
            CarContext.Cars.ToList().ForEach(c => cars.Add(c));
            DataContext = this;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Tb1.Text == string.Empty && Tb2.Text == string.Empty && Tb3.Text == string.Empty && Tb4.Text == string.Empty)
            {
                BtnDelete.IsEnabled = false;
                BtnUpdate.IsEnabled = false;
            }
            if (Cars.SelectedItem is not null &&
                ((Tb1.Text != cars[Cars.SelectedIndex].Marka && Tb1.Text != string.Empty) ||
                (Tb2.Text != cars[Cars.SelectedIndex].Model && Tb2.Text != string.Empty) ||
                (Tb3.Text != cars[Cars.SelectedIndex].Year.ToString() && Tb3.Text != string.Empty) ||
                (Tb4.Text != cars[Cars.SelectedIndex].StateNumber.ToString() && Tb4.Text != string.Empty)))
                BtnUpdate.IsEnabled = true;
            else
                BtnUpdate.IsEnabled = false;
        }


        private void Cars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cars.SelectedItem is not null)
            {
                BtnDelete.IsEnabled = true;

                Tb1.Text = cars[Cars.SelectedIndex].Marka;
                Tb2.Text = cars[Cars.SelectedIndex].Model;
                Tb3.Text = cars[Cars.SelectedIndex].Year.ToString();
                Tb4.Text = cars[Cars.SelectedIndex].StateNumber.ToString();
            }

        }

        private void TbClear()
        {
            Tb1.Clear();
            Tb2.Clear();
            Tb3.Clear();
            Tb4.Clear();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (Tb1.Text is not null && Tb2.Text is not null)
            {
                using (CarDbContext database = new())
                {


                    Car car = new()
                    {
                        Model = Tb2.Text,
                        Marka = Tb1.Text,
                        Year = Convert.ToInt32(Tb3.Text),
                        StateNumber = Convert.ToInt32(Tb4.Text)
                    };
                    database.Cars.Add(car);

                    database.SaveChanges();

                    cars.Clear();

                    CarContext.Cars.ToList().ForEach(c => cars.Add(c));

                    TbClear();


                }
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Car car = null!;
            using (var database = new CarDbContext())
            {

                car = database.Cars.FirstOrDefault(c => c.Id == cars[Cars.SelectedIndex].Id)!;
                if (car is not null)
                {
                    car.Marka = Tb1.Text;
                    car.Model = Tb2.Text;
                    car.Year = Convert.ToInt32(Tb3.Text);
                    car.StateNumber = Convert.ToInt32(Tb4.Text);

                    database.Cars.Update(car);

                    cars.Clear();

                    database.Cars.ToList().ForEach(c => cars.Add(c));

                    database.SaveChanges();
                }

                TbClear();


            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            using (CarDbContext database = new())
            {
                Car car = database.Cars.FirstOrDefault(c => c.Id == cars[Cars.SelectedIndex].Id)!;

                database.Remove(car);
                database.SaveChanges();

                TbClear();

                Cars.SelectedItem = null;

                cars.Clear();

                database.Cars.ToList().ForEach(c => cars.Add(c));
            }
        }



    }
    
}
