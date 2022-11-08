using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;

namespace CNC
{
    class GCodeButton
    {
        public class CustomAttributes : GH_ComponentAttributes
        {
            public CustomAttributes(CreateGCode owner) : base(owner) { }

            private System.Drawing.Rectangle ButtonBounds { get; set; }

            #region Layout and render
            protected override void Layout()
            {
                base.Layout();

                System.Drawing.Rectangle rec0 = GH_Convert.ToRectangle(Bounds);
                rec0.Height += 22;

                System.Drawing.Rectangle rec1 = rec0;
                rec1.Y = rec1.Bottom - 22;
                rec1.Height = 22;
                rec1.Inflate(-2, -2);

                Bounds = rec0;
                ButtonBounds = rec1;
            }

            protected override void Render(GH_Canvas canvas, System.Drawing.Graphics graphics, GH_CanvasChannel channel)
            {
                base.Render(canvas, graphics, channel);
                CreateGCode comp = Owner as CreateGCode;
                if (channel == GH_CanvasChannel.Objects)
                {
                    GH_Capsule button = GH_Capsule.CreateTextCapsule(ButtonBounds, ButtonBounds, comp.Run == true ? GH_Palette.Grey : GH_Palette.Transparent, "Write", 2, 0);
                    button.Render(graphics, Selected, Owner.Locked, false);
                    button.Dispose();
                }
            }
            #endregion

            #region ClickHandling
            public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    CreateGCode comp = Owner as CreateGCode;
                    System.Drawing.RectangleF rec = ButtonBounds;
                    if (rec.Contains(e.CanvasLocation))
                    {
                        comp.Run = true;
                        comp.ExpireSolution(true);
                        return GH_ObjectResponse.Handled;
                    }
                }
                return base.RespondToMouseDown(sender, e);
            }

            public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    CreateGCode comp = Owner as CreateGCode;
                    System.Drawing.RectangleF rec = ButtonBounds;
                    if (rec.Contains(e.CanvasLocation))
                    {
                        comp.Run = false;
                        comp.ExpireSolution(true);
                        return GH_ObjectResponse.Handled;
                    }
                }
                return base.RespondToMouseDown(sender, e);
            }
            #endregion

        }
    }
}