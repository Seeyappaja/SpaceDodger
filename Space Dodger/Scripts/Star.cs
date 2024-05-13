using Godot;
using System;
using System.Threading.Tasks;

public partial class Star : Sprite2D
{
    private float starSpeed = 15.0f; // Adjust for faster initial speed
    private float starWidth;
    private float starX;
    private float starY;
    private float starRadius;
    private float starRradian;
    private float starRdegree;
    Random rnd = new Random();
    private Sprite2D playerSprite;

    // Called when the node enters the scene tree for the first time.
    public async override void _Ready()
    {
        await Task.Delay(rnd.Next(0,1500));
        starWidth = GetViewportRect().Size.X / 100;
        this.Position = new Vector2(GetViewportRect().Size.X / 2, GetViewportRect().Size.Y / 2);
        starRadius = starWidth / 2;
        starRradian = (float)(rnd.Next(0, 361) * (Math.PI / 180));
        starRdegree = (float)(starRradian / (Math.PI / 180));
        this.Scale = new Vector2(GetViewportRect().Size.X / 1000, GetViewportRect().Size.X / 1000);

        playerSprite = GetNode<Sprite2D>("/root/Node2D/PlayerSprite");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        ChangeStarPosition();
    }

    private async void ChangeStarPosition()
    {
        await Task.Delay(3 * (int)this.starSpeed);

        if (playerSprite != null)
        {
            float deltaX = (float)Math.Cos(this.starRradian + playerSprite.GlobalRotation) * starSpeed;
            float deltaY = (float)Math.Sin(this.starRradian + playerSprite.GlobalRotation) * starSpeed;
            this.Position = new Vector2(this.Position.X + deltaX, this.Position.Y + deltaY);
        }
        else
        {
            this.Position = new Vector2(this.Position.X + (float)Math.Cos(this.starRradian) * this.starSpeed,
            this.Position.Y + (float)Math.Sin(this.starRradian) * this.starSpeed);
        }


        this.Scale *= 1.04f;

        if ((this.Position.X <= 0 || this.Position.X >= GetViewportRect().Size.X) || (this.Position.Y <= 0 || this.Position.Y >= GetViewportRect().Size.Y))
        {
            this.Position = new Vector2(GetViewportRect().Size.X / 2, GetViewportRect().Size.Y / 2);
            starRradian = (float)(rnd.Next(0, 361) * (Math.PI / 180));
            starRdegree = (float)(starRradian / (Math.PI / 180));
            this.Scale = new Vector2(GetViewportRect().Size.X / 1000, GetViewportRect().Size.X / 1000);
        }
    }
}

