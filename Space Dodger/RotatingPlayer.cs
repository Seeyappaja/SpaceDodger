using Godot;
using System;

public partial class RotatingPlayer : Node2D
{
    // Player sprite
    private Sprite2D playerSprite;

    // Rotation properties
    private float rotationSpeed = 2.0f; // Adjust this value as needed
    private float rotationDirection = 1.0f; // Initial rotation direction

    // Distance from center
    private float distanceFromCenter = 100.0f; // Adjust this value as needed
    private Label scoreLabel;

    // Called when the node enters the scene tree for the first time
    public override void _Ready()
    {
        // Get the player sprite
        playerSprite = GetNode<Sprite2D>("PlayerSprite");

        // Set the initial position of the player sprite to the center of the viewport
        Vector2 viewportCenter = GetViewportRect().Size / 2;
        playerSprite.GlobalPosition = viewportCenter;
        // Rotate the player sprite by 90 degrees
        //playerSprite.RotationDegrees = 180;
        scoreLabel = GetNode<Label>("/root/Node2D/Current Score");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
        if (scoreLabel.Text != "GAME OVER")
        {
            playerSprite.Scale = new Vector2(GetViewportRect().Size.X / 400, GetViewportRect().Size.X / 400);
            // Calculate the rotation angle based on time and speed
            //float rotationAngle = rotationSpeed * rotationDirection * (float)delta;
            float rotationAngle = rotationSpeed * rotationDirection * (float)delta;

            // Rotate the player sprite around the center of the node
            playerSprite.Rotate(rotationAngle);
            // Update player position to maintain distance from center
            Vector2 viewportCenter = GetViewportRect().Size / 2;
            Vector2 offset = new Vector2(GetViewportRect().Size.X / 2.25f, 0).Rotated(playerSprite.GlobalRotation + (float)Math.PI / 2);
            playerSprite.GlobalPosition = viewportCenter + offset;

            // Check for input to change rotation direction
            if (Input.IsActionJustReleased("key_action_changeShipRotation") && scoreLabel.Text != "GAME OVER") // Change "ui_accept" to the action name you've assigned for Space key
            {
                rotationDirection *= -1; // Reverse rotation direction
            }
        }
        else if (Input.IsActionJustReleased("key_action_changeShipRotation") && scoreLabel.Text == "GAME OVER")
        {
            rotationDirection *= -1;
            scoreLabel.Text = "Score: 0";
            GetNode<AudioStreamPlayer2D>("/root/Node2D/Theme").Playing = true;
            GetNode<AudioStreamPlayer2D>("/root/Node2D/AudioStreamPlayer2D").Playing = false;
        }
    }
}
