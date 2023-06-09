
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;


namespace Drone_Service_App
{

    public partial class MainWindow : Window
    {
        public static List<Drone> FinishedList = new List<Drone>();
        public static Queue<Drone> StandardQueue = new Queue<Drone>();
        public static Queue<Drone> ExpressQueue = new Queue<Drone>();
        public MainWindow()
        {
            InitializeComponent();
        }
        //Add method
        #region
        private void AddNewItem()
        {
            CheckFields();
            //Retrieve the value of the ServiceTag control
            int serviceTagValue = ServiceTag.Value.Value;

            //Declare the newItem variable outside of the try-catch block
            Drone newItem;

            try
            {
                //Creates a new item Drone object with the desired data
                newItem = new Drone
                {
                    ClientName = txtName.Text,
                    DroneModel = txtModel.Text,
                    ServiceProblem = txtProblem.Text,
                    ServiceCost = double.Parse(txtCost.Text),
                    ServiceTag = serviceTagValue
                };
                StatusBar("Item successfully added to queue");
            }
            catch (System.FormatException)
            {
                StatusBar("Action cancelled - Please fill out all fields, and please enter numbers only for cost ");
                return;
            }

            //Checks if there are any duplicates
            if (HasDuplicates(newItem.ClientName))
            {
                ClearTextBoxes();
                // There are duplicates
                StatusBar("Duplicate found - Action Cancelled");
                return;
            }

            // Get the service priority from the GetServicePriority method
            string servicePriority = GetServicePriority();

            //Check the state of the radio buttons to determine which queue to add the new item to
            if (servicePriority == "Express")
            {
                // Increase the service cost by 15%
                newItem.ServiceCost *= 1.15;

                //Add the service item to the Express Service queue
                ExpressService.Enqueue(newItem);

                //Display the contents of the ExpressService queue in the list view Express control
                Display(lstViewExpress, ExpressService);

                //Increments the value of the Service Tag control by 10
                ServiceTag.Value += 10;
            }
            else if (servicePriority == "Standard")
            {
                //Add the service item to the RegularService queue
                StandardService.Enqueue(newItem);

                //Display the contents of the RegularService queue in the list view control
                Display(lstViewStandard, StandardService);

                //Increments the value of the Service Tag control by 10
                ServiceTag.Value += 10;
            }
            ClearTextBoxes();
        }
        #endregion


        private void addQueue_Click(object sender, RoutedEventArgs e)
		{

		}

		private void searchButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void editButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void completeButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void paidButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void deleteButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void txtClientName_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void txtDroneModel_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void txtSerProblem_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void txtSerCost_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void addQueue_Click_1(object sender, RoutedEventArgs e)
		{

		}

		private void editButton_Click_1(object sender, RoutedEventArgs e)
		{

		}

		private void completeButton_Click_1(object sender, RoutedEventArgs e)
		{

		}

		private void paidButton_Click_1(object sender, RoutedEventArgs e)
		{

		}

		private void deleteButton_Click_1(object sender, RoutedEventArgs e)
		{

		}

		private void searchButton_Click_1(object sender, RoutedEventArgs e)
		{

		}

		private void RepeatButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void RepeatButton_Click_1(object sender, RoutedEventArgs e)
		{

		}

		private void RdoStandard_Checked(object sender, RoutedEventArgs e)
		{

		}

		private void RdoExpress_Checked(object sender, RoutedEventArgs e)
		{

		}

		private void lstView_Standard_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void lstView_Express_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void lstView_Finished_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}
