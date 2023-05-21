using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace machine_management_system
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FillDataGrid();
        }

        public void Reset() 
        {
            usernameText.Text = "";
            passwordText.Password = "";
            confirmPasswordText.Password = "";
            shopNameText.Text = "";
            phoneNumberText.Text = "";
            emailText.Text = "";
        }

        public void ItemReset()
        {
            itemText.Text = "";
            priceText.Text = "";
            brandText.Text = "";
            quantityText.Text = "";
        }

        public void ChangeToRegisterPage()
        {
            tabControl.SelectedIndex = 0;
        }

        public void ChangeToLoginPage()
        {
            tabControl.SelectedIndex = 1;
        }

        public void ChangeToMainPage()
        {
            tabControl.SelectedIndex = 2;
        }

        private void OnSignUpClick(object sender, RoutedEventArgs e)
        {
            string username = usernameText.Text;
            string password = passwordText.Password;
            string confirmPassword = confirmPasswordText.Password;
            string shopName = shopNameText.Text;
            string phoneNumber = phoneNumberText.Text;
            string email = emailText.Text;

            if (username.Length == 0)
            {
                string errorMessage = "Enter an username";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                usernameText.Focus();
            }
            else if (password.Length == 0)
            {
                string errorMessage = "Enter a password";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                passwordText.Focus();
            }
            else if (confirmPassword.Length == 0)
            {
                string errorMessage = "Enter a confirm password";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                confirmPasswordText.Focus();
            }
            else if (!password.Equals(confirmPassword))
            {
                string errorMessage = "Password did not match";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                passwordText.Focus();
                confirmPasswordText.Focus();
            }
            else if (shopName.Length == 0)
            {
                string errorMessage = "Enter a shop name";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                shopNameText.Focus();
            }
            else if (phoneNumber.Length == 0)
            {
                string errorMessage = "Enter a phone number";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                phoneNumberText.Focus();
            }
            else if (!Regex.IsMatch(phoneNumber, "^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))
            {
                string errorMessage = "Enter a valid phone number";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                phoneNumberText.Focus();
            }
            else if (email.Length == 0)
            {
                string errorMessage = "Enter an email.";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                emailText.Focus();
            }
            else if (!Regex.IsMatch(email, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                string errorMessage = "Enter a valid email.";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                emailText.Focus();
            }
            else
            {
                string connectionString = @"Data Source=DESKTOP-1U13RLV; Integrated Security=True; Initial Catalog=machine";
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO user_info (username,password,shopName,phoneNumber,email) VALUES (@username,@password,@shopName,@phoneNumber,@email)", con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@shopName", shopName);
                cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.ExecuteNonQuery();
                con.Close();
                Reset();
                MessageBox.Show("Register successfully!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                ChangeToLoginPage();
            }
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            string username = usernameLoginText.Text;
            string password = passwordLoginText.Password;

            if (username.Length == 0)
            {
                string errorMessage = "Enter an username";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                usernameText.Focus();
            }
            else if (password.Length == 0)
            {
                string errorMessage = "Enter a password";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                passwordText.Focus();
            }
            else
            {
                SqlConnection con = new SqlConnection("Data Source=DESKTOP-1U13RLV; Integrated Security=True; Initial Catalog=machine");
                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from user_info where username='" + username + "'  and password='" + password + "'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("Welcome " + username + "!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    ChangeToMainPage();
                }
                else
                {
                    string errorMessage = "Sorry! Please enter existing username/password.";
                    MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                con.Close();
            }
        }

        private void OnClickToLoginPage(object sender, RoutedEventArgs e)
        {
            ChangeToLoginPage();
        }

        private void OnClickToRegisterPage(object sender, RoutedEventArgs e)
        {
            ChangeToRegisterPage();
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            string item = itemText.Text;
            string price = priceText.Text;
            string brand = brandText.Text;
            string quantity= quantityText.Text;
            
            if (item.Length == 0)
            {
                string errorMessage = "Enter an item";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                itemText.Focus();
            }
            else if (price.Length == 0)
            {
                string errorMessage = "Enter a price";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                priceText.Focus();
            }
            else if (brand.Length == 0)
            {
                string errorMessage = "Enter a brand";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                brandText.Focus();
            }
            else if (quantity.Length == 0)
            {
                string errorMessage = "Enter a quantity";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                quantityText.Focus();
            }
            else
            {
                if (price.EndsWith("."))
                {
                    price = price + "0";
                }
                string connectionString = @"Data Source=DESKTOP-1U13RLV; Integrated Security=True; Initial Catalog=machine";
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO item_info (item,price,brand,quantity) VALUES (@item,@price,@brand,@quantity)", con);
                cmd.Parameters.AddWithValue("@item", item);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@brand", brand);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.ExecuteNonQuery();
                con.Close();
                ItemReset();
                MessageBox.Show("New item added!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                FillDataGrid();
            }
        }

        private void PriceText_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            bool containsDecimalPoint = e.Text.Contains(".");
            char dotSymbol = '.';
            int dotCount = priceText.Text.Count(c => c == dotSymbol);
            int resultInt;
            if (!int.TryParse(e.Text, out resultInt) || containsDecimalPoint)
            {
                e.Handled = true;
            }
            if (dotCount == 0 && priceText.Text.Length > 0)
            {
                e.Handled = false;
            }
        }

        private void QuantityText_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            int result;
            if (!int.TryParse(e.Text, out result))
            {
                e.Handled = true;
            }
        }

        private void OnUpdateClick(object sender, RoutedEventArgs e)
        {
            string item = itemText.Text;
            string price = priceText.Text;
            string brand = brandText.Text;
            string quantity = quantityText.Text;

            if (item.Length == 0)
            {
                string errorMessage = "Enter an item to update the info!";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                itemText.Focus();
            } else
            {
                string connectionString = @"Data Source=DESKTOP-1U13RLV; Integrated Security=True; Initial Catalog=machine";
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE item_info SET item = @item, price = @price, brand = @brand, quantity = @quantity WHERE item = @item", con);
                cmd.Parameters.AddWithValue("@item", item);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@brand", brand);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.ExecuteNonQuery();
                con.Close();
                FillDataGrid();
            }
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            string item = itemText.Text;

            if (item.Length == 0)
            {
                string errorMessage = "Enter an item to update the info!";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                itemText.Focus();
            }
            else
            {
                string connectionString = @"Data Source=DESKTOP-1U13RLV; Integrated Security=True; Initial Catalog=machine";
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM item_info WHERE item = @item", con);
                cmd.Parameters.AddWithValue("@item", item);
                cmd.ExecuteNonQuery();
                con.Close();
                FillDataGrid();
            }
        }

        private void FillDataGrid()
        {

            string ConString = @"Data Source=DESKTOP-1U13RLV; Integrated Security=True; Initial Catalog=machine";

            using (SqlConnection con = new SqlConnection(ConString))
            {
                string CmdString = "SELECT * FROM item_info";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("itemTable");
                sda.Fill(dt);
                itemTable.ItemsSource = dt.DefaultView;
            }
        }

        private void itemTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get selected row
            DataRowView selectedRow = itemTable.SelectedItem as DataRowView;

            // Check if the row actually selected
            if (selectedRow != null)
            {
                string column1 = selectedRow["item"].ToString();
                string column2 = selectedRow["price"].ToString();
                string column3 = selectedRow["brand"].ToString();
                string column4 = selectedRow["quantity"].ToString();

                itemText.Text = column1;
                priceText.Text = column2;
                brandText.Text = column3;
                quantityText.Text = column4;
            }
        }
    }
}
