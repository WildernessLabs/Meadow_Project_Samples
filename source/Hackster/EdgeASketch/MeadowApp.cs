﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.TftSpi;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Rotary;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors.Rotary;
using System;

namespace EdgeASketch
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        int x, y;
        St7789 st7789;
        GraphicsLibrary graphics;
        RotaryEncoderWithButton rotaryX;
        RotaryEncoderWithButton rotaryY;

        public MeadowApp()
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            x = y = 120;

            var config = new SpiClockConfiguration(
                speedKHz: 6000,
                mode: SpiClockConfiguration.Mode.Mode3);
            st7789 = new St7789(
                device: Device,
                spiBus: Device.CreateSpiBus(
                    clock: Device.Pins.SCK,
                    copi: Device.Pins.MOSI,
                    cipo: Device.Pins.MISO,
                    config: config),
                chipSelectPin: null,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240);

            graphics = new GraphicsLibrary(st7789);
            graphics.Clear(true);
            graphics.DrawRectangle(0, 0, 240, 240, Color.White, true);
            graphics.DrawPixel(x, y, Color.Red);
            graphics.Show();

            rotaryX = new RotaryEncoderWithButton(Device,
                Device.Pins.A00, Device.Pins.A01, Device.Pins.A02);
            rotaryX.Rotated += RotaryXRotated;

            rotaryY = new RotaryEncoderWithButton(Device,
            Device.Pins.D02, Device.Pins.D03, Device.Pins.D04);
            rotaryY.Rotated += RotaryYRotated;            
            rotaryY.Clicked += RotaryYClicked;

            led.SetColor(RgbLed.Colors.Green);
        }

        void RotaryXRotated(object sender, RotaryChangeResult e)
        {
            if (e.New == RotationDirection.Clockwise)
                x++;
            else
                x--;

            if (x > 239) x = 239;
            else if (x < 0) x = 0;

            graphics.DrawPixel(x, y + 1, Color.Red);
            graphics.DrawPixel(x, y, Color.Red);
            graphics.DrawPixel(x, y - 1, Color.Red);
            graphics.Show();
        }

        void RotaryYRotated(object sender, RotaryChangeResult e)
        {
            if (e.New == RotationDirection.Clockwise)
                y++;
            else
                y--;

            if (y > 239) y = 239;
            else if (y < 0) y = 0;

            graphics.DrawPixel(x + 1, y, Color.Red);
            graphics.DrawPixel(x, y, Color.Red);
            graphics.DrawPixel(x - 1, y, Color.Red);
            graphics.Show();
        }

        void RotaryYClicked(object sender, EventArgs e)
        {
            x = y = 120;

            graphics.DrawRectangle(0, 0, 240, 240, Color.White, true);
            graphics.DrawPixel(x, y, Color.Red);
            graphics.Show();
        }
    }
}