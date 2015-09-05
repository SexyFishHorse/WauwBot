﻿namespace SexyFishHorse.WauwBot
{
    using System;
    using System.Windows.Forms;
    using Ninject;
    using SexyFishHorse.Irc.Client.Configuration;
    using SexyFishHorse.WauwBot.View;

    static class Program
    {
        [STAThread]
        static void Main()
        {
            var kernel = new StandardKernel(new IrcClientModule());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = kernel.Get<WauwBot>();

            Application.Run(form);
        }
    }
}
