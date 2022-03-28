using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using System;
using System.Collections.Generic;
using System.IO;

public class The_Duel : PhysicsGame
{
    private string[] lines;
    private Label[] answers = new Label[4];
    private Label question = new Label();
    string correctAnswer;
    private string[] data;
    private IntMeter[] playerPoints = new IntMeter[2];
    private DoubleMeter downCounter;
    private Timer TimeCounter;
    private int playerInTurn;


    public override void Begin()
    {
        string questions = "../../../../../questions.txt";
        lines = File.ReadAllLines(questions);
        CreateBoxes();
        int x = RandomGen.NextInt(lines.Length);
        Menu();
        Points();
        StartGame();
        ReadTimeCounter();
        GenerateQuestionAndAnswers(x);




        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.A, ButtonState.Pressed, SelectPlayer, "player1 answer", 0);
        Keyboard.Listen(Key.B, ButtonState.Pressed, SelectPlayer, "player2 answer", 1);
    }

    private void SelectPlayer(int playerIndex)
    {
        if (playerInTurn == -1) // no player selected
        {
            playerInTurn = playerIndex;
            MessageDisplay.Add("player in turn: " + (playerInTurn + 1));
        }
    }

    void ReadTimeCounter()
    {
       
        downCounter = new DoubleMeter(10);
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

    private void TimeOut()
    {
        downCounter.Value -= 0.1;
        if (downCounter.Value <= 0)
        {
            MessageDisplay.Add("Time out...");
            TimeCounter.Stop();
        }
        Console.ReadLine();

    }

    private void StartGame()
    {

    }


    void Points()
    {
        playerPoints[0] = PlayerPoints(Screen.Left + 100.0, Screen.Top - 100.0);
        playerPoints[1] = PlayerPoints(Screen.Right - 100.0, Screen.Top - 100.0);

    }
    IntMeter PlayerPoints(double x, double y)
    {
        IntMeter calculator = new IntMeter(0);
        calculator.MaxValue = 10;
         

        Label screen = new Label();
        screen.BindTo(calculator);
        screen.X = x;
        screen.Y = y;
        screen.TextColor = Color.Black;
        screen.BorderColor = Level.Background.Color;
        screen.Color = Level.Background.Color;
        Add(screen);

       
        IntMeter collectedObjects = new IntMeter(0);
        calculator.MaxValue = 5;
        calculator.UpperLimit += PlayerWon;
        calculator.AddTrigger(9000, TriggerDirection.Up, CallSound);

        IntMeter playerLives = new IntMeter(3);
        calculator.MinValue = 0;
        calculator.LowerLimit += PlayerLoses;
        
        return calculator;
    }

    private void CallSound()
    {
        PlaySound("OVER TEN POINTS");
       
    }

    private void PlayerLoses()
    {
        MessageDisplay.Add("Player 1 lost the game.");
    }

    private void PlayerWon()
    {
        MessageDisplay.Add("Player 1 won the game.");
    }

    private void GenerateQuestionAndAnswers(int lineNumber)
    {
        
        data = lines[lineNumber].Split('|');
        // set the question text 


        question.Text = data[0];
        correctAnswer = data[data.Length -1];

        for (int i = 0; i < answers.Length; i++)
        {
            
            Label box = answers[i];
            box.Text = data[i + 1];
        }

        downCounter.Value = downCounter.DefaultValue;
        playerInTurn = -1;   // -1 means not a player 
    }


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

            //option.TextColor = Color.Green;
        }
        else // wrong answer
        {
            MessageDisplay.Add("Incorrect!");
            IntMeter currentPlayerPoints = playerPoints[playerInTurn];
            currentPlayerPoints.AddValue(-1);

            //option.TextColor = Color.Aqua;
        }

        int x = RandomGen.NextInt(lines.Length);
        GenerateQuestionAndAnswers(x);

    }

}

