using System;

namespace CLI_Sharp
{
    public abstract class CommandProcessor
    {
        public ConsoleDisplay display { get; private set; }
        public Logger logger { get; private set; }

        public void setDisplay(ConsoleDisplay display)
        {
            this.display = display;
        }

        public void setLogger(Logger logger)
        {
            this.logger = logger;
        }
        public abstract void processCommand(String cmd);
    }
}