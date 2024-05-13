using Godot;
using System;
using System.Threading.Tasks;

public partial class WarpBubble : Sprite2D
{
    private float bubbleSpeed = 5.0f; // Adjust for faster initial speed
    private float bubbleWidth;
    private float bubbleX;
    private float bubbleY;
    private float bubbleRadius;
    private float bubbleRradian;
    private float bubbleRdegree;
    Random rnd = new Random();
    private Sprite2D playerSprite;
    private Label scoreLabel;
    private Label highscoreLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bubbleWidth = GetViewportRect().Size.X / 100;
        this.Position = new Vector2(GetViewportRect().Size.X / 2, GetViewportRect().Size.Y / 2);
        bubbleRadius = bubbleWidth / 2;
        bubbleRradian = (float)(rnd.Next(0, 361) * (Math.PI / 180));
        bubbleRdegree = (float)(bubbleRradian / (Math.PI / 180));
        this.Scale = new Vector2(GetViewportRect().Size.X / 6000, GetViewportRect().Size.X / 6000);

        playerSprite = GetNode<Sprite2D>("/root/Node2D/PlayerSprite");
        scoreLabel = GetNode<Label>("/root/Node2D/Current Score");
        highscoreLabel = GetNode<Label>("/root/Node2D/High Score");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (scoreLabel.Text == "GAME OVER")
        {
            this.Position = new Vector2(GetViewportRect().Size.X / 2, GetViewportRect().Size.Y / 2);
            bubbleWidth = GetViewportRect().Size.X / 100;
            bubbleRadius = bubbleWidth / 2;
            bubbleRradian = (float)(rnd.Next(0, 361) * (Math.PI / 180));
            bubbleRdegree = (float)(bubbleRradian / (Math.PI / 180));
            this.Scale = new Vector2(GetViewportRect().Size.X / 2500, GetViewportRect().Size.X / 2500);
        }
        else if (scoreLabel.Text != "GAME OVER")
        {
            ChangeMeteorPosition();
            CheckForCollission();
        }
    }

    private async void ChangeMeteorPosition()
    {
        await Task.Delay(3 * (int)this.bubbleSpeed);
        this.Position = new Vector2(this.Position.X + (float)Math.Cos(this.bubbleRradian) * this.bubbleSpeed,
            this.Position.Y + (float)Math.Sin(this.bubbleRradian) * this.bubbleSpeed);
        this.Scale *= 1.03f;

        if ((this.Position.X <= 0 || this.Position.X >= GetViewportRect().Size.X) || (this.Position.Y <= 0 || this.Position.Y >= GetViewportRect().Size.Y))
        {
            this.Position = new Vector2(GetViewportRect().Size.X / 2, GetViewportRect().Size.Y / 2);
            bubbleRradian = (float)(rnd.Next(0, 361) * (Math.PI / 180));
            bubbleRdegree = (float)(bubbleRradian / (Math.PI / 180));
            this.Scale = new Vector2(GetViewportRect().Size.X / 2500, GetViewportRect().Size.X / 2500);
        }
    }

    private void CheckForCollission()
    {
        Vector2 position1 = this.Position;
        Vector2 position2 = playerSprite.Position;

        Vector2 viewportCenter = GetViewportRect().Size / 2;
        Vector2 offset = new Vector2(GetViewportRect().Size.X / 3, 0).Rotated(playerSprite.GlobalRotation + (float)Math.PI / 2);

        if ((Math.Abs(position1.Y - position2.Y) <= GetViewportRect().Size.X / 10 && Math.Abs(position1.X - position2.X) <= GetViewportRect().Size.X / 10))
        {
            AudioStream audio = (AudioStream)ResourceLoader.Load("res://Sound/warp.wav");
            GetNode<AudioStreamPlayer2D>("/root/Node2D/AudioStreamPlayer2D").Stream = audio;
            GetNode<AudioStreamPlayer2D>("/root/Node2D/AudioStreamPlayer2D").Play();
            scoreLabel.Text = scoreLabel.Text.Split(" ")[0] + " " + (scoreLabel.Text.Split(" ")[1].ToInt() + 3);

            this.Position = new Vector2(GetViewportRect().Size.X / 2, GetViewportRect().Size.Y / 2);
            bubbleRradian = (float)(rnd.Next(0, 361) * (Math.PI / 180));
            bubbleRdegree = (float)(bubbleRradian / (Math.PI / 180));
            this.Scale = new Vector2(GetViewportRect().Size.X / 2500, GetViewportRect().Size.X / 2500);
        }
    }
}
