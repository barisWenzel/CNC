using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNC
{
    public static class UTIL
    {
        //https://wiki.fablab-muenchen.de/pages/viewpage.action?pageId=3900165
        public static Curve getCorrectOffset(Curve crv, double dis)
        {
            Curve curve;
            List<Curve> crvs = new List<Curve>();
            crvs.AddRange(crv.Offset(Plane.WorldXY, dis, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance, CurveOffsetCornerStyle.Sharp));
            crvs.AddRange(crv.Offset(Plane.WorldXY, -dis, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance, CurveOffsetCornerStyle.Sharp));


            if (dis < 0)
                curve = crvs.OrderBy(item => item.GetLength()).ToList()[0];

            else
                curve = crvs.OrderBy(item => item.GetLength()).ToList()[1];

            return curve;
        }



        public static double getZustellungenByMaterial(string material, double durchmesser)
        {
           //https://www.cnc-wiki.de/Betrieb/Fraesparameter

            double zustellungen = 1;
           
            if (material == "Metal")
                zustellungen= 0.5* durchmesser;

            if (material == "Wood")
                zustellungen =0.5 * durchmesser;

            if(material == "Plastic")
                zustellungen = 1.5* durchmesser;


            if (material == "Styrodur")
                zustellungen = 3.5* durchmesser;
            

            return zustellungen;    

        }

        public static List<double> getRange(double max,int steps)
        {
            var dbls = new List<double>();
            double step = max / steps;
            for (int i = 0; i < steps + 1; i++)
            {
                dbls.Add(step * i);
            }
            return dbls;
        }


        public static int[] getSchnittgeschwindigkeit(string material)
        {

            /*
            Richtwerte von hier: //https://www.precifast.de/schnittgeschwindigkeit-beim-fraesen-berechnen/
         
            Stahl: 40 – 120 m / min
            Aluminium: 100 – 500 m / min
            Kupfer, Messing und Bronze: 100 – 200 m / min
            Kunststoffe: 50 – 150 m / min
            Holz: Bis zu 3000 m / min
            */

            int[] schnittgeschwindigkeit = new int[2];

            if (material == "Stahl")
            { 
                schnittgeschwindigkeit[0] = 40;
                schnittgeschwindigkeit[1] = 120;
            }

            if (material == "Aluminium")
            {
                schnittgeschwindigkeit[0] = 100;
                schnittgeschwindigkeit[1] = 500;
            }

            if (material == "Plastic")
            {
                schnittgeschwindigkeit[0] = 50;
                schnittgeschwindigkeit[1] = 150;
            }

            if (material == "Wood")
            {
                schnittgeschwindigkeit[0] = 400;
                schnittgeschwindigkeit[1] = 3000;
            }


            return schnittgeschwindigkeit;
        }



        public static int getDrehzahl(int schnittgeschwindigkeit,double wkzgDurchmesser)
        {
            //n [U/min] = (vc [m/min] *1000) / (3.14 * Ød1 [mm
            return (int)(schnittgeschwindigkeit*1000 / (Math.PI * wkzgDurchmesser));
        }
        

        public static double getVorschub(int zahnzahl, int drehzahl, double vorschubProZahn)//feedrate
        {
            // Vorschub(mm/min) = Zähnezahl * Drehzahl * Vorschub pro Zahn
            return zahnzahl * drehzahl * vorschubProZahn;
        }


    }
}
