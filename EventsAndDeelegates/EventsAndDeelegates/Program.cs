using System;
using System.Threading;

namespace EventsAndDeelegates
{
    class Program
    {
        
        static void Main(string[] args)
        {

            Console.WriteLine("Введите имя 1-ого таймера");
            string name1 = Console.ReadLine();
            Console.WriteLine("Введите отсчетное время 1-ого таймера");
            int time1 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите имя 2-ого таймера");
            string name2 = Console.ReadLine();
            Console.WriteLine("Введите отсчетное время 2-ого таймера");
            int time2 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите имя 3-ого таймера");
            string name3 = Console.ReadLine();
            Console.WriteLine("Введите отсчетное время 3-ого таймера");
            int time3 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Каким образом запустить обработку таймеров:\n 1) Методы\n 2) Анонимные делегаты \n 3) Лямбда выражения");
            int ch = Convert.ToInt32(Console.ReadLine());
            if(ch == 1)
            {

                Methods methods1 = new Methods(name1, time1);
                Methods methods2 = new Methods(name2, time2);
                Methods methods3 = new Methods(name3, time3);
                Thread thread1 = new Thread(new ThreadStart(methods1.Run));
                Thread thread2 = new Thread(new ThreadStart(methods2.Run));
                Thread thread3 = new Thread(new ThreadStart(methods3.Run));
                thread1.Start();
                thread2.Start();
                thread3.Start();

            }
            else if (ch == 2)
            {
                AnonimMethods methods1 = new AnonimMethods(name1, time1);
                AnonimMethods methods2 = new AnonimMethods(name2, time2);
                AnonimMethods methods3 = new AnonimMethods(name3, time3);
                Thread thread1 = new Thread(new ThreadStart(methods1.Run));
                Thread thread2 = new Thread(new ThreadStart(methods2.Run));
                Thread thread3 = new Thread(new ThreadStart(methods3.Run));
                thread1.Start();
                thread2.Start();
                thread3.Start();
            }
            else if(ch == 3)
            {
                Lambda methods1 = new Lambda(name1, time1);
                Lambda methods2 = new Lambda(name2, time2);
                Lambda methods3 = new Lambda(name3, time3);
                Thread thread1 = new Thread(new ThreadStart(methods1.Run));
                Thread thread2 = new Thread(new ThreadStart(methods2.Run));
                Thread thread3 = new Thread(new ThreadStart(methods3.Run));
                thread1.Start();
                thread2.Start();
                thread3.Start();
            }
            else
            {
                Console.WriteLine("Неверный ввод");

            }
            Console.ReadLine();
        }
    }


    interface ICutDownNotifier
    {
        void Init();
        void Run();
    }

    class Events
    {

        int Time;
        public delegate void StartTimer();
        public event StartTimer StartEventMessage;

        public delegate void LeftNSecond(int t);
        public event LeftNSecond LeftSecEventMessage;

        public delegate void TimerEnd();
        public event TimerEnd TimerEndEventMessage;

        public Events(int Time)
        {
            this.Time = Time;
        } 

        public void timer()
        {
            StartEventMessage();
            for (int i = 0; i <= Time; i++)
            {
                int T = Time - i;
                Thread.Sleep(1000);
                LeftSecEventMessage(T);

                if(i == Time)
                {
                    TimerEndEventMessage();
                }
            }
            
        }
    }

    class Methods :  ICutDownNotifier {

        string Name;
        int Time;
        Events events;
        public Methods(string name, int time)
        {
            Name = name;
            Time = time;
            events = new Events(Time);
        }

        public void Init()
        {
            events.StartEventMessage += startMessageMethod;
            events.LeftSecEventMessage += leftMessageMethod;
            events.TimerEndEventMessage += endMessageMethod;
        }

        public void Run()
        {
            Init();
            events.timer();
        }

        public void startMessageMethod()
        {
            
            Console.WriteLine("Таймер " + Name + " начал работу");
        }

        public void leftMessageMethod(int T)
        {

            Console.WriteLine("Таймеру " + Name + " осталось " + T + " секунд");
        }

        public void endMessageMethod()
        {
            Console.WriteLine("Таймер " + Name + " завершил работу");
        }

    }

    class AnonimMethods : ICutDownNotifier
    {
        string Name;
        int Time;
        Events events;
        public AnonimMethods(string name, int time)
        {
            Name = name;
            Time = time;
            events = new Events(Time);
        }

        public void Init()
        {
            events.StartEventMessage += delegate ()
            {
                Console.WriteLine("Таймер " + Name + " начал работу");
            };
            events.LeftSecEventMessage += delegate (int T)
            {
                Console.WriteLine("Таймеру " + Name + " осталось " + T + " секунд");
            };
            events.TimerEndEventMessage += delegate ()
            {
                Console.WriteLine("Таймер " + Name + " завершил работу");
            };

        }

        public void Run()
        {
            Init();
            events.timer();
        }
    }

    class Lambda : ICutDownNotifier
    {
        string Name;
        int Time;
        Events events;
        public Lambda(string name, int time)
        {
            Name = name;
            Time = time;
            events = new Events(Time);
        }

        public void Init()
        {
            events.StartEventMessage += () =>
            {
                Console.WriteLine("Таймер " + Name + " начал работу");
            };
            events.LeftSecEventMessage +=  (int T) =>
            {
                Console.WriteLine("Таймеру " + Name + " осталось " + T + " секунд");
            };
            events.TimerEndEventMessage += () =>
            {
                Console.WriteLine("Таймер " + Name + " завершил работу");
            };

        }
        public void Run()
        {
            Init();
            events.timer();
        }
    }
}
