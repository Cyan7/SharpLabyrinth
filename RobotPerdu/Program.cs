using System;
using System.Collections.Generic;

namespace RobotPerdu
{
    class Program
    {
        static Dictionary<char, Direction> directionCharMap = new Dictionary<char, Direction> {{'N', Direction.N},{'S', Direction.S},{'O', Direction.O},{'E', Direction.E}};

        static void Main(string[] args)
        {

            Labyrinthe labyrinthe = new Labyrinthe();
            Console.WriteLine("Taper ? pour la liste des commandes.");

            while(true)
            {
                Console.Write($"{labyrinthe.Robot.Salle.nom} $ ");
                string line = Console.ReadLine();

                switch(line[0])
                {
                case '?':
                    Console.WriteLine(
@"b   - voir sac
r   - voir salle
i   - voir materiel salle
t   - prendre materiel
g   - voir issues
gX  - prendre issue N/S/E/O
m   - decrire robot
l   - decrire labyrinthe
q   - quitter"      );
                    break;
                case 'b':
                    labyrinthe.Robot.Sac.seDecrire();
                    break;
                case 'r':
                    labyrinthe.Robot.Salle.seDecrire();
                    break;
                case 'i':
                    labyrinthe.Robot.Salle.decrireMateriel();
                    break;
                case 't':
                    labyrinthe.Robot.prendreTout();
                    break;
                case 'g':
                    if(line.Length>1 && directionCharMap.ContainsKey(line[1])) {
                        labyrinthe.Robot.allerVers(directionCharMap[line[1]]);
                    }
                    else labyrinthe.Robot.Salle.decrireIssues();
                    break;
                case 'm':
                    labyrinthe.Robot.seDecrire();
                    break;
                case 'l':
                    labyrinthe.seDecrire();
                    break;
                case 'q':
                    return;
                default:
                    Console.Error.WriteLine("Commande inconnue.");
                    break;
                }
            }
        }
    }

    class Labyrinthe
    {
        public Robot Robot { get; }
        public List<Salle> Salles { get; }
        public Labyrinthe()
        {
            Salles = new List<Salle>();
            Salles.Add(new Salle("salle 1"));
            Salles.Add(new Salle("salle 2"));
            Salles.Add(new Salle("salle 3"));
            Salles.Add(new Salle("salle 4"));

            Salles[0].sorties.Add(Direction.E, new Issue(Salles[1], 1));
            Salles[0].sorties.Add(Direction.S, new Issue(Salles[2], 1));
            Salles[1].sorties.Add(Direction.O, new Issue(Salles[0], 1));
            Salles[1].sorties.Add(Direction.S, new Issue(Salles[3], 1));
            Salles[2].sorties.Add(Direction.N, new Issue(Salles[0], 1));
            Salles[2].sorties.Add(Direction.E, new Issue(Salles[3], 1));
            Salles[3].sorties.Add(Direction.N, new Issue(Salles[1], 1));
            Salles[3].sorties.Add(Direction.O, new Issue(Salles[2], 1));

            Salles[0].monMatos.Add(new Table(60, 3));
            Salles[1].monMatos.Add(new Chaise("verte", 8));
            Salles[3].monMatos.Add(new Bouteille(2));

            this.Robot = new Robot(Salles[0]);
        }
        public void seDecrire()
        {
            Console.WriteLine($"Je suis un labyrinthe contenant {Salles.Count} salles.");
        }
    }

    class Robot {
        public Salle Salle { get; private set; }
        public Sac Sac { get; }
        public Robot(Salle position) {
            this.Salle = position;
            this.Sac = new Sac();
        }
        public void allerVers(Direction dir)
        {
            if(Salle.sorties.ContainsKey(dir) && Salle.sorties[dir].etat != 0)
            {
                this.Salle = Salle.sorties[dir].salleDansLaquelleOnEntre;
            }
        }

        public void prendreTout()
        {
            Salle.mettreMaterielDans(Sac);
        }

        public void decrireMateriel()
        {
            Sac.seDecrire();
        }

        public void seDecrire()
        {
            Console.WriteLine($"Je suis un robot dans {Salle.nom}, mon sac contient {Sac.monRobateriel.Count} objets.");
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
        public string Couleur { get; set; }
        public int NbPieds { get; set; }
        public Chaise(string coul, int pieds)
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
        public List<Materiel> monRobateriel { get; }

        public Sac()
        {
            monRobateriel = new List<Materiel>();
        }

        public void seDecrire()
        {
            Console.WriteLine($"Je suis un gros sac, et dans mon ventre, il y a {monRobateriel.Count} objets.");
            for(int i=0; i<monRobateriel.Count; i++)
            {
                Console.Write($"L'objet {i+1} dit : ");
                monRobateriel[i].seDecrire();
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
            Console.WriteLine($"{salleDansLaquelleOnEntre.nom} ({etat})");
        }
    }

    class Salle
    {
        public string nom { get; set; }
        public List<Materiel> monMatos { get; }
        public Dictionary<Direction, Issue> sorties { get; }

        public Salle(string nom)
        {
            this.nom = nom;
            this.monMatos = new List<Materiel>();
            this.sorties = new Dictionary<Direction, Issue>();
        }
        public void decrireIssues()
        {
            foreach (KeyValuePair<Direction, Issue> entry in sorties)
            {
                Console.Write(entry.Key + " : ");
                entry.Value.seDecrire();
            }
        }

        public void decrireMateriel()
        {
            for(int i=0; i<monMatos.Count; i++)
            {
                Console.Write($"L'objet {i+1} dit : ");
                monMatos[i].seDecrire();
            }
        }

        public void mettreMaterielDans(Sac sac)
        {
            sac.monRobateriel.AddRange(monMatos);
            viderMateriel();
        }

        public void seDecrire()
        {
            Console.WriteLine($"Je suis la salle {nom} et je contiens {monMatos.Count} objets.");
        }

        public void viderMateriel()
        {
            monMatos.Clear();
        }
    }

}
