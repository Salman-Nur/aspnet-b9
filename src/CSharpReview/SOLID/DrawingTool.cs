using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID
{
    public enum ShapeTypes
    {
        Rectangle,
        Circle,
        Triangle
    }
    public class DrawingTool
    {
        public void DrawShape(ShapeTypes type, string color)
        {
            if(type == ShapeTypes.Rectangle)
            {
                // code to draw shape
            }
            else if(type == ShapeTypes.Triangle)
            {
                // code to draw shape
            }
            else
            {
                // code to draw shape
            }
        }
    }
}
