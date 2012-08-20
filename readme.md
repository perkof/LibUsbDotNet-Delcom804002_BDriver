This is a driver that can be used with LibUsbDotNet to power your Delcom 804002_B usb light device from within a .NET application.

#Usage
Install LibUsbDotNet as your USB device driver for the Delcom 804002_B

Instantiate it in your code.

`LightDevice light = new LightDevice();`

Turn the light on
`light.TurnOnLight(LightColour.Red);`

Turn the light off
`light.TurnOffLight(LightColour.Red);`