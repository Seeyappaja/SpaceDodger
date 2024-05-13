using Godot;
using System;
using System.Threading.Tasks;

public partial class Meteor : Sprite2D
{
    private float meteorSpeed = 5.0f; // Adjust for faster initial speed
    private float meteorWidth;
    private float meteorX;
    private float meteorY;
    private float meteorRadius;
    private float meteorRradian;
    private float meteorRdegree;
    Random rnd = new Random();
    private Sprite2D playerSprite;
    private AnimatedSprite2D explosionSprite;
    private Label scoreLabel;
    private Label highscoreLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        meteorWidth = GetViewportRect().Size.X / 100;
        this.Position = new Vector2( GetViewportRect().Size.X / 2, GetViewportRect().Size.Y / 2);
        meteorRadius = meteorWidth / 2;
        meteorRradian = (float)(rnd.Next(0,361) * (Math.PI / 180));
        meteorRdegree = (float)(meteorRradian / (Math.PI / 180));
        this.Scale = new Vector2(GetViewportRect().Size.X / 6000, GetViewportRect().Size.X / 6000);

        playerSprite = GetNode<Sprite2D>("/root/Node2D/PlayerSprite");
        scoreLabel = GetNode<Label>("/root/Node2D/Current Score");
        highscoreLabel = GetNode<Label>("/root/Node2D/High Score");
        explosionSprite = GetNode<AnimatedSprite2D>("/root/Node2D/AnimatedSprite2D");
        explosionSprite.Play("explode");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        if (scoreLabel.Text == "GAME OVER")
        {
            this.Position = new Vector2(GetViewportRect().Size.X / 2, GetViewportRect().Size.Y / 2);
            meteorWidth = GetViewportRect().Size.X / 100;
            meteorRadius = meteorWidth / 2;
            meteorRradian = (float)(rnd.Next(0, 361) * (Math.PI / 180));
            meteorRdegree = (float)(meteorRradian / (Math.PI / 180));
            this.Scale = new Vector2(GetViewportRect().Size.X / 6000, GetViewportRect().Size.X / 6000);
            explosionSprite.Scale = new Vector2(GetViewportRect().Size.X / 200, GetViewportRect().Size.X / 200);
        }
        else if (scoreLabel.Text != "GAME OVER")
        {
            explosionSprite.GlobalPosition = new Vector2(GetViewportRect().Size.X * -1, GetViewportRect().Size.Y * -1);
            ChangeMeteorPosition();
            CheckForCollission();
        }
    }

	private async void ChangeMeteorPosition()
	{
        await Task.Delay(3*(int)this.meteorSpeed);
        this.Position = new Vector2(this.Position.X + (float)Math.Cos(this.meteorRradian) * this.meteorSpeed,
            this.Position.Y + (float)Math.Sin(this.meteorRradian) * this.meteorSpeed);
        this.Scale *= 1.03f;

        if ((this.Position.X <= 0 || this.Position.X >= GetViewportRect().Size.X) || (this.Position.Y <= 0 || this.Position.Y >= GetViewportRect().Size.Y))
        {
            this.Position = new Vector2(GetViewportRect().Size.X / 2, GetViewportRect().Size.Y / 2);
            meteorRradian = (float)(rnd.Next(0, 361) * (Math.PI / 180));
            meteorRdegree = (float)(meteorRradian / (Math.PI / 180));
            this.Scale = new Vector2(GetViewportRect().Size.X / 6000, GetViewportRect().Size.X / 6000);
            scoreLabel.Text = scoreLabel.Text.Split(" ")[0] + " " + (scoreLabel.Text.Split(" ")[1].ToInt()+1);
        }
    }

    private void CheckForCollission()
    {
        Vector2 position1 = this.Position;
        Vector2 position2 = playerSprite.Position;

        Vector2 viewportCenter = GetViewportRect().Size / 2;
        Vector2 offset = new Vector2(GetViewportRect().Size.X / 3, 0).Rotated(playerSprite.GlobalRotation + (float)Math.PI / 2);

        if ((Math.Abs(position1.Y - position2.Y) <= GetViewportRect().Size.X / 11 && Math.Abs(position1.X - position2.X) <= GetViewportRect().Size.X / 11))
        {
            GetNode<AudioStreamPlayer2D>("/root/Node2D/Theme").Playing = false;
            AudioStream audio = (AudioStream)ResourceLoader.Load("res://Sound/go.mp3");
            GetNode<AudioStreamPlayer2D>("/root/Node2D/AudioStreamPlayer2D").Stream = audio;
            GetNode<AudioStreamPlayer2D>("/root/Node2D/AudioStreamPlayer2D").Play();
            if(highscoreLabel.Text.Split(" ")[1].ToInt() < scoreLabel.Text.Split(" ")[1].ToInt())
            {
                highscoreLabel.Text = "Highscore: " + scoreLabel.Text.Split(" ")[1];
            }
            explosionSprite.GlobalPosition = position2;
            scoreLabel.Text = "GAME OVER";
        }
    }
}
