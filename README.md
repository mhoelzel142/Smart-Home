# Smart Home
A smart home dashboard for Windows 10 IoT on Raspberry Pi
#
This simple smart home dashboard is designed for Raspberry Pi devices running Windows 10 IoT Core. 

Currently, the dashboard only supports temperature and humidity sensors, but more functionality will be added as I get time (and build new sensors). 

The UI is created using Universal Windows Platform (UWP) and .Net Core. There are 3 projects:

Thermostat.Migrations - this project is used for running migration commands using Entity Framework Code-First

Thermostat.Model - This is where the models reside. Models are accessed from the UWP project, and the database is updated from the Migrations project. 

Thermostat.UWP - This is the GUI. It is designed to run on the official Raspberry Pi 7" touch screen, so UI elements are scaled for display on an 800x480 screen. 

Contributions are welcome - I'm still learning UWP and C#, so this project is an ongoing learning experience for me. 
