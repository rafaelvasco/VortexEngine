using VortexCore;

namespace VortexDemo
{
    public class Gui : Group
    {
        public Gui()
        {
            var background = new Sprite(Assets.LoadTexture("seiken.png"));
            var foregroundObject = new Sprite(Assets.LoadTexture("party.png"));

            this.Add(background);
            this.Add(foregroundObject);

            foregroundObject.X = this.Width / 2 - foregroundObject.Width / 2;
            foregroundObject.Y = this.Height / 2 - foregroundObject.Height / 2;

        }

    }
}
