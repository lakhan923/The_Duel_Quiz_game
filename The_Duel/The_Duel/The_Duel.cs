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
<<<<<<< HEAD
=======
  
  
>>>>>>> 49a8ad761f5ff2eb70679c8d618023b0407ea57b

    public override void Begin()
    {
        string questions = "../../../../../questions.txt";
        lines = File.ReadAllLines(questions);
        CreateBoxes();
        int x = RandomGen.NextInt(lines.Length);
        GenerateQuestionAndAnswers(x);
        Menu();
<<<<<<< HEAD

=======
      
       



        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
>>>>>>> 49a8ad761f5ff2eb70679c8d618023b0407ea57b
    }
   
    private void GenerateQuestionAndAnswers(int lineNumber)
    {
<<<<<<< HEAD

        string[] data = lines[lineNumber].Split('|');
=======
        
        data = lines[lineNumber].Split('|');
>>>>>>> 49a8ad761f5ff2eb70679c8d618023b0407ea57b
        // set the question text 
        question.Text = data[0];
<<<<<<< HEAD
        correctAnswer = data[data.Length - 1];
=======
        correctAnswer = data[data.Length -1];
>>>>>>> 49a8ad761f5ff2eb70679c8d618023b0407ea57b

        for (int i = 0; i < answers.Length; i++)
        {
            Label box = answers[i];
            box.Text = data[i + 1];
        }

    }

<<<<<<< HEAD
    public void CreateBoxes()
=======

    public void CreateBoxes() 
>>>>>>> 49a8ad761f5ff2eb70679c8d618023b0407ea57b
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

        Mouse.ListenOn(option1, MouseButton.Left, ButtonState.Pressed, ChooseAnswer, null, option1, correctAnswer);
        Mouse.ListenOn(option2, MouseButton.Left, ButtonState.Pressed, ChooseAnswer, null, option2, correctAnswer);
        Mouse.ListenOn(option3, MouseButton.Left, ButtonState.Pressed, ChooseAnswer, null, option3, correctAnswer);
        Mouse.ListenOn(option4, MouseButton.Left, ButtonState.Pressed, ChooseAnswer, null, option4, correctAnswer);

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

    void ChooseAnswer(Label option, string correctAnswer)
    {
<<<<<<< HEAD
        if (option.Text == correctAnswer)
=======
        int correctIndex = int.Parse(correctAnswer);
        if (option.Text == data [correctIndex])
>>>>>>> 49a8ad761f5ff2eb70679c8d618023b0407ea57b
            option.TextColor = Color.Yellow;
    }


}

