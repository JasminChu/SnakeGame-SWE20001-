using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Media;
using System.Windows.Forms;

namespace Snake
{
    struct Position
    {
        public int row;
        public int col;
        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //JASMINNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN
            //backgorund sound is played when the player start the game
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "/mainmenu.wav";
            player.Play();


            byte right = 0;
            byte left = 1;
            byte down = 2;
            byte up = 3;
            int lastFoodTime = 0;
            int foodDissapearTime = 8000;
            int negativePoints = 0;
            //-------------------------------------life---------------------------------
            int life = 3;
            //Array which is a linear data structure is used 
            //position store direction (array)
            Position[] directions = new Position[]
            {
                    new Position(0, 1), // right
                    new Position(0, -1), // left
                    new Position(1, 0), // down
                    new Position(-1, 0), // up
            };
            double sleepTime = 100;
            int direction = right;
            Random randomNumbersGenerator = new Random();
            Console.BufferHeight = Console.WindowHeight;
            lastFoodTime = Environment.TickCount;

            //Linked List which is a linear data structure is used 
            //Creating a linkedlist 
            //Using List class 
            //list to store position of obstacles
            //JASMINNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN
            //The obstacles are randomizd so it will appear randomly everytime user play it
            List<Position> obstacles = new List<Position>()
                    {
                        new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                            randomNumbersGenerator.Next(0, Console.WindowWidth)),
                        new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                            randomNumbersGenerator.Next(0, Console.WindowWidth)),
                        new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                            randomNumbersGenerator.Next(0, Console.WindowWidth)),
                        new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                            randomNumbersGenerator.Next(0, Console.WindowWidth))
                    };

            //For each loop
            //Each obstacle in List(obstacles) to set the color, position
            //Print out the obstacle
            foreach (Position obstacle in obstacles)
            {

                //drawing obstacles
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(obstacle.col, obstacle.row);
                Console.Write("=");
            }

            //creating snake body (5 "*")
            //Queue which is a linear data structure is used
            //Queue is like a container
            //Enqueue is implementation of Queue to insert new element at the rear of the queue
            //Set 5 items in snakeElements by setting the position (0,i)
            //i increase every time until 5
            //snakeElements used to store the snake body elements (*)
            //JASMINNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN
            //Reduce the body length of snake to 3 units of * 
            Queue<Position> snakeElements = new Queue<Position>();
            for (int i = 0; i <= 3; i++)
            {
                snakeElements.Enqueue(new Position(0, i));
            }

            //The position is create randomly
            //creating food in the game
            Position food;
            do
            {
                food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                    randomNumbersGenerator.Next(0, Console.WindowWidth));
            }
            //new food will be created if snake eat food OR obstacle has the same position with food
            while (snakeElements.Contains(food) || obstacles.Contains(food));
            Console.SetCursorPosition(food.col, food.row);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("@");

            //drawing snake body ("*")
            //set color and position of each of the part of body in snakeElements
            foreach (Position position in snakeElements)
            {
                Console.SetCursorPosition(position.col, position.row);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("*");
            }

            //The following code will run until the program stop
            while (true)
            {
                negativePoints++;

                //the movement of the snake
                //When the user click the arrow key, if the snake direction is not same,
                //it will change the snake direction
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();
                    if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        if (direction != right) direction = left;
                    }
                    if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        if (direction != left) direction = right;
                    }
                    if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        if (direction != down) direction = up;
                    }
                    if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        if (direction != up) direction = down;
                    }
                }

                //manage the position of the snake head if the snake exceed the width or height of the console window
                //if the snake disappear at the bottom, it will reappear from the top
                Position snakeHead = snakeElements.Last();
                Position nextDirection = directions[direction];

                Position snakeNewHead = new Position(snakeHead.row + nextDirection.row,
                    snakeHead.col + nextDirection.col);

                if (snakeNewHead.col < 0) snakeNewHead.col = Console.WindowWidth - 1;
                if (snakeNewHead.row < 0) snakeNewHead.row = Console.WindowHeight - 1;
                if (snakeNewHead.row >= Console.WindowHeight) snakeNewHead.row = 0;
                if (snakeNewHead.col >= Console.WindowWidth) snakeNewHead.col = 0;

                //the game will over if the snake eat its body OR eat the obstacles
                //Stack which is a linear data structure is used
                if (snakeElements.Contains(snakeNewHead) || obstacles.Contains(snakeNewHead))
                {
                    //JASMINNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN
                    //Game over sound will display if the snake die
                    SoundPlayer player1 = new SoundPlayer();
                    player1.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "/die.wav";
                    player1.PlaySync();

                    //Remove the obstacles which the snake has eaten
                    obstacles.Remove(snakeNewHead);

                    //If the life is greater than 0, when the snake eat the obstacle, the life will minus
                    //New obstacle will be randomize
                    if(life > 0)
                    {
                        life -= 1;

                        Position obstacle = new Position();
                        //generate new position for the obstacles
                        do
                        {
                            obstacle = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                                randomNumbersGenerator.Next(0, Console.WindowWidth));
                        } while (snakeElements.Contains(obstacle) || //if snake eat the obstacle
                        obstacles.Contains(obstacle) ||         //if obstacles appear at the same position
                        (food.row != obstacle.row && food.col != obstacle.row));
                        //the position of food and obstacle is different
                        obstacles.Add(obstacle); //then obstacle will be generated
                        Console.SetCursorPosition(obstacle.col, obstacle.row);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("=");

                        if (player1.IsLoadCompleted == true)
                        {
                            player.Play();
                        }
                    }
                    else
                    {
                        //displayed when game over
                        //--------------------------------------GameOver-------------------------------------------
                        Console.ForegroundColor = ConsoleColor.Red;
                        string gameover = "Game over!";
                        string points = "Your points are: {0}";
                        int height = decimal.ToInt32((Console.WindowHeight) / 2);
                        int width = decimal.ToInt32((Console.WindowWidth - gameover.Length) / 2);
                        int userPoints = (snakeElements.Count - 6) * 100 - negativePoints;
                        userPoints -= ((3 - life) * 10);
                        //if (userPoints < 0) userPoints = 0;
                        userPoints = Math.Max(userPoints, 0);

                        Console.SetCursorPosition(width, height);
                        Console.WriteLine(gameover);

                        width = decimal.ToInt32((Console.WindowWidth - points.Length) / 2);
                        height = decimal.ToInt32((Console.WindowHeight) / 2) + 1;

                        Console.SetCursorPosition(width, height);
                        Console.WriteLine(points, userPoints);

                        //JASMINNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN
                        //--------------------------------------Exit Game-------------------------------------------
                        string exit = "Press Enter to exit.";
                        Console.WriteLine(exit);

                        while (Console.ReadKey().Key != ConsoleKey.Enter)
                        {
                            Console.WriteLine(exit);
                        }
                        Environment.Exit(0);
                    }
                }

                //Set the position of the snake
                Console.SetCursorPosition(snakeHead.col, snakeHead.row);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("*");

                //draw the snake head according to different direction
                snakeElements.Enqueue(snakeNewHead);
                Console.SetCursorPosition(snakeNewHead.col, snakeNewHead.row);
                Console.ForegroundColor = ConsoleColor.Gray;
                if (direction == right) Console.Write(">");
                if (direction == left) Console.Write("<");
                if (direction == up) Console.Write("^");
                if (direction == down) Console.Write("v");

                //when the snake eat the food
                if (snakeNewHead.col == food.col && snakeNewHead.row == food.row)
                {

                    //feeding the snake
                    //generate new position for the food
                    do
                    {
                        food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                            randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    //if the snake eat the food or obstacles has the same position with food
                    while (snakeElements.Contains(food) || obstacles.Contains(food));

                    lastFoodTime = Environment.TickCount;
                    Console.SetCursorPosition(food.col, food.row);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("@"); //Creating food
                    sleepTime--;

                    Position obstacle = new Position();
                    //generate new position for the obstacles
                    do
                    {

                        obstacle = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                            randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(obstacle) || //if snake eat the obstacle
                        obstacles.Contains(obstacle) ||         //if obstacles appear at the same position
                        (food.row != obstacle.row && food.col != obstacle.row));
                    //the position of food and obstacle is different
                    obstacles.Add(obstacle); //then obstacle will be generated
                    Console.SetCursorPosition(obstacle.col, obstacle.row);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("=");
                }
                else
                {
                    // moving...if didn't meet the conditions above then the snake will keep moving
                    Position last = snakeElements.Dequeue();
                    //The snake position will be set to the begining of the snakeElements 
                    //“Dequeue” which is used to remove and return the begining object
                    Console.SetCursorPosition(last.col, last.row);
                    Console.Write(" ");
                }

                //If the food appear at the console window (whole game time minus time of last food）
                //is greater than the foodDissapearTime which intialise is 8000
                //------------------------------FoodRelocateTime--------------------------------------
                //add another 5000 time to extend the food relocate time
                if (Environment.TickCount - lastFoodTime >= foodDissapearTime+5000)
                {
                    negativePoints = negativePoints + 50;
                    Console.SetCursorPosition(food.col, food.row); //the cursor position will set to the food position.
                    Console.Write(" ");
                    do
                    {
                        food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight), //create new food
                            randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    //if snake eat food or obstacle and food appear at the same position
                    while (snakeElements.Contains(food) || obstacles.Contains(food));
                    lastFoodTime = Environment.TickCount; //The lastFoodTime will reset to the present time
                }

                Console.SetCursorPosition(food.col, food.row); //creating food
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("@");

                sleepTime -= 0.01;

                Thread.Sleep((int)sleepTime);
            }
        }
    }
}