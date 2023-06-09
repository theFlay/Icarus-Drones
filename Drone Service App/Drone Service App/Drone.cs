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

        #region Setters and Getters
        public string ClientName
        {
            get { return clientName; }
            set { clientName = ToTitleCase(value); }
        }

        public string DroneModel
        {
            get { return droneModel; }
            set { droneModel = value; }
        }

        public string ServiceProblem
        {
            get { return serviceProblem; }
            set { serviceProblem = ToSentenceCase(value); }
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
        #endregion

        public string Display()
        {
            return $"{ClientName} - ${ServiceCost}";
        }

        #region Input Formatting
        private string ToTitleCase(string input)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
        }

        private string ToSentenceCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input[0].ToString().ToUpper() + input.Substring(1).ToLower();
        }
        #endregion
    }
}