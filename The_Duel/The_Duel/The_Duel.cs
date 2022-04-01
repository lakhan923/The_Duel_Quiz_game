using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using System;
using System.Collections.Generic;
using System.IO;


namespace Program
{
    /// @author Tuulia Hynynen & Laiba Khan
    /// @version 1.4.2022
    /// <summary>
    /// This is the main class for the questioning competition between two player.
    /// The player which click first after appeance of a question will answer the question.
    /// The player will see a question and they can click on the answer. 
    /// They get a positive and negative point depending on right on wrong answer respectively.
    /// The score can not be lower than 0
    /// There is a limited time to answer each question.
    /// </summary>
    public class The_Duel : PhysicsGame
    {
        private string[] lines;
        private Label[] answers = new Label[4];
        private Label question = new Label();
        private string correctAnswer;
        private string[] data;
        private IntMeter[] playerPoints = new IntMeter[2];
        private DoubleMeter downCounter;
        private Timer TimeCounter;
        private int playerInTurn;

        /// <summary>
        /// Main method where game execution begins
        /// </summary>
        public override void Begin()
        {
            string questions = "../../../../../questions.txt";
            lines = File.ReadAllLines(questions);
            CreateBoxes();
            int x = RandomGen.NextInt(lines.Length);
            Menu();
            Points();
            ReadTimeCounter();
            GenerateQuestionAndAnswers(x);

            PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
            Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
            Keyboard.Listen(Key.A, ButtonState.Pressed, SelectPlayer, "player1 answer", 0);
            Keyboard.Listen(Key.B, ButtonState.Pressed, SelectPlayer, "player2 answer", 1);
        }


        /// <summary>
        /// To select a player amongst the two players.
        /// </summary>
        /// <param name="playerIndex"></param>
        private void SelectPlayer(int playerIndex)
        {
            if (playerInTurn == -1) // no player selected
            {
                playerInTurn = playerIndex;
                MessageDisplay.Add("player in turn: " + (playerInTurn + 1));
            }
        }


        /// <summary>
        /// The timer counter for a question. The handles the time in which a question should be answered.
        /// </summary>
        void ReadTimeCounter()
        {

            downCounter = new DoubleMeter(20);
            TimeCounter = new Timer();
            TimeCounter.Interval = 0.1;
            TimeCounter.Timeout += TimeOut;
            TimeCounter.Start();

            Label TimeDisplay = new Label();
            TimeDisplay.TextColor = Color.Red;
            TimeDisplay.DecimalPlaces = 1;
            TimeDisplay.BindTo(downCounter);
            TimeDisplay.X = 0.0;
            TimeDisplay.Y = 200;
            Add(TimeDisplay);
        }


        /// <summary>
        /// Used to display a time out message when a question is not answered in the specified amount of time
        /// </summary>
        private void TimeOut()
        {
            downCounter.Value -= 0.1;
            if (downCounter.Value <= 0)
            {
                MessageDisplay.Add("Time out...");
                TimeCounter.Stop();
                IntMeter currentPlayerPoints = playerPoints[playerInTurn];
                currentPlayerPoints.AddValue(-1);
               
            }
            Console.ReadLine();

        }

        /// <summary>
        /// Update point for each player
        /// </summary>
        void Points()
        {
            playerPoints[0] = PlayerPoints(Screen.Left + 100.0, Screen.Top - 100.0, 0);
            playerPoints[1] = PlayerPoints(Screen.Right - 100.0, Screen.Top - 100.0, 1);

        }


        /// <summary>
        /// Show updated points for each player on the screen. Addtionaly play music when a player wins.
        /// </summary>
        /// <param name="x">The x-coordinate on screen where the score should be displayed </param>
        /// <param name="y">The y-coordinate on screen where the score should be displayed </param>
        /// <returns>it return the points calculator of type <cref="IntMeter"/> for a player</returns>
        IntMeter PlayerPoints(double x, double y, int playerIndex)
        {
            IntMeter calculator = new IntMeter(0);
            calculator.MaxValue = 10;
            calculator.MinValue = 0;

            Label screen = new Label();
            screen.BindTo(calculator);
            screen.X = x;
            screen.Y = y;
            screen.TextColor = Color.Black;
            screen.BorderColor = Level.Background.Color;
            screen.Color = Level.Background.Color;
            Add(screen);

            calculator.UpperLimit += playerIndex == 0 ? PlayerOneWon : PlayerTwoWon;
            calculator.AddTrigger(9000, TriggerDirection.Up, CallSound);
            return calculator;
        }


        /// <summary>
        /// Play sound along with a text message
        /// </summary>
        private void CallSound()
        {
            PlaySound("OVER TEN POINTS");
        }


        /// <summary>
        /// Display message when player one is the loser
        /// </summary>
        private void PlayerOneLoses()
        {
            MessageDisplay.Add("Player 1 lost the game! Sorry try again!");
        }


        /// <summary>
        /// Display message when player one is the winner
        /// </summary>
        private void PlayerOneWon()
        {
            MessageDisplay.Add("Player 1 won the game! Congrulations!");
        }


        /// <summary>
        /// Display message when player 2 is the loser
        /// </summary>
        private void PlayerTwoLoses()
        {
            MessageDisplay.Add("Player 2 lost the game! Sorry try again!");
        }


        /// <summary>
        /// Display message when player two is the winner
        /// </summary>
        private void PlayerTwoWon()
        {
            MessageDisplay.Add("Player 2 won the game! Congrulations!");
        }


        /// <summary>
        /// Generate questions and answers on the UI screen. The question and answer are read from a text file.
        /// </summary>
        /// <param name="lineNumber">index of question in the text files where question are stored</param>
        private void GenerateQuestionAndAnswers(int lineNumber)
        {
            data = lines[lineNumber].Split('|');

            // set the question text 
            question.Text = data[0];
            correctAnswer = data[data.Length - 1];

            for (int i = 0; i < answers.Length; i++)
            {

                Label box = answers[i];
                box.Text = data[i + 1];
            }

            downCounter.Value = downCounter.DefaultValue;
            playerInTurn = -1;   // -1 means not a player 
        }


        /// <summary>
        /// Create UI labels to display on the screen. A label can have text inside it.
        /// </summary>
        public void CreateBoxes()
        {
            // create a question label
            for (int i = 0; i < answers.Length; i++)
            {
                Label box = new Label();
                box.Y = -20 * i;
                Add(box);
                box.Text = "test";
                answers[i] = box;
            }

            question = new Label();
            question.Y = 20 + answers[0].Y;
            Add(question);
        }


        /// <summary>
        /// Display answer menu on the UI screen and handle the events related to player clicking on those answers.
        /// </summary>
        void Menu()
        {
            List<Label> options;
            options = new List<Label>();
            Label option1 = answers[0];
            Label option2 = answers[1];
            Label option3 = answers[2];
            Label option4 = answers[3];

            foreach (Label option in options)
            {
                Add(option);
            }

            Mouse.ListenOn(option1, MouseButton.Left, ButtonState.Pressed, ChooseAnswer, null, option1);
            Mouse.ListenOn(option2, MouseButton.Left, ButtonState.Pressed, ChooseAnswer, null, option2);
            Mouse.ListenOn(option3, MouseButton.Left, ButtonState.Pressed, ChooseAnswer, null, option3);
            Mouse.ListenOn(option4, MouseButton.Left, ButtonState.Pressed, ChooseAnswer, null, option4);

            Mouse.ListenOn(option1, HoverState.Enter, MouseButton.None, ButtonState.Irrelevant, MovingInMenu, null, option1, true);
            Mouse.ListenOn(option1, HoverState.Exit, MouseButton.None, ButtonState.Irrelevant, MovingInMenu, null, option1, false);
            Mouse.ListenOn(option2, HoverState.Enter, MouseButton.None, ButtonState.Irrelevant, MovingInMenu, null, option2, true);
            Mouse.ListenOn(option2, HoverState.Exit, MouseButton.None, ButtonState.Irrelevant, MovingInMenu, null, option2, false);
            Mouse.ListenOn(option3, HoverState.Enter, MouseButton.None, ButtonState.Irrelevant, MovingInMenu, null, option3, true);
            Mouse.ListenOn(option3, HoverState.Exit, MouseButton.None, ButtonState.Irrelevant, MovingInMenu, null, option3, false);
            Mouse.ListenOn(option4, HoverState.Enter, MouseButton.None, ButtonState.Irrelevant, MovingInMenu, null, option4, true);
            Mouse.ListenOn(option4, HoverState.Exit, MouseButton.None, ButtonState.Irrelevant, MovingInMenu, null, option4, false);
        }


        /// <summary>
        /// Highlight an answer from the answer menu when mouse cursor is hovered over it.
        /// </summary>
        /// <param name="option">The answer option in the answer menu</param>
        /// <param name="on">status of mouse cursor over answer option in the answer menu</param>
        void MovingInMenu(Label option, bool on)
        {
            if (on)
            {
                option.TextColor = Color.Red;
            }
            else
            {
                option.TextColor = Color.Black;
            }
        }


        /// <summary>
        /// Handles the user click on an answer option from answers menu. Also declares the loser player incase one player wins.
        /// </summary>
        /// <param name="option">The option on which the click occured</param>
        void ChooseAnswer(Label option)
        {
            if (playerInTurn == -1)
            {
                MessageDisplay.Add("no player is selected");

                return;
            }

            int correctIndex = int.Parse(correctAnswer);
            if (option.Text == data[correctIndex]) // right answer 
            {
                MessageDisplay.Add("Correct!");
                IntMeter currentPlayerPoints = playerPoints[playerInTurn];
                currentPlayerPoints.AddValue(1);

                if (currentPlayerPoints.Value == currentPlayerPoints.MaxValue)
                {
                    //abit hacky logic
                    int otherPlayerIndex = playerInTurn == 0 ? 1 : 0;
                    IntMeter otherPlayerPoints = playerPoints[otherPlayerIndex];
                    int temp = otherPlayerPoints.MinValue;
                    otherPlayerPoints.MinValue = otherPlayerPoints.Value;
                    otherPlayerPoints.LowerLimit += otherPlayerIndex == 0 ? PlayerOneLoses : PlayerTwoLoses;
                    otherPlayerPoints.AddValue(-1);
                    otherPlayerPoints.MinValue = temp;
                }
                //option.TextColor = Color.Green;
            }
            else // wrong answer
            {
                MessageDisplay.Add("Incorrect!");
                IntMeter currentPlayerPoints = playerPoints[playerInTurn];
                currentPlayerPoints.AddValue(-1);

                //option.TextColor = Color.Aqua;
            }
            //calculator.LowerLimit += PlayerLoses;

            int x = RandomGen.NextInt(lines.Length);
            GenerateQuestionAndAnswers(x);
        }


    }


}
