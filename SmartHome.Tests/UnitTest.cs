
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SmartHome.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AssertAllColorsAreNotNull()
        {
            var x = Thermostat.UWP.Helpers.Colors.GetAvailableColors();
            Assert.IsNotNull(x);
        }

        [TestMethod]
        public void AssertRandomColorsAreNotNull()
        {
            var x = Thermostat.UWP.Helpers.Colors.GetAvailableColors();
            List<string> colorList = new List<string>();


            Random rand = new Random();

            for (int i = 0; i < x.Count; i++)
            {
                int r = rand.Next(x.Count - 1);
                colorList.Add(x[r]);
            }

            Assert.AreNotEqual(colorList, x);
        }
    }
}
