using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


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
            int serviceTagValue = int.Parse(txtSerTag.Text);
            int ServiceTag = serviceTagValue;
            Drone newItem;            

            try
            {
                newItem = new Drone
                {
                    ClientName = txtClientName.Text,
                    DroneModel = txtDroneModel.Text,
                    ServiceProblem = txtSerProblem.Text,
                    ServiceCost = double.Parse(txtSerCost.Text),
                    ServiceTag = serviceTagValue
                };
                StatusBar("Item successfully added to queue");
            }
            catch (System.FormatException)
            {
                StatusBar("Action cancelled - Please fill out all fields");
                return;
            }
            //Dupe Check
            if (HasDuplicates(newItem.ClientName))
            {
                ClearTextBoxes();
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
                ExpressQueue.Enqueue(newItem);

                //Display the contents of the ExpressService queue in the list view Express control
                Display(lstView_Express, ExpressQueue);

                //Increments the value of the Service Tag control by 10
                ServiceTag += 10;
            }
            else if (servicePriority == "Standard")
            {
                //Add the service item to the RegularService queue
                StandardQueue.Enqueue(newItem);

                //Display the contents of the RegularService queue in the list view control
                Display(lstView_Standard, StandardQueue);

                //Increments the value of the Service Tag control by 10
                ServiceTag += 10;
            }
            ClearTextBoxes();
        }
        #endregion

        //Checks all fields and radio buttons if they are null or empty
        #region
        private void CheckFields()
        {
            if (string.IsNullOrEmpty(txtClientName.Text) || string.IsNullOrEmpty(txtDroneModel.Text) || string.IsNullOrEmpty(txtSerProblem.Text) || string.IsNullOrEmpty(txtSerCost.Text) || (RdoStandard.IsChecked == false && RdoExpress.IsChecked == false))
            {
                StatusBar("Action cancelled - Please fill out all fields correctly");
                ClearTextBoxes();
            }
        }
        #endregion

        //Duplicate check method
        #region
        private bool HasDuplicates(string clientName)
        {
            // Copies Standard queue then checks the copy for a duplicate
            Queue<Drone> standardQueueCopy = new Queue<Drone>(StandardQueue);
            while (standardQueueCopy.Count > 0)
            {
                Drone item = standardQueueCopy.Dequeue();
                if (item.ClientName == clientName)
                {
                    return true;
                }
            }

            // Copies Express queue then checks the copy for a duplicate
            Queue<Drone> expressQueueCopy = new Queue<Drone>(ExpressQueue);
            while (expressQueueCopy.Count > 0)
            {
                Drone item = expressQueueCopy.Dequeue();
                if (item.ClientName == clientName)
                {
                    return true;
                }
            }
            // No Dupes
            return false;
        }

        #endregion

        //Display method displays all attributes
        #region
        private void Display(ListView listView, IEnumerable<Drone> collection)
        {
            //Clears the current items in the List View
            listView.Items.Clear();

            //Iterate through each item in the queue
            foreach (Drone drone in collection)
            {
                //Create a new List View Item with the desired data
                ListViewItem item = new ListViewItem();
                item.Content = $"{drone.ClientName} - {drone.DroneModel} - {drone.ServiceProblem} - ${drone.ServiceCost.ToString("F1")} - {drone.ServiceTag}";

                //Stores a reference to the associated Drone object in the Tag property of the List View Item
                item.Tag = drone;

                //Adds the List View Item to the List View
                listView.Items.Add(item);
            }
        }
        #endregion

        //Method for returning service values
        #region
        private string GetServicePriority()
        {
            // Check the state of the radio buttons to determine the service priority
            if (RdoExpress.IsChecked == true)
            {
                // Express service has priority
                return "Express";
            }
            else if (RdoStandard.IsChecked == true)
            {
                // Standard service has priority
                return "Standard";
            }
            else
            {
                // If neither radio button is checked, return an empty string to indicate an error
                return "";
            }
        }
        #endregion

        //Edit button method
        #region
        private void EditSelectedItem()
        {
            if (lstView_Standard.SelectedItem != null)
            {
                int serviceTagValue = int.Parse(txtSerTag.Text);
                int ServiceTag = serviceTagValue;

                ListViewItem selectedItem = (ListViewItem)lstView_Standard.SelectedItem;
                int index = lstView_Standard.Items.IndexOf(selectedItem);

                // Retrieve the drone from the queue without removing it
                Drone selectedDrone = StandardQueue.ElementAt(index);

                // Update the attributes of the selected drone with the values entered in the text boxes
                selectedDrone.ClientName = txtClientName.Text;
                selectedDrone.DroneModel = txtDroneModel.Text;
                selectedDrone.ServiceProblem = txtSerProblem.Text;
                selectedDrone.ServiceCost = double.Parse(txtSerCost.Text);
                selectedDrone.ServiceTag = ServiceTag;
                StatusBar("Item has been successfully edited");

                if (RdoExpress.IsChecked == true)
                {
                    // Increase the service cost by 15%
                    selectedDrone.ServiceCost *= 1.15;

                    // Add the selected drone to the Express Service queue
                    ExpressQueue.Enqueue(selectedDrone);
                    StatusBar("Item has been successfully edited");

                    // Remove the selected drone from the Standard Service queue
                    StandardQueue = new Queue<Drone>(StandardQueue.Where((item, i) => i != index));
                }

                Display(lstView_Standard, StandardQueue);
                Display(lstView_Express, ExpressQueue);
            }
            else if (lstView_Express.SelectedItem != null)
            {
                int serviceTagValue = int.Parse(txtSerTag.Text);
                int ServiceTag = serviceTagValue;

                ListViewItem selectedItem = (ListViewItem)lstView_Express.SelectedItem;
                int index = lstView_Express.Items.IndexOf(selectedItem);

                // Retrieve the drone from the queue without removing it
                Drone selectedDrone = ExpressQueue.ElementAt(index);

                // Update the attributes of the selected drone with the values entered in the text boxes
                selectedDrone.ClientName = txtClientName.Text;
                selectedDrone.DroneModel = txtDroneModel.Text;
                selectedDrone.ServiceProblem = txtSerProblem.Text;
                selectedDrone.ServiceCost = double.Parse(txtSerCost.Text);
                selectedDrone.ServiceTag = ServiceTag;
                StatusBar("Item has been successfully edited");

                if (RdoStandard.IsChecked == true)
                {
                    // Remove the 15% surcharge by dividing by 1.15
                    selectedDrone.ServiceCost /= 1.15;

                    // Add the selected drone to the Standard Service queue
                    StandardQueue.Enqueue(selectedDrone);

                    // Remove the selected drone from the Express Service queue
                    ExpressQueue = new Queue<Drone>(ExpressQueue.Where((item, i) => i != index));
                    StatusBar("Item has been successfully edited");
                }

                Display(lstView_Standard, StandardQueue);
                Display(lstView_Express, ExpressQueue);
            }
        }

        #endregion

        //Delete Button method
        #region
        private void DeleteSelectedItem()
        {
            //Checks if an item is selected in the list View Standard control
            if (lstView_Standard.SelectedItem != null)
            {
                //Retrieves the selected item from the list View Standard control
                ListViewItem selectedItem = (ListViewItem)lstView_Standard.SelectedItem;

                //Retrieves the index of the selected item in the Standard Service queue
                int index = lstView_Standard.Items.IndexOf(selectedItem);

                //Remove the selected item from the Standard Service queue
                StandardQueue = new Queue<Drone>(StandardQueue.Where((item, i) => i != index));
                StatusBar("Item has been successfully deleted");

                //Refreshs the contents of the list View Standard control
                Display(lstView_Standard, StandardQueue);
                ClearTextBoxes();
            }
            //Checks if an item is selected in the list View Express control
            else if (lstView_Express.SelectedItem != null)
            {
                //Retrieves the selected item from the list View Express control
                ListViewItem selectedItem = (ListViewItem)lstView_Express.SelectedItem;

                //Retrieves the index of the selected item in the Express Service queue
                int index = lstView_Express.Items.IndexOf(selectedItem);

                //Removes the selected item from the Express Service queue
                ExpressQueue = new Queue<Drone>(ExpressQueue.Where((item, i) => i != index));
                StatusBar("Item has been successfully deleted");

                //Refreshs the contents of the list View Express control
                Display(lstView_Express, ExpressQueue);
                ClearTextBoxes();
            }
            else if (lstView_Finished.SelectedItem != null)
            {
                StatusBar("Action cancelled - You can't delete that item");
            }
        }
        #endregion

        //Clears textboxes 
        #region
        private void ClearTextBoxes()
        {
            int serviceTagValue = int.Parse(txtSerTag.Text);
            int ServiceTag = serviceTagValue;
            //Clears the text in all of the text boxes
            txtClientName.Clear();
            txtDroneModel.Clear();
            txtSerProblem.Clear();
            txtSerCost.Clear();

            //Checks if the value of the Service Tag control exceeds 900
            if (ServiceTag > 900)
            {
                //Sets the value of the Service Tag control to 900
                ServiceTag = 900;

                StatusBar("Can't count that high lol");
            }

            // Uncheck both radio buttons
            RdoStandard.IsChecked = false;
            RdoExpress.IsChecked = false;
        }
        #endregion

        // Move to finished list method CHECK FUNCTIONALITY
        #region
        private void MoveToFinished()
        {
            Drone item = null;

            // Check if an item is selected in the list View Standard control
            if (lstView_Standard.SelectedItem != null)
            {
                // Retrieve the selected item from the list View Standard control
                ListViewItem selectedItem = (ListViewItem)lstView_Standard.SelectedItem;

                // Retrieve the index of the selected item in the Standard Service queue
                int index = lstView_Standard.Items.IndexOf(selectedItem);

                // Check if the selected item index matches the index of the top item in the Standard Service queue
                if (index == 0)
                {
                    // Dequeue the top item from the Standard Service queue
                    item = StandardQueue.Dequeue();
                }
            }
            // Check if an item is selected in the list View Express control
            else if (lstView_Express.SelectedItem != null)
            {
                // Retrieve the selected item from the list View Express control
                ListViewItem selectedItem = (ListViewItem)lstView_Express.SelectedItem;

                // Retrieve the index of the selected item in the Express Service queue
                int index = lstView_Express.Items.IndexOf(selectedItem);

                // Check if the selected item index matches the index of the top item in the Express Service queue
                if (index == 0)
                {
                    // Dequeue the top item from the Express Service queue
                    item = ExpressQueue.Dequeue();
                }
            }

            if (item != null)
            {
                // Add the selected item to the Finished List
                FinishedList.Add(item);
                StatusBar("Job completed");

                // Refresh the contents of the list View Standard control
                Display(lstView_Standard, StandardQueue);

                // Refresh the contents of the list View Express control
                Display(lstView_Express, ExpressQueue);

                // Refresh the contents of the list View Finished control
                Display(lstView_Finished, FinishedList);
            }
        }
        #endregion


        //Client paid method
        #region
        private void ClientPaid()
        {
            //Checks if an item is selected in the list View Finished control
            if (lstView_Finished.SelectedItem != null)
            {
                //Retrieves the selected item from the list View Finished control
                ListViewItem selectedItem = (ListViewItem)lstView_Finished.SelectedItem;

                //Retrieves the index of the selected item in the Finished List
                int index = lstView_Finished.Items.IndexOf(selectedItem);

                //Removes the selected item from the Finished List
                FinishedList.RemoveAt(index);

                //Refreshs the contents of the list View Finished control
                Display(lstView_Finished, FinishedList);
            }
        }
        #endregion

        private void StatusBar(string msg)
        {
            statusBar.Items.Clear();
            statusBar.Items.Add(msg);
        }
        private void addQueue_Click(object sender, RoutedEventArgs e)
		{
            AddNewItem();
        }


		private void editButton_Click(object sender, RoutedEventArgs e)
		{
            EditSelectedItem();
        }

		private void completeButton_Click(object sender, RoutedEventArgs e)
		{
            MoveToFinished();
        }

		private void paidButton_Click(object sender, RoutedEventArgs e)
		{
            ClientPaid();
            StatusBar("Client paid - removed from finished jobs");
        }

		private void deleteButton_Click(object sender, RoutedEventArgs e)
		{
            DeleteSelectedItem();
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

		private void RdoStandard_Checked(object sender, RoutedEventArgs e)
		{

		}

		private void RdoExpress_Checked(object sender, RoutedEventArgs e)
		{

		}

        //Deselecting listview items
        #region
        private void lstView_Standard_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            lstView_Express.SelectedItem = null;
            lstView_Finished.SelectedItem = null;
        }

		private void lstView_Express_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            lstView_Standard.SelectedItem = null;
            lstView_Finished.SelectedItem = null;
        }

		private void lstView_Finished_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            lstView_Standard.SelectedItem = null;
            lstView_Express.SelectedItem = null;
        }
        #endregion


        private void lstView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Gets the selected item from the list view
            var standardDrone = (lstView_Standard.SelectedItem as ListViewItem)?.Tag as Drone;
            var expressDrone = (lstView_Express.SelectedItem as ListViewItem)?.Tag as Drone;
            var finishedDrone = (lstView_Finished.SelectedItem as ListViewItem)?.Tag as Drone;
            int serviceTagValue = int.Parse(txtSerTag.Text);
            int ServiceTag = serviceTagValue;

            if (standardDrone != null)
            {
                //Displays the attributes of the selected item in the text boxes
                txtClientName.Text = standardDrone.ClientName;
                txtDroneModel.Text = standardDrone.DroneModel;
                txtSerProblem.Text = standardDrone.ServiceProblem;
                txtSerCost.Text = standardDrone.ServiceCost.ToString();
                ServiceTag = standardDrone.ServiceTag;

                //Checks the Standard radio button
                RdoStandard.IsChecked = true;
            }
            else if (expressDrone != null)
            {
                //Displays the attributes of the selected item in the text boxes
                txtClientName.Text = expressDrone.ClientName;
                txtDroneModel.Text = expressDrone.DroneModel;
                txtSerProblem.Text = expressDrone.ServiceProblem;
                txtSerCost.Text = expressDrone.ServiceCost.ToString();
                ServiceTag = expressDrone.ServiceTag;

                //Checks the Express radio button
                RdoExpress.IsChecked = true;
            }
            else if (finishedDrone != null)
            {
                txtClientName.Text = finishedDrone.ClientName;
                txtDroneModel.Text = finishedDrone.DroneModel;
                txtSerProblem.Text = finishedDrone.ServiceProblem;
                txtSerCost.Text = finishedDrone.ServiceCost.ToString();
                ServiceTag = finishedDrone.ServiceTag;

                //Unchecks both radio buttons
                RdoStandard.IsChecked = false;
                RdoExpress.IsChecked = false;
            }
            StatusBar("All fields poplated - Waiting for next action");
        }
        private void minusButton(object sender, RoutedEventArgs e)
        {
            int currentValue = int.Parse(txtSerTag.Text);
            int newValue = currentValue - 10;
            newValue = Math.Max(newValue, 100); // Ensure the new value is not less than 100
            txtSerTag.Text = newValue.ToString();
        }

        private void plusButton(object sender, RoutedEventArgs e)
        {
            int currentValue = int.Parse(txtSerTag.Text);
            int newValue = currentValue + 10;
            newValue = Math.Min(newValue, 900); // Ensure the new value is not greater than 900
            txtSerTag.Text = newValue.ToString();
        }

    }
}
