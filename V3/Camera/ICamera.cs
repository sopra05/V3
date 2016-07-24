using Microsoft.Xna.Framework;
using V3.Objects;

namespace V3.Camera
{
    public interface ICamera
    {
        Matrix Transform { get; set; }
        Vector2 Location { get; set; }
        Point ScreenSize { get; }
        Rectangle ScreenRectangle { get; }

        void Update(ICreature player);

        int MapPixelWidth { get; set; }
        int MapPixelHeight { get; set; }
    }
    }
