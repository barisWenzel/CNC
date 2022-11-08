using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace CNC
{
    public class CreateGCode : GH_Component
    {

        public CreateGCode() : base("GCode", "GCode", "Generate the GCode", "CNC", "CNC") { }



        #region Button

        public override void CreateAttributes()
        {
            m_attributes = new GCodeButton.CustomAttributes(this);
        }

        public bool Run;

        #endregion

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("pts", "p", "pts for the GCode", GH_ParamAccess.tree);
            pManager.AddTextParameter("dir", "dir", "dir", GH_ParamAccess.item);
            pManager.AddTextParameter("name", "name", "name", GH_ParamAccess.item);
            pManager.AddNumberParameter("clearance", "clear", "clearance", GH_ParamAccess.item, 20);
            pManager.AddNumberParameter("spindlespeed", "sspeed", "spindlespeed", GH_ParamAccess.item, 6000);//this will be calculated
            pManager.AddNumberParameter("Feedrate", "Feedrate", "Feedrate", GH_ParamAccess.item);


        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {

        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {



            var pts = new GH_Structure<GH_Point>();
            if ((!DA.GetDataTree(0, out pts))) return;

            string path = string.Empty;
            if (!DA.GetData(1, ref path)) return;

            string name = string.Empty;
            if (!DA.GetData(2, ref name)) return;

            double clearance = 0.0;
            if (!DA.GetData(3, ref clearance)) return;

            double spindleSpeed = 0.0;
            if (!DA.GetData(4, ref spindleSpeed)) return;

            double Feedrate = 0.0;
            if (!DA.GetData(5, ref Feedrate)) return;

            #region  G-Code Befehle (DIN 66025 / ISO 6983 / Sinumerik)
            string setToMM = "G21";
            string absPos = "G90";
            string clear = "G1 Z" + clearance.ToString();
            string spindlespeed = "S " + spindleSpeed.ToString();
            string startSpindle = "M03 ";
            string startSpindleAtSpeed = startSpindle + spindlespeed;
            string stopSpindle = "M05";
            string endProgram = "M30";
            string feedrate = "F" + Feedrate;
            string eilgang = "G00";


            #endregion


            List<string> movement = new List<string>();
            movement.Add(setToMM);
            movement.Add(absPos);
            movement.Add(clear);
            movement.Add(startSpindleAtSpeed);
            movement.Add(feedrate);


            for (int i = 0; i < pts.Branches.Count; i++)
            {
                
                movement.Add(eilgang); // ohne Bohrer probieren!
              
                GH_Point ptOld = pts.Branches[i][0];
                Rhino.Geometry.Point3d ptNew = new Point3d(ptOld.Value.X, ptOld.Value.Y, clearance);
                pts.Branches[i].Insert(0, (new GH_Point(ptNew)));


                for (int j = 0; j < pts.Branches[i].Count; j++)
                {
                    GH_Point pt = pts.Branches[i][j];
                    string ptToNc = String.Format("{0} {1}{2:0.0000} {3}{4:0.0000} {5:0.0000}{6:0.0000}", "G1", "X", pt.Value.X, "Y", pt.Value.Y, "Z", pt.Value.Z);
                    movement.Add(ptToNc);
                }
                movement.Add(clear);
                movement.Add(feedrate);
            }
            movement.Add(clear);
            movement.Add(stopSpindle);
            movement.Add(endProgram);
            // an Ursprung fahreny

            if (Run)
            {
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                string fileName = string.Format("{0}\\{1}" + ".nc", path, name);

                System.IO.File.WriteAllLines(fileName, movement);
                //System.Windows.Forms.MessageBox.Show("The file was successfully written to " + path, "Export result", System.Windows.Forms.MessageBoxButtons.OK);
            }

            this.Message = "Create G-Code";
        }
        
        public override Guid ComponentGuid => new Guid("5F356CC5-0AD8-4D6F-8CD9-4EAF671D469C");
    }
}