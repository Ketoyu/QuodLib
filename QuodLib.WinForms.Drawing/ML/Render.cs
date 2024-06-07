using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QuodLib.ML.Foundation.Network;

namespace QuodLib.WinForms.Drawing.ML {
    public static class Render {
        

        public static void Network(Graphics g, Func<double, Pen> pen, Pen penInactive, Func<double, Brush> brushActive, IEnumerable<Connection> connections, int maxLayers, int maxNodes, Rectangle bounds) {
            float nodeDiameter = System.Math.Min(
                bounds.Width / (maxLayers + 1), 
                bounds.Height / (maxNodes + 1)
            ) / 6;

            float layerSpacing = bounds.Width / (maxLayers + 1);
            float nodeSpacing = bounds.Height / (maxNodes + 1);

            foreach (Connection c in connections) {
                float x1 = bounds.X + nodeDiameter + (layerSpacing * c.LayerIn),
                    y1 = bounds.Y + nodeDiameter + (nodeSpacing * c.IndexIn);

                //1. draw Weight
                if (c.Weight != null && c.IndexNode != null)
                    g.DrawLine(pen((double)c.Weight), 
                        x1, y1, 
                        bounds.X + nodeDiameter + (layerSpacing * (c.LayerIn + 1)), bounds.Y + nodeDiameter + (nodeSpacing * (c.IndexIn + 1))
                    );

                //2. draw { LayerIn, IndexIn }
                if (c.Activation == null)
                    g.DrawCircle(penInactive, x1, y1, nodeDiameter / 2F);
                else
                    g.FillCircle(brushActive((double)c.Activation), x1, y1, nodeDiameter / 2F);
            }
        }

    }
}
