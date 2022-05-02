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



        public static double getZustellungenByMaterial(string material)
        {

            double zustellungen = 0.0;
           
            if (material == "Metal")
                zustellungen= 0.5;

            if (material == "Wood_Plastic")
                zustellungen =1.5;

            if (material == "Styrodur")
                zustellungen = 3.5;

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


        public static int getSchnittgeschwindigkeit(string material)
        {
            //Richtwerte von hier: //https://www.precifast.de/schnittgeschwindigkeit-beim-fraesen-berechnen/

            int schnittgeschwindigkeit = 0;

            if (material == "Stahl")
                schnittgeschwindigkeit = 100;

            if (material == "Alu")
                schnittgeschwindigkeit = 300;

            if (material == "Plastic")
                schnittgeschwindigkeit = 100;

            if (material == "Wood")
                schnittgeschwindigkeit = 1500;


            return schnittgeschwindigkeit;
        }



        public static double getDrehzahl(int schnittgeschwindigkeit,double wkzgDurchmesser)
        {
           // n[U / min] = (vc[m / min] * 1000) / (3.14 * Ød1[mm])
            return (schnittgeschwindigkeit * 1000) / (Math.PI * wkzgDurchmesser);
        }
        /*

         1) Schnittgeschwindigkeit
        //https://www.precifast.de/schnittgeschwindigkeit-beim-fraesen-berechnen/
            Richtwerte:

            Stahl: 40 – 120 m/min
            Aluminium: 100 – 500 m/min
            Kupfer, Messing und Bronze: 100 – 200 m/min
            Kunststoffe: 50 – 150 m/min
            Holz: Bis zu 3000 m/min

        2)Drehzahl

           n [U/min] = (vc [m/min] *1000) / (3.14 * Ød1 [mm])


        */

        public static double getVorschub(int zahnzahl, int drehzahl, int vorschubProZahn)//feedrate
        {
            // Vorschub = Zähnezahl * Drehzahl * Vorschub pro Zahn

            return zahnzahl * drehzahl * vorschubProZahn;

        }
    }
}
