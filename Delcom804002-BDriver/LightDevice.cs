using System;
using LibUsbDotNet;
using LibUsbDotNet.Main;

namespace Delcom804002_BDriver
{
    public class LightDevice
    {
        public UsbDevice MyUsbDevice;
        public UsbDeviceFinder MyUsbFinder = new UsbDeviceFinder(0x0FC5, 0x1223);

        public LightDevice()
        {
            if(!GetDelcomBuildLight())
            {
                throw new Exception("Unable to initiate a connection to the device.");
            }
        }

        ~LightDevice()
        {
            DisconnectAndFinalizeDevice();
        }

        private bool GetDelcomBuildLight()
        {
            try
            {
                MyUsbDevice = UsbDevice.OpenUsbDevice(MyUsbFinder);

                // If the device is open and ready
                if (MyUsbDevice == null) throw new Exception("Device Not Found.");

                IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                if (!ReferenceEquals(wholeUsbDevice, null))
                {
                    // This is a "whole" USB device. Before it can be used, 
                    // the desired configuration and interface must be selected.

                    // Select config #1
                    wholeUsbDevice.SetConfiguration(1);

                    // Claim interface #0.
                    wholeUsbDevice.ClaimInterface(0);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void DisconnectAndFinalizeDevice()
        {
            if (MyUsbDevice != null)
            {
                if (MyUsbDevice.IsOpen)
                {
                    TurnOffLight(LightColour.Red);
                    TurnOffLight(LightColour.Orange);
                    TurnOffLight(LightColour.Green);
                    IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                    if (!ReferenceEquals(wholeUsbDevice, null))
                    {
                        // Release interface #0.
                        wholeUsbDevice.ReleaseInterface(0);
                    }
                    MyUsbDevice.Close();
                }
                MyUsbDevice = null;

                // Free usb resources
                UsbDevice.Exit();
            }
        }

        public bool TurnOnLight(LightColour colour)
        {
            var packet = new UsbSetupPacket(0x48, 0x12, (short)0x0c0a, (short)colour, (short)0x0000);
            int temp2;
            return MyUsbDevice.ControlTransfer(ref packet, new byte[0], 0, out temp2);
        }

        public bool TurnOffLight(LightColour colour)
        {
            var modifiedColour = (short)colour * 0x100;
            var packet = new UsbSetupPacket(0x48, 0x12, (short)0x0c0a, (short)modifiedColour, (short)0x0000);
            int temp2;
            return MyUsbDevice.ControlTransfer(ref packet, new byte[0], 0, out temp2);
        }
    }
}
