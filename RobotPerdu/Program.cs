using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace RobotPerdu
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    interface Materiel
    {
        void seDecrire();
    }


    class Table : Materiel
    {
        public int Hauteur { get; set; }
        public int NbPieds { get; set; }

        public Table(int h, int pieds)
        {
            this.Hauteur = h;
            this.NbPieds = pieds;
        }
        void Materiel.seDecrire()
        {
            Console.WriteLine("Bonjour, je suis une table de " + Hauteur + "cm de hauteur, et j'ai " + NbPieds + " pieds.");
        }
    }

    class Chaise : Materiel
    {
        public int Couleur { get; set; }
        public int NbPieds { get; set; }
        public Chaise(int coul, int pieds)
        {
            this.Couleur = coul;
            this.NbPieds = pieds;
        }
        void Materiel.seDecrire()
        {
            Console.WriteLine("Bonjour, je suis une chaise " + Couleur + " à " + NbPieds + " pieds.");
        }
    }

    class Bouteille : Materiel
    {
        public int Volume { get; set; }

        public Bouteille(int vol)
        {
            this.Volume = vol;
        }
        void Materiel.seDecrire()
        {
            Console.WriteLine("Bonjour, je suis une bouteille de " + Volume + "L.");
        }
    }

    class Sac
    {
        public ArrayList monRobateriel { get; set; }
        public Sac(ArrayList matos)
        {
            this.monRobateriel = matos;
        }
        public void seDecrire()
        {
            Console.WriteLine("Je suis un gros sac, et dans mon ventre, il y a des objets : ");
            foreach(Materiel m in monRobateriel)
            {
                Console.WriteLine("L'objet dit : \" ");
                m.seDecrire();
                Console.WriteLine("\"");
            }
        }
    }

    enum Direction
    {
        N,
        S,
        E,
        O
    }

    class Issue
    {
        public Salle salleDansLaquelleOnEntre { get; set; }

        
        public int etat { get; set; }

        public Issue(Salle sale, int state)
        {
            this.salleDansLaquelleOnEntre = sale;
            this.etat = state;
        }
        public void seDecrire()
        {
            Console.WriteLine("Je suis une issue vers la salle "+ salleDansLaquelleOnEntre.nom);
        }
    }

    class Salle
    {
        public string nom { get; set; }
        public ArrayList monMatos { get; set; }
        public Dictionary<Direction, Issue> sorties { get; set; }

        public Salle(string nom, ArrayList matos, Dictionary<Direction,Issue> sorties)
        {
            this.nom = nom;
            this.monMatos = matos;
            this.sorties = sorties;
        }
        void decrireIssues()
        {
            foreach (KeyValuePair<Direction, Issue> entry in sorties)
            {
                Console.WriteLine(entry.Key + " : " + entry.Value.salleDansLaquelleOnEntre.nom);
            }
        }

        void decrireMateriel()
        {
            foreach(Materiel m in monMatos)
            {
                m.seDecrire();
            }
        }

        void mettreMaterielDans(Sac sac)
        {
            sac.monRobateriel.AddRange(monMatos);
        }

        void seDecrire()
        {
            Console.WriteLine("Je suis la salle " + nom + "et je contiens " + monMatos.Capacity + " objets.");
        }

        void viderMateriel()
        {
            monMatos.Clear();
        }
    }

}
