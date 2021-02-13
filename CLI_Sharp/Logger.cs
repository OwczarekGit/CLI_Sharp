using System;
using System.IO;

namespace CLI_Sharp
{
    public class Logger
    {
        private ConsoleDisplay display;
        public bool writeToDisplay = false;
        public bool logToFile = false;
        public String logFileName = $"{Directory.GetCurrentDirectory()}/log-{DateTime.Now.ToString("hh-mm-ss")}.txt";

        public Logger(){}

        public Logger(bool logToFile)
        {
            this.logToFile = logToFile;
        }

        public void setDisplay(ConsoleDisplay display)
        {
            this.display = display;
            writeToDisplay = true;
        }

        public virtual void err(object s)
        {
            write($"[E][{getTimeNow()}] {s}");
        }

        public virtual void warn(object s)
        {
             write($"[W][{getTimeNow()}] {s}");
        }

        public virtual void info(object s)
        {
             write($"[I][{getTimeNow()}] {s}");
        }
        
        public virtual void user(object s)
        {
             write($"[U][{getTimeNow()}] {s}");
        }

        public void plain(object s)
        {
            write(s);
        }

        public void write(object s)
        {
            if (display == null || !writeToDisplay || !display.running)
            {
                Console.WriteLine(s);
            }
            else
            {
                display.addToDisplay(s.ToString());
            }

            if (logToFile)
            {
                //TODO Logging to file.
            }
        }

        private String getTimeNow()
        {
            return DateTime.Now.ToString("hh:mm:ss");
        }
    }
}