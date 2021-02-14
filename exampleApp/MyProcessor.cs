using CLI_Sharp;

namespace exampleApp
{
    public class MyProcessor : CommandProcessor
    {
        
        // This is where you handle user input that comes from the CLI
        // This class has access to Logger and ConsoleDisplay instances
        // so you can execute methods on these.
        public override void processCommand(string cmd)
        {
            // If user enters "hello" i will use Logger to info "Hello User!".
            if (cmd == "hello")
                logger.info("Hello User!");

            // If user enters "stop" i will use stop() method of the ConsoleDisplay instance.
            // Notice that Logger works independently of CLI it just outputs like Console.Write's. 
            if (cmd == "stop")
                display.stop();
            
            // If user enters "math" i will call static calculate method of Program class
            // and pass it to the Logger.err() method.
            if (cmd == "math")
                logger.err(Program.calculate());
        }
    }
}