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
    private Label[] questions = new Label[4];
    private Label question = new Label();

    public override void Begin()
    {
        string questions = "../../../../../questions.txt";
        lines = File.ReadAllLines(questions);

        CreateBoxes();
        
        int x = RandomGen.NextInt(lines.Length);
        GenerateQuestionAndAnswers(x);

        

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    private void GenerateQuestionAndAnswers(int lineNumber)
    {

        string[] data = lines[lineNumber].Split('|');
        // set the question text 
        for (int i = 0; i < questions.Length; i++)
        {
           Label box = questions[i];
           box.Text = data[i + 1];
            
        }
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
          questions[i] = box;
            
        }
        
    }
}

