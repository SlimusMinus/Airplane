using System;
using static System.Console;

namespace Airplane_exam
{
    public delegate void Flydelegate(int speed, int height);
    public delegate void DelegPoints(int recomend_heidht, int height, int height_comparison);

    class Airplane
    {

        protected int speed;
        public int Myspeed
        {
            get { return speed; }
            set { speed = value; }
        }
        protected int height;

        public int Myheight
        {
            get { return height; }
            set { height = value; }
        }
        private int points;

        public int Mypoints
        {
            get { return points; }
            set { points = value; }
        }

        public void Flight_correction(int speed, int height)
        {
            SetCursorPosition(0, 0);
            WriteLine("***********************************************************");
            WriteLine($"скорость самолёта составляет {speed} км*ч высота {height} км");
            WriteLine("***********************************************************");

        }

        public void Points_penal(int recomend_heidht, int height, int height_comparison)
        {
            SetCursorPosition(0, 5);
            if (recomend_heidht != height && height_comparison > 300 && height_comparison <= 600)
            {
                WriteLine($"Штрафные баллы - {points += 25}");
                WriteLine("***********************************************************");

            }
            else if (recomend_heidht != height && height_comparison > 600 && height_comparison < 1000)
            {
                WriteLine($"Штрафные баллы - {points += 50}");
                WriteLine("***********************************************************");
            }
            try
            {
                if (points > 1000)
                    throw new Exception("Непригоден к полетам, вы набрали штафных баллов более 1000");
            }
            catch (Exception ex)
            {
                Clear();
                WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        public Airplane(int speed, int height, int points)
        {
            this.speed = speed;
            this.height = height;
            this.points = points;
        }
        public void Penalty_points_height(int height, int height_comparison)
        {
            try
            {
                if (height > 1000)
                    throw new Exception("Непригоден к полетам, вы набрали недопустимую высоту");
                if (height_comparison > 1300)
                    throw new Exception("Самолет разбился, разница между рекомендуемой и текущей высотой более 1300 км");
            }
            catch (Exception ex)
            {
                Clear();
                WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

    }

    class Dispatcher
    {
        public string name { get; set; }
        public Dispatcher(string name)
        {
            this.name=name;
        }
        public event Flydelegate fly_event;
        public event DelegPoints delegpoints;

        public void Flight_correction(int speed, int height)
        {
            if (fly_event!=null)
            {
                fly_event(speed, height);
            }
        }

        public void PointsEvent(int recomend_heidht, int height, int height_comparison)
        {
            if (delegpoints!=null)
            {
                delegpoints(recomend_heidht, height, height_comparison);
            }
        }

        public void Exeptions_null(int speed_or_height)
        {
            try
            {
                if (speed_or_height <= 0)
                    throw new Exception("Самолет разбился, скорость или высота не должны быть равными 0");
            }
            catch (Exception ex)
            {
                Clear();
                WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }
       
    }

    internal class Program
    {

        public static void Main(string[] args)
        {
            BackgroundColor = ConsoleColor.White;
            ForegroundColor = ConsoleColor.DarkBlue;
            Clear();

            int height = 0, speed = 0, distance = 0, points = 0, points2 = 0, height_comparison = 0, recomend_heidht = 0;
            Random random = new Random();
            //корректировка погодных условий 
            int wether = random.Next(-200, 200);
            int wether2 = random.Next(-200, 200);

            ConsoleKeyInfo key;

            Airplane air = new Airplane(height, speed, points);

            WriteLine("Введите имя первого диспетчера");
            string name1 = ReadLine();

            WriteLine("Введите имя второго диспетчера");
            string name2 = ReadLine();
            Clear();

            WriteLine($"Управление самолетом переходит диспетчеру {name1}");
            WriteLine("Начните полет с набора скорости на взлетной полосе клавишой ->");

            int x = 10;
            int y = 50;

            do
            {
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.RightArrow)
                    air.Myspeed += 50;
                else
                    WriteLine("Вы начинаете движение самолета не с той кнопки, повторите ввод");

            } while (key.Key != ConsoleKey.RightArrow);
            Clear();
            WriteLine("Вы начинаете набор скорости и высоты");
            do
            {

                if (x < 0)
                    x = 0;
                else if (x >= BufferWidth)
                    x = BufferWidth - 1;

                if (y < 0)
                    y = 0;
                else if (y >= BufferHeight)
                    y = BufferHeight - 1;

                Clear();
                SetCursorPosition(x, y);
                WriteLine("^--/-->");


                Dispatcher dispatcher1 = new Dispatcher(name1);
                //подписка на событие вывода на консоль текущей скорости и высоты
                dispatcher1.fly_event += air.Flight_correction;
                switch (Console.ReadKey().Key)
                {

                    case ConsoleKey.UpArrow:
                        {
                            //увеличение высоты 
                            air.Myheight += 50;
                            //событие вывода на консоль текущей скорости и высоты
                            dispatcher1.Flight_correction(air.Myspeed, air.Myheight);

                            WriteLine($"Рекомендуемая высота {recomend_heidht = 4 * air.Myspeed - wether}");
                            WriteLine("***********************************************************");
                            //разница между рекомендуемой высотой и текущей
                            height_comparison = recomend_heidht - air.Myheight;
                            if (height_comparison < 0)
                                height_comparison*=-1;
                            //метод класса по выоте более 1000 км и разницей между текущей и рекомендуемой высотой
                            air.Penalty_points_height(air.Myheight, height_comparison);
                            //увеличение дистанции
                            distance += 50;
                            //перемещение самолета по вертикали
                            y-=2;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        {
                            if (air.Myheight <= 0)
                                dispatcher1.Exeptions_null(air.Myheight);
                            else
                            {
                                air.Myheight -= 50;

                                dispatcher1.Flight_correction(air.Myspeed, air.Myheight);

                                WriteLine($"Рекомендуемая высота {recomend_heidht = 4 * air.Myspeed - wether}");
                                WriteLine("***********************************************************");

                                height_comparison = recomend_heidht - air.Myheight;
                                if (height_comparison < 0)
                                    height_comparison*=-1;

                                air.Penalty_points_height(air.Myheight, height_comparison);

                            }
                            distance += 50;
                            y+=2;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        {
                            air.Myspeed += 50;

                            dispatcher1.Flight_correction(air.Myspeed, air.Myheight);
                            WriteLine($"Рекомендуемая высота {recomend_heidht = 4 * air.Myspeed - wether}");
                            WriteLine("***********************************************************");

                            height_comparison = recomend_heidht - air.Myheight;

                            if (height_comparison < 0)
                                height_comparison*=-1;
                            air.Penalty_points_height(air.Myheight, height_comparison);

                            if (air.Myspeed > 1000)
                                WriteLine($"Немедленно снизьте скорость, штрафные баллы - {air.Mypoints += 100}");
                            distance += 50;
                            x+=4;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        {
                            if (air.Myspeed <= 0)
                                dispatcher1.Exeptions_null(air.Myspeed);
                            else
                            {
                                air.Myspeed -= 50;
                                dispatcher1.Flight_correction(air.Myspeed, air.Myheight);
                            }
                            dispatcher1.Flight_correction(air.Myspeed, air.Myheight);
                            WriteLine($"Рекомендуемая высота {recomend_heidht = 4 * air.Myspeed - wether}");
                            WriteLine("***********************************************************");

                            height_comparison = recomend_heidht - air.Myheight;

                            if (height_comparison < 0)
                                height_comparison*=-1;

                            air.Penalty_points_height(air.Myheight, height_comparison);

                            distance += 50;
                            x-=4;
                        }
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
                //подписка на событие штрафных баллов
                dispatcher1.delegpoints += air.Points_penal;
                //событие штрафных быллов
                air.Points_penal(recomend_heidht, air.Myheight, height_comparison);
                points = air.Mypoints;
                ReadKey();

            } while (distance != 500);
            Clear();
            WriteLine("**************************************************************************************");
            WriteLine($"Вы пролетели {distance} км и теперь управление самолетом переходит диспетчеру {name2}");
            WriteLine("**************************************************************************************");

            ReadKey();
            do
            {
                if (x < 0)
                    x = 0;
                else if (x >= BufferWidth)
                    x = BufferWidth - 1;

                if (y < 0)
                    y = 0;
                else if (y >= BufferHeight)
                    y = BufferHeight - 1;

                Clear();
                SetCursorPosition(x, y);
                WriteLine("^--/-->");

                Dispatcher dispatcher2 = new Dispatcher(name2);
                dispatcher2.fly_event += air.Flight_correction;
                wether = random.Next(-200, 200);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            air.Myheight += 50;

                            dispatcher2.Flight_correction(air.Myspeed, air.Myheight);

                            WriteLine($"Рекомендуемая высота {recomend_heidht = 4 * air.Myspeed - wether2}");
                            WriteLine("***********************************************************");

                            height_comparison = recomend_heidht - air.Myheight;
                            if (height_comparison < 0)
                                height_comparison*=-1;

                            air.Penalty_points_height(air.Myheight, height_comparison);
                                                       
                            distance += 50;
                            y-=2;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        {
                            if (air.Myheight <= 0)
                                dispatcher2.Exeptions_null(air.Myheight);
                            else
                            {
                                air.Myheight -= 50;

                                dispatcher2.Flight_correction(air.Myspeed, air.Myheight);

                                WriteLine($"Рекомендуемая высота {recomend_heidht = 4 * air.Myspeed - wether2}");
                                WriteLine("***********************************************************");

                                height_comparison = recomend_heidht - air.Myheight;
                                if (height_comparison < 0)
                                    height_comparison*=-1;

                                air.Penalty_points_height(air.Myheight, height_comparison);

                            }
                            distance += 50;
                            y+=2;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        {

                            air.Myspeed += 50;

                            dispatcher2.Flight_correction(air.Myspeed, air.Myheight);
                            WriteLine($"Рекомендуемая высота {recomend_heidht = 4 * air.Myspeed - wether2}");
                            WriteLine("***********************************************************");

                            height_comparison = recomend_heidht - air.Myheight;
                            if (height_comparison < 0)
                                height_comparison*=-1;

                            air.Penalty_points_height(air.Myheight, height_comparison);

                            if (air.Myspeed > 1000)
                                WriteLine($"Немедленно снизьте скорость, штрафные баллы - {air.Mypoints += 100}");
                            distance += 50;
                            x+=4;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        {
                            if (air.Myspeed <= 0)
                                dispatcher2.Exeptions_null(air.Myspeed);
                            else
                            {
                                air.Myspeed -= 50;
                                dispatcher2.Flight_correction(air.Myspeed, air.Myheight);
                            }
                            dispatcher2.Flight_correction(air.Myspeed, air.Myheight);
                            WriteLine($"Рекомендуемая высота {recomend_heidht = 4 * air.Myspeed - wether2}");
                            WriteLine("***********************************************************");

                            height_comparison = recomend_heidht - air.Myheight;

                            if (height_comparison < 0)
                                height_comparison*=-1;

                            air.Penalty_points_height(air.Myheight, height_comparison);

                            distance += 50;
                            x-=4;
                        }
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
                dispatcher2.delegpoints += air.Points_penal;
                air.Points_penal(recomend_heidht, air.Myheight, height_comparison);
                points2 = air.Mypoints;
                ReadKey();

            } while (distance != 950);
            Clear();
            WriteLine("***********************************************************");
            WriteLine("Происходит посадка самолета, снизьте скорость и высоту до 0");
            WriteLine("***********************************************************");
            ReadKey();
            do
            {
                if (x < 0)
                    x = 0;
                else if (x >= BufferWidth)
                    x = BufferWidth - 1;

                if (y < 0)
                    y = 0;
                else if (y >= BufferHeight)
                    y = BufferHeight - 1;

                Clear();
                SetCursorPosition(x, y);
                WriteLine("^--/-->");

                Dispatcher dispatcher2 = new Dispatcher(name2);
                dispatcher2.fly_event += air.Flight_correction;
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            air.Myheight += 150;
                            dispatcher2.Flight_correction(air.Myspeed, air.Myheight);
                            y-=2;;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        {
                            air.Myheight -= 150;
                            if (air.Myheight < 0)
                            {
                                SetCursorPosition(0, 0);
                                WriteLine("Вы уже на земле, снижайте скорость");
                                air.Myheight = 0;
                            }
                            else 
                                dispatcher2.Flight_correction(air.Myspeed, air.Myheight);
                            y+=2;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        {
                            air.Myspeed += 50;
                            dispatcher2.Flight_correction(air.Myspeed, air.Myheight);
                            x+=4;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        {
                            air.Myspeed -= 50;
                            if (air.Myspeed < 0)
                            {
                                SetCursorPosition(0, 0);
                                WriteLine("Скорость равна 0, снижайтесь");
                                air.Myspeed = 0;
                            }
                            else
                                dispatcher2.Flight_correction(air.Myspeed, air.Myheight);
                            x-=4;
                        }
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
                ReadKey();

            } while (air.Myspeed > 0 && air.Myheight > 0);
            Clear();
            WriteLine("******************************************************");
            WriteLine("Программа «Тренажер пилота самолета» завершена успешно");
            WriteLine($"Диспетчер {name1} начислил(а) {points} штрафных баллов");
            WriteLine($"Диспетчер {name2} начислил(а) {points2 - points} штрафных баллов");
            WriteLine("******************************************************");

        }
    }
}
