﻿using System;
using System.Threading;
using CLI_Sharp;

namespace exampleApp
{
    class Program
    {
        // Creating the Logger instance, you have to use Logger's methods
        // for writing to Console instead of Console.Write/WriteLine.
        // Logger class provides methods for: warnings, errors, information, plain text.
        // You can implement your own Logger and add more methods or override the default ones.
        // It is a good idea to make Logger instance static so it can be used from other classes.
        public static readonly Logger Logger = new Logger();
        // You can pass "true" in the constructor to enable logging to file. 
        
        static void Main(string[] args)
        {
            
            // Creating an instance of MyProcessor, this class has to inherit
            // from CommandProcessor and implement processCommand method.
            // Check MyProcessor for implementation example.
            MyProcessor processor = new MyProcessor(); 
            
            // Creating ConsoleDisplay instance.
            ConsoleDisplay display = new ConsoleDisplay(Logger, processor);
            
            // Starting the CLI.
            display.start(); 
            
            // Your program continues as usual.
            // !IMPORTANT! You must keep the program running as CLI_Sharp runs on separate thread
            // therefore when main thread of your program ends the program WILL exit.
            // In this case i'm just gonna make a while loop that uses Logger instance
            // to display some warnings every second.
            while (true)
            {
                Logger.warn("This is a warning.");
                Thread.Sleep(1000);
            }
        }

        // Example method that will be called when user enters "math"
        public static int calculate()
        {
            return 12 * 10;
        }
    }
}