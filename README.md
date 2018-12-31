# Smart Home
A smart home dashboard for Windows 10 IoT on Raspberry Pi
#
This simple smart home dashboard is designed for Raspberry Pi devices running Windows 10 IoT Core. 

Currently, the dashboard supports temperature/humidity sensors and 5-day forecasts from OpenWeatherMap. More functionality will be added as I build more sensors - I'm currently exploring wireless relay board (for smart lights) and Speech-To-Text (via CMUSphinx) for voice control functionality. Eventually, I aim to give the smart home a name - Jarvis, perhaps? - and tell it to turn off the lights, and it happens. This functionality will require lots of research, but it's my long-term goal.  

Contributions to the project are welcome - I'm a novice with UWP, so this project is basically an interactive learning experience for me. 

## Getting Started

Thank you for your interest in my smart home app! For full instructions on getting started, check out the wiki or the project's homepage: (link coming soon). 

This project was created using Windows 10 IoT Core, and designed for a Raspberry Pi 3b+ using the official Raspberry Pi 7" touch screen with a resolution of 800 x 480. Using this project on any other device may yeild undesirable results, or worse - some UI elements may not render at all. If you plan to use this on another device, it will almost certainly be necessary to modify the .xaml files to adjust the UI for your device. 

To get started, download the source code and open the .sln in Visual Studio 2017 or above. The solution has several projects, but the main ones are: 

Thermostat.UWP - The UI/front end. This is the GUI of the application, and where most of the logic resides.

Thermostat.Models - This is where the Models reside. Models include information about IoT Devices (sensors, actuators, etc). 

Thermostat.Migrations - This is a .Net Core console application designed for running migrations using Entity Framework code-first. The reason for this project is that UWP applications cannot use Entity Framework Core directly, so a reference was added from the UWP project to the Migrations project. For more details and to learn more about using EF Core with UWP, see the Microsoft docs here: https://docs.microsoft.com/en-us/ef/core/get-started/uwp/getting-started

Once you've downloaded the project, you'll have to create a Resources file (.resw) to store things like API keys. By default, this file is listed in the .gitignore file, so it isn't checked in with commits - this is to keep your API keys secure. You can reference the API keys from your code using ResourceLoader.GetForCurrentView().GetString("Key Name Here"). 

## IMPORTANT:
If you want to use the weather forecast feature, you must obtain an API key from https://openweathermap.org. Create a resources (.resw) file in the root directory of the UWP project and name it Resources.resw. Create a Key named ApiKey and paste your api key as the value. This file is .gitignore'd from the repo to protect the API key. If you fork this project, I'd recommend keeping it this way. 
