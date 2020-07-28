# WizLightUniversal
WizLightUniversal is (rather, will be) a cross-platform desktop app for controlling Wiz brand lights.
It uses my OpenWiz library to connect to lights on LAN.

Currently, only BR30 LED color lights are confirmed working with this app. It (should) be trivial to add support for other lights (if they aren't already supported). If you have other Wiz brand lights, please do share your discoveries.

## Credits
Big thank you to Konrad Müller for the template code. See their blog post here:
https://medium.com/@k.l.mueller/cross-platform-tray-bar-applications-with-xamarin-forms-bbd6c1b7f17a

## Status
The application hides in the system tray, much like Dropbox. To open it, simply left-click the tray icon to spawn a window. The window keeps track of the last page used.
The home page shows you all lights on your network. Clicking the gear icon will navigate to the preferences page, where you must enter your Home ID. Clicking the refresh icon will attempt to refresh the list of lights on LAN. Selecting a light will navigate you to the control page, where you may modify color, temperature, and/or brightness.
### Windows 10
The system tray item is an icon that reacts to changes in the system theme. No idea how the app reacts in another other Windows OS. A window is drawn at the corner where the icon rests. To kill, right-click on the icon and select "Quit."
### macOS
Works similarly to above, except the tray icon spawns a standard macOS Popover enclosing a similar GUI. All functionality is present.
