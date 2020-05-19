using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
           // logger.Info("Program started");
            int selection;
            do
            {
                var mm = new MainMenu();
                selection = mm.GetMainMenuInpput();
                var bm = new BlogMenu();

                //logger.Info("Option {choice} selected", selection);
                bm.Process(selection);
            } while (selection != 7);
        }
    }
}
