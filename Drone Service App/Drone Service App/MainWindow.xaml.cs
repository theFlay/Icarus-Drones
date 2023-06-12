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
            int serviceTagValue = ServiceTag.Value.Value;
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

            if (HasDuplicates(newItem.ClientName))
            {
                ClearTextBoxes();
                StatusBar("Duplicate found - Action Cancelled");
                return;
            }
            string servicePriority = GetServicePriority();

            if (servicePriority == "Express")
            {
                newItem.ServiceCost *= 1.15;
                ExpressQueue.Enqueue(newItem);
                Display(lstView_Express, ExpressQueue);
                ServiceTag.Value += 10;
            }
            else if (servicePriority == "Standard")
            {
                StandardQueue.Enqueue(newItem);
                Display(lstView_Standard, StandardQueue);
                ServiceTag.Value += 10;
            }
            ClearTextBoxes();
        }
        #endregion

        //Null CHeck
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

        //Dupe Check 
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

        //Display Method
        #region
        private void Display(ListView listView, IEnumerable<Drone> collection)
        {
            listView.Items.Clear();
            foreach (Drone drone in collection)
            {
                ListViewItem item = new ListViewItem();
                item.Content = $"{drone.ClientName} - {drone.DroneModel} - {drone.ServiceProblem} - ${drone.ServiceCost.ToString("F1")} - {drone.ServiceTag}";
                item.Tag = drone;
                listView.Items.Add(item);
            }
        }
        #endregion

        //RadioBtn Check
        #region
        private string GetServicePriority()
        {
            if (RdoExpress.IsChecked == true)
            {
                return "Express";
            }
            else if (RdoStandard.IsChecked == true)
            {
                return "Standard";
            }
            else
            {
                return "";
            }
        }
        #endregion

        //Edit  Method
        #region
        private void EditSelectedItem()
        {
            //Standard edit
            if (lstView_Standard.SelectedItem != null)
            {
                try //null check try catch
                {
                    ListViewItem selectedItem = (ListViewItem)lstView_Standard.SelectedItem;
                    int index = lstView_Standard.Items.IndexOf(selectedItem);
                    Drone selectedDrone = StandardQueue.ElementAt(index);

                    selectedDrone.ClientName = txtClientName.Text;
                    selectedDrone.DroneModel = txtDroneModel.Text;
                    selectedDrone.ServiceProblem = txtSerProblem.Text;
                    selectedDrone.ServiceCost = double.Parse(txtSerCost.Text);
                    selectedDrone.ServiceTag = ServiceTag.Value.Value;
                    StatusBar("Item has been successfully edited");

                    if (RdoExpress.IsChecked == true)
                    {
                        selectedDrone.ServiceCost *= 1.15;
                        ExpressQueue.Enqueue(selectedDrone);
                        StatusBar("Item has been successfully edited");
                        StandardQueue = new Queue<Drone>(StandardQueue.Where((item, i) => i != index));
                    }

                    Display(lstView_Standard, StandardQueue);
                    Display(lstView_Express, ExpressQueue);
                }
                catch
                {
                    System.FormatException e = new System.FormatException();
                    MessageBox.Show("Please make sure all feilds are filled");
                }

            }

            //Express Edit
            else if (lstView_Express.SelectedItem != null)
            {
                try //null check try catch
                {
                    int serviceTagValue = ServiceTag.Value.Value;

                    ListViewItem selectedItem = (ListViewItem)lstView_Express.SelectedItem;
                    int index = lstView_Express.Items.IndexOf(selectedItem);
                    Drone selectedDrone = ExpressQueue.ElementAt(index);

                    selectedDrone.ClientName = txtClientName.Text;
                    selectedDrone.DroneModel = txtDroneModel.Text;
                    selectedDrone.ServiceProblem = txtSerProblem.Text;
                    selectedDrone.ServiceCost = double.Parse(txtSerCost.Text);
                    selectedDrone.ServiceTag = ServiceTag.Value.Value;
                    StatusBar("Item has been successfully edited");

                    if (RdoStandard.IsChecked == true)
                    {
                        selectedDrone.ServiceCost /= 1.15;
                        StandardQueue.Enqueue(selectedDrone);
                        ExpressQueue = new Queue<Drone>(ExpressQueue.Where((item, i) => i != index));
                        StatusBar("Item has been successfully edited");
                    }

                    Display(lstView_Standard, StandardQueue);
                    Display(lstView_Express, ExpressQueue);
                }
                catch
                {
                    System.FormatException e = new System.FormatException();
                    MessageBox.Show("Please make sure all feilds are filled");
                }
            }
        }

        #endregion

        //Delete  Method
        #region
        private void DeleteSelectedItem()
        {

            if (lstView_Standard.SelectedItem != null)
            {
                ListViewItem selectedItem = (ListViewItem)lstView_Standard.SelectedItem;
                int index = lstView_Standard.Items.IndexOf(selectedItem);
                StandardQueue = new Queue<Drone>(StandardQueue.Where((item, i) => i != index));
                StatusBar("Item has been successfully deleted");
                Display(lstView_Standard, StandardQueue);
                ClearTextBoxes();
            }

            else if (lstView_Express.SelectedItem != null)
            {
                ListViewItem selectedItem = (ListViewItem)lstView_Express.SelectedItem;
                int index = lstView_Express.Items.IndexOf(selectedItem);
                ExpressQueue = new Queue<Drone>(ExpressQueue.Where((item, i) => i != index));
                StatusBar("Item has been successfully deleted");
                Display(lstView_Express, ExpressQueue);
                ClearTextBoxes();
            }
            else if (lstView_Finished.SelectedItem != null)
            {
                StatusBar("Action cancelled - You can't delete that item");
            }
        }
        #endregion

        //Clear Textboxes
        #region
        private void ClearTextBoxes()
        {
            int serviceTagValue = ServiceTag.Value.Value;
            txtClientName.Clear();
            txtDroneModel.Clear();
            txtSerProblem.Clear();
            txtSerCost.Clear();


            if (ServiceTag.Value > 900)
            {
                ServiceTag.Value = 900;
            }
            RdoStandard.IsChecked = false;
            RdoExpress.IsChecked = false;
        }
        #endregion

        //Move to Finished
        #region
        private void MoveToFinished()
        {
            Drone item = null;
            if (lstView_Standard.SelectedItem != null)
            {
                ListViewItem selectedItem = (ListViewItem)lstView_Standard.SelectedItem;
                int index = lstView_Standard.Items.IndexOf(selectedItem);

                if (index == 0)
                {
                    item = StandardQueue.Dequeue();
                }
            }

            else if (lstView_Express.SelectedItem != null)
            {
                ListViewItem selectedItem = (ListViewItem)lstView_Express.SelectedItem;
                int index = lstView_Express.Items.IndexOf(selectedItem);

                if (index == 0)
                {
                    item = ExpressQueue.Dequeue();
                }
            }

            if (item != null)
            {
                FinishedList.Add(item);
                StatusBar("Job completed");
                //Refresh
                Display(lstView_Standard, StandardQueue);
                Display(lstView_Express, ExpressQueue);
                Display(lstView_Finished, FinishedList);
            }
        }
        #endregion


        //Client Paid
        #region
        private void ClientPaid()
        {
            if (lstView_Finished.SelectedItem != null)
            {
                ListViewItem selectedItem = (ListViewItem)lstView_Finished.SelectedItem;
                int index = lstView_Finished.Items.IndexOf(selectedItem);
                FinishedList.RemoveAt(index);
                Display(lstView_Finished, FinishedList);
            }
        }
        #endregion

        //Button clicks
        #region
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

        //unused
        #region
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
        #endregion
        #endregion

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
            var standardDrone = (lstView_Standard.SelectedItem as ListViewItem)?.Tag as Drone;
            var expressDrone = (lstView_Express.SelectedItem as ListViewItem)?.Tag as Drone;
            var finishedDrone = (lstView_Finished.SelectedItem as ListViewItem)?.Tag as Drone;
            int serviceTagValue = ServiceTag.Value.Value;


            if (standardDrone != null)
            {
                txtClientName.Text = standardDrone.ClientName;
                txtDroneModel.Text = standardDrone.DroneModel;
                txtSerProblem.Text = standardDrone.ServiceProblem;
                txtSerCost.Text = standardDrone.ServiceCost.ToString();
                ServiceTag.Value = standardDrone.ServiceTag;
                RdoStandard.IsChecked = true;
            }
            else if (expressDrone != null)
            {
                txtClientName.Text = expressDrone.ClientName;
                txtDroneModel.Text = expressDrone.DroneModel;
                txtSerProblem.Text = expressDrone.ServiceProblem;
                txtSerCost.Text = expressDrone.ServiceCost.ToString();
                ServiceTag.Value = expressDrone.ServiceTag;
                RdoExpress.IsChecked = true;
            }
            else if (finishedDrone != null)
            {
                txtClientName.Text = finishedDrone.ClientName;
                txtDroneModel.Text = finishedDrone.DroneModel;
                txtSerProblem.Text = finishedDrone.ServiceProblem;
                txtSerCost.Text = finishedDrone.ServiceCost.ToString();
                ServiceTag.Value = finishedDrone.ServiceTag;
                RdoStandard.IsChecked = false;
                RdoExpress.IsChecked = false;
            }
            StatusBar("All fields poplated - Waiting for next action");
        }
        private void minusButton(object sender, RoutedEventArgs e)
        {
            int currentValue = int.Parse(ServiceTag.Text);
            int newValue = currentValue - 10;
            newValue = Math.Max(newValue, 100); // Ensure the new value is not less than 100
            ServiceTag.Text = newValue.ToString();
        }

        private void plusButton(object sender, RoutedEventArgs e)
        {
            int currentValue = int.Parse(ServiceTag.Text);
            int newValue = currentValue + 10;
            newValue = Math.Min(newValue, 900); // Ensure the new value is not greater than 900
            ServiceTag.Text = newValue.ToString();
        }

    }
}
