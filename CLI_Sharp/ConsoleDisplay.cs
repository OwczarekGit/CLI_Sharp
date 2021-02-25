using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CLI_Sharp
{
    public class ConsoleDisplay
    {
        private Vec2 size = new Vec2(0,0);
        private bool dynamicSize = false;
        public bool running { get; private set; } = false;
        public String title = "CLI";
        private Thread inputThread;
        private Thread updateThread;
        public bool dynamicRefresh = true;
        private String inputBuffer = "";
        private Logger logger;
        private CommandProcessor processor;
        public Queue<String> displayBuffer { get; } = new Queue<string>();
        private DateTime startTime = DateTime.Now;
        private Queue<String> history = new Queue<string>();
        private int historyIndex = 0;
        private short delay = 1000;
        
        public ConsoleDisplay(Logger logger, CommandProcessor processor)
        {
            this.processor = processor;
            this.logger = logger;
            this.logger.setDisplay(this);
            this.processor.setLogger(this.logger);
            this.processor.setDisplay(this);
            Console.OutputEncoding = Encoding.UTF8;
            dynamicSize = true;
            
            displayBuffer.Enqueue("[-- Hello World! --]");
        }

        public ConsoleDisplay(Logger logger,CommandProcessor processor, Vec2 size) : this(logger, processor)
        {
            this.size = size;
            dynamicSize = false;
        }

        private void displayUpdater()
        {
            while (running)
            {
                Thread.Sleep(1);

                if (delay <= 0)
                {
                    if (dynamicRefresh)
                    {
                        forceRedraw();
                    }
                    delay = 1000;
                }
                else
                {
                    
                    delay--;
                }
            }
        }
        
        public void addToDisplay(String s)
        {
            String[] tmp = s.Split('\n');

            lock (displayBuffer)
            {
                foreach (var line in tmp)
                {
                    if (displayBuffer.Count > size.y - 9)
                    {
                        displayBuffer.Dequeue();
                    }

                    displayBuffer.Enqueue(line);
                }
            }
        }

        private void updateSize()
        {
            if (!dynamicSize)
                return;

            size.x = Console.WindowWidth-1;
            size.y = Console.WindowHeight-1;
        }

        public void forceRedraw()
        {
            draw();
        }

        public void start()
        {
            Console.CursorVisible = false;
            updateSize();
            forceRedraw();
            if (!running)
            {
                running = true;
                updateThread = new Thread(displayUpdater);
                inputThread = new Thread(getUserInput);
                inputThread.Start();
                updateThread.Start();
            }
        }

        public void stop()
        {
            running = false;
            clearDisplay();
            Console.CursorVisible = true;
        }

        private void getUserInput()
        {
            while (running)
            {
                var input = Console.ReadKey(true);

                lock (displayBuffer)
                {
                    if (input.Key == ConsoleKey.UpArrow)
                    {
                        if (historyIndex > 0)
                        {
                            historyIndex--;
                            inputBuffer = history.ToArray()[historyIndex];
                        }
                    }
                    else if (input.Key == ConsoleKey.DownArrow)
                    {
                        if (historyIndex < history.Count - 1)
                        {
                            historyIndex++;
                            inputBuffer = history.ToArray()[historyIndex];
                        }
                    }
                    else
                    {
                        if (input.Key == ConsoleKey.Delete)
                        {
                            inputBuffer = "";
                        }
                        else
                        {
                            if (input.Key == ConsoleKey.Enter)
                            {
                                processCommand();
                                inputBuffer = "";
                            }
                            else
                            {
                                if (input.Key == ConsoleKey.Backspace)
                                {
                                    if (inputBuffer.Length > 0)
                                    {
                                        inputBuffer = inputBuffer.Remove(inputBuffer.Length - 1);
                                    }
                                }
                                else
                                {
                                    if (input.Key != ConsoleKey.Tab)
                                    {
                                        inputBuffer += input.KeyChar;
                                    }
                                }
                            }
                        }
                    }
                }
                forceRedraw();
            }
            
        }

        private void processCommand()
        {
            if (inputBuffer.Length > 0)
            {
                if (history.Count > 40)
                {
                    history.Dequeue();
                }

                history.Enqueue(inputBuffer);
            }

            historyIndex = history.Count;
            processor.processCommand(inputBuffer);
        }

        private void draw()
        {
            try
            {
                updateSize();
                clearDisplay();
                drawFrame();
                drawTime();
                drawTitle();
                drawInput();
                drawDisplay();
            }
            catch (Exception e)
            {
            }
        }

        private void clearDisplay()
        {
            Console.Clear();
        }

        private void setPosition(Vec2 pos)
        {
            Console.SetCursorPosition(pos.x, pos.y);
        }

        private void drawFrame()
        {
            setPosition(new Vec2(0,0));
            drawVLines(size.y-1,'|');
            
            setPosition(new Vec2(0, 0));
            drawHLine(size.x, '=');
            
            setPosition(new Vec2(0, size.y-1));
            drawHLine(size.x, '=');
            
            setPosition(new Vec2(0,size.y-5));
            drawHLine(size.x,'=');
            
            setPosition(new Vec2(0,0));
            Console.Write('O');
            
            setPosition(new Vec2(size.x-1,0));
            Console.Write('O');
            
            setPosition(new Vec2(0,size.y-1));
            Console.Write('O');
            
            setPosition(new Vec2(size.x-1,size.y-1));
            Console.Write('O');
            
            setPosition(new Vec2(0,size.y-5));
            Console.Write('O');
            
            setPosition(new Vec2(size.x-1,size.y-5));
            Console.Write('O');
        }

        private void drawInput()
        {
            String tmp = $"[Command({inputBuffer.Length})> {inputBuffer}";
            setPosition(new Vec2(2, size.y-3));
            Console.Write(tmp);
            drawHLine(size.x-5-tmp.Length,'_');
            Console.Write(']');
        }

        private void drawHLine(int len, Char c)
        {
            String tmp = String.Empty;
            
            for (int i = 0; i < len; i++)
            {
                tmp += c;
            }

            Console.Write(tmp);
        }

        private void drawVLines(int len, Char c)
        {
            String tmp = $"{c}";
            
            for (int i = 0; i < size.x-2; i++)
            {
                tmp += ' ';
            }

            tmp += c;

            for (int i = 0; i < len; i++)
            {
               Console.Write(tmp); 
               Console.SetCursorPosition(0, Console.CursorTop+1);
            }
        }

        private void drawTime()
        {
            String uptime = DateTime.Now.Subtract(startTime).ToString("c").Remove(8);
            
            String tmp = $"[ UPTIME: {uptime} ]=[ {DateTime.Now.ToString("hh:mm:ss")} ]";
            Console.SetCursorPosition(size.x-2-tmp.Length, size.y-1);
            Console.Write(tmp);
        }

        private void drawTitle()
        {
            String tmp = $"[ {title} ]";
            Console.SetCursorPosition(2,0);
            Console.Write(tmp);
        }

        private void drawDisplay()
        {
            if (displayBuffer.Count == 0)
                return;

            var tmpArr = displayBuffer.ToArray();
            for (int i = 0; i < tmpArr.Length; i++)
            {
                if (2+i < size.y-6)
                {
                    String tmp = tmpArr[i];
                    
                    if (tmp.Length > size.x-4)
                    {
                        tmp = tmp.Remove(size.x-7);
                        tmp += "...";
                    }
                    
                    setPosition(new Vec2(2, 2+i)); 
                    Console.Write(tmp);
                }
            }
        }
    }
}
