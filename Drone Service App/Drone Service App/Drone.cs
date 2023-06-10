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
        // Private attributes to hold the data for each drone
        private string clientName;
        private string droneModel;
        private string serviceProblem;
        private double serviceCost;
        private int serviceTag;

        // Public getter and setter methods for the clientName attribute
        public string ClientName
        {
            get { return clientName; }
            set { clientName = ToTitleCase(value); } // Format the input as Title case
        }

        // Public getter and setter methods for the droneModel attribute
        public string DroneModel
        {
            get { return droneModel; }
            set { droneModel = value; }
        }

        // Public getter and setter methods for the serviceProblem attribute
        public string ServiceProblem
        {
            get { return serviceProblem; }
            set { serviceProblem = ToSentenceCase(value); } // Format the input as Sentence case
        }

        // Public getter and setter methods for the serviceCost attribute
        public double ServiceCost
        {
            get { return serviceCost; }
            set { serviceCost = value; }
        }

        // Public getter and setter methods for the serviceTag attribute
        public int ServiceTag
        {
            get { return serviceTag; }
            set { serviceTag = value; }
        }

        // Public method to return a string for Client Name and Service Cost
        public string Display()
        {
            return $"{ClientName} - ${ServiceCost}";
        }

        // Private method to format a string as Title case
        private string ToTitleCase(string input)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
        }

        // Private method to format a string as Sentence case
        private string ToSentenceCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input[0].ToString().ToUpper() + input.Substring(1).ToLower();
        }
    }
}
