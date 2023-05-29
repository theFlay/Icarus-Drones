using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Drone_Service_App
{
	public class Drone
	{
		private string clientName;
		private string droneModel;
		private string serviceProblem;
		private double serviceCost;
		private int serviceTag;

		public string ClientName
		{
			get { return clientName; }
			set { clientName = FormatName(value); }
		}

		public string DroneModel
		{
			get { return droneModel; }
			set { droneModel = value; }
		}

		public string ServiceProblem
		{
			get { return serviceProblem; }
			set { serviceProblem = value; }
		}

		public double ServiceCost
		{
			get { return serviceCost; }
			set { serviceCost = value; }
		}

		public int ServiceTag
		{
			get { return serviceTag; }
			set { serviceTag = value; }
		}

		public string Display()
		{
			return $"{ClientName} - ${ServiceCost}";
		}

		private string FormatName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				return string.Empty;

			string formattedName = char.ToUpper(name[0]) + name.Substring(1).ToLower();
			return formattedName;
		}
	}

	public class DroneServiceApplication
	{
		private Queue<Drone> regularService;
		private Queue<Drone> expressService;
		private List<Drone> finishedList;

		public DroneServiceApplication()
		{
			regularService = new Queue<Drone>();
			expressService = new Queue<Drone>();
			finishedList = new List<Drone>();
		}

		public void AddNewItem(TextBox clientNameTextBox, TextBox droneModelTextBox, TextBox serviceProblemTextBox, TextBox serviceCostTextBox, NumericUpDown serviceTagNumericUpDown, RadioButton regularRadioButton, RadioButton expressRadioButton)
		{
			string clientName = clientNameTextBox.Text;
			string droneModel = droneModelTextBox.Text;
			string serviceProblem = serviceProblemTextBox.Text;
			double serviceCost = double.Parse(serviceCostTextBox.Text);
			int serviceTag = (int)serviceTagNumericUpDown.Value;

			Drone newDrone = new Drone
			{
				ClientName = clientName,
				DroneModel = droneModel,
				ServiceProblem = serviceProblem,
				ServiceCost = serviceCost,
				ServiceTag = serviceTag
			};

			if (expressRadioButton.IsChecked == true)
			{
				serviceCost *= 1.15; // Increase service cost by 15% for express priority
				newDrone.ServiceCost = serviceCost;
				expressService.Enqueue(newDrone);
			}
			else
			{
				regularService.Enqueue(newDrone);
			}

			ClearTextBoxes(clientNameTextBox, droneModelTextBox, serviceProblemTextBox, serviceCostTextBox);
			serviceTagNumericUpDown.Value++; // Increment service tag
		}

		private void ClearTextBoxes(params TextBox[] textBoxes)
		{
			foreach (TextBox textBox in textBoxes)
				textBox.Clear();
		}
	}
}