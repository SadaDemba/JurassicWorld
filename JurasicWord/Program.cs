Console.WriteLine("Bienvenue dans le jeu");

//-----------------DECLARATION DE VARIABLES-------------------------------

// Nombre de ligne et de colonnes de notre plateau
int nbLignes, nbColonnes;
bool partieTerminee;
string raisonDeLaFin;

// Position de chacun des personnages stockée dans des tableaux [numLigne,numColonne]
int[] owenPos, indominusPos, bluePos, maisiePos;

//Le nombre de grenade que Owen peut lancer
int nbGrenades;

// Plateau qui est un tableau de caractères à 2D
char[,] plateau;

//Caractères permettant d'identifier le contnu de chaque case le plateau
const char VIDE = ' ';
const char OWEN = 'O';
const char BLUE = 'B';
const char MAISIE = 'M';
const char INDOMINUS = 'I';
const char TROU = 'X';

//Utilisé pour la génération aléatoire
Random generateur = new();

//------------------FIN DECLARATION--------------------------

RenseignerDimensionsPlateau();

//Affecter le ombre de grenades à la plus grande dimension
nbGrenades = Math.Max(nbLignes, nbColonnes);

// Création du plateau
plateau = new char[nbLignes, nbColonnes];

//Initialiser notre plateau avec des cases vides
for (int i = 0; i < nbLignes; i++)
{
    for (int j = 0; j < nbColonnes; j++)
    {
        plateau[i, j] = VIDE;
    }
}


// Affectation des positions de départ des utilisateurs
owenPos = [0, 0];
bluePos = [0, 1];
maisiePos = [nbLignes - 1, nbColonnes - 1];
indominusPos = [nbLignes / 2, nbColonnes / 2];

partieTerminee = false;
raisonDeLaFin = "";

while (true) // Ajouter une boucle pour continuer à jouer
{
    PlacerPersonnages();
    AfficherPlateau(plateau);

    //Tour de Owen
    DeplacerManuellement(OWEN, owenPos);

    AfficherPlateau(plateau);

    //Tour de Blue
    DeplacerManuellement(BLUE, bluePos);

    //Tour de Maisie
    DeplacerAleatoirement(MAISIE, maisiePos);

    //Tour de Indominus Rex
    DeplacerAleatoirement(INDOMINUS, indominusPos);

    if (partieTerminee)
    {
        Console.WriteLine($"{raisonDeLaFin}  \n ===== Tu as perdu =====");
        break;
    }



}

//------------------Fonctions---------------------------------------

// Fonction permettant d'avoir le nombre de lignes et de colonnes de notre plateau
void RenseignerDimensionsPlateau()
{

    // Saisie et validation du nombre de lignes
    do
    {
        Console.WriteLine("Entrez le nombre de lignes (doit être un nombre positif) :");
        var input = Console.ReadLine();


        // Valider que l'entrée est un nombre positif
        if (int.TryParse(input, out nbLignes) && nbLignes > 0)
        {
            break;
        }
        else
        {
            Console.WriteLine("Veuillez entrer un nombre valide et positif.");
        }
    } while (true);


    // Saisie et validation du nombre de colonnes
    do
    {
        Console.WriteLine("Entrez le nombre de colonnes (doit être un nombre positif) :");
        var input = Console.ReadLine();


        // Valider que l'entrée est un nombre positif
        if (int.TryParse(input, out nbColonnes) && nbColonnes > 0)
        {
            break;
        }
        else
        {
            Console.WriteLine("Veuillez entrer un nombre valide et positif.");
        }
    } while (true);


}

int [] RenseignerCible()
{
    Console.WriteLine("Choix de la cible par Owen avec portée de 3 cases :");
    int[] cible = [];

    // Saisie et validation du numéro de ligne de la cible
    do
    {
        Console.WriteLine("Entrez le numéro de ligne de la cible (doit être un nombre positif) :");
        var input = Console.ReadLine();


        // Valider que l'entrée est un nombre, qu'elle est positif,
        // et qu'elle est à la portée de Owen
        if (! int.TryParse(input, out cible[0]))
        {
            Console.WriteLine("Veuillez entrer un nombre !");
        }
        else if( cible[0] < 0 && cible[0] >= nbLignes )
        {
            Console.WriteLine($"Veuillez entrer un nombre positif et inférieur à {nbLignes - 1 }.");
        }
        else if(Math.Abs(cible[0] - owenPos[0]) > 3)
        {
            Console.WriteLine("Cible hors de portée!");
        }
        else
        {
            break;
        }
    } while (true);



    // Saisie et validation du numéro de colonne de la cible
    do
    {
        Console.WriteLine("Entrez le numéro de colonne de la cible (doit être un nombre positif) :");
        var input = Console.ReadLine();


        // Valider que l'entrée est un nombre, qu'elle est positif,
        // et qu'elle est à la portée de Owen
        if (!int.TryParse(input, out cible[1]))
        {
            Console.WriteLine("Veuillez entrer un nombre !");
        }
        else if (cible[1] > 0 && cible[0] >= nbColonnes)
        {
            Console.WriteLine($"Veuillez entrer un nombre positif et inférieur à {nbColonnes - 1}.");
        }
        else if (Math.Abs(cible[1] - owenPos[1]) > 3)
        {
            Console.WriteLine("Cible hors de portée!");
        }
        else
        {
            break;
        }
    } while (true);

    return cible;
}

// Fonction pour placer les personnages dans le plateau
void PlacerPersonnages()
{
    plateau[owenPos[0], owenPos[1]] = OWEN;
    plateau[bluePos[0], bluePos[1]] = BLUE;
    plateau[maisiePos[0], maisiePos[1]] = MAISIE;
    plateau[indominusPos[0], indominusPos[1]] = INDOMINUS;
}

void AfficherPlateau(char[,] plateau)
{
    Console.Clear();


    // Afficher le plateau
    for (int i = 0; i < nbLignes; i++)
    {
        Console.Write("|"); // Numéros de lignes
        for (int j = 0; j < nbColonnes; j++)
        {
            Console.Write($"{plateau[i, j]}|");
        }
        Console.WriteLine();
    }
}


void DeplacerManuellement(char personnage, int[] position)
{
    string nomPersonnage = personnage == OWEN ? "Owen" : "Blue";
    Console.WriteLine($"Utiliser les fleches pour déplacer {nomPersonnage}...");

    bool deplacementValide = false;
    int nouveauX, nouveauY;
    ConsoleKey direction;

    while (!deplacementValide)
    {
        // Variables pour déterminer les nouvelles positions
        nouveauX = position[0];
        nouveauY = position[1];
        do
        {
            direction = Console.ReadKey().Key;

            switch (direction)
            {
                case ConsoleKey.RightArrow:
                    nouveauY++;
                    break;

                case ConsoleKey.LeftArrow:
                    nouveauY--;
                    break;

                case ConsoleKey.UpArrow:
                    nouveauX--;
                    break;

                case ConsoleKey.DownArrow:
                    nouveauX++;
                    break;

                default:
                    Console.WriteLine("Veuillez utiliser les fleches pour deplacer le personnage...");
                    break;
            }
        } while (nouveauX == position[0] && nouveauY == position[1]);


        // Vérifier si le déplacement est valide (dans les limites du plateau)
        if (nouveauX >= 0 && nouveauX < nbLignes && nouveauY >= 0 && nouveauY < nbColonnes)
        {
            if (plateau[nouveauX, nouveauY] == VIDE)
            {
                // Effectuer le déplacement
                plateau[position[0], position[1]] = VIDE;
                position[0] = nouveauX;
                position[1] = nouveauY;
                plateau[position[0], position[1]] = personnage;
                deplacementValide = true;
            }
            else if (personnage == BLUE && plateau[nouveauX, nouveauY] == INDOMINUS)
            {
                //Logique permettant de repousser I de 3 cases ou jusqu'au bord d'un fossé proche
            }
            else
            {
                Console.WriteLine("Déplacement invalide !\n Veuillez choisir une autre direction...");
            }
        }
    }

}

void LancerGrenade() {

    int [] cible1 = RenseignerCible();
    int[] cible2 = cible1;
    int direction;

    do
    {
        // Choisir une direction aléatoire : 0 = haut, 1 = bas, 2 = gauche, 3 = droite
        direction = generateur.Next(0, 4);

         switch (direction)
        {
            case 0: // Haut
                cible2[0] --;
                break;
            case 1: // Bas
                cible2[0]++;
                break;
            case 2: // Gauche
                cible2[1]--;
                break;
            case 3: // Droite
                cible2[1]++;
                break;
        }

        if(EstDeplacementValide(cible2))
        {
            break;
        }

    } while (true);

    //Verifier si il ne touche pas un des personnages
    // Si oui fin partie.
    //Sinon Placer trous et decrementer grenades

}

bool EstDeplacementValide(int[] position)
{
    return position[0] >= 0 && position[0] < nbLignes &&
           position[1] >= 0 && position[1] < nbColonnes;
}

void DeplacerAleatoirement(char personnage, int[] position)
{
    bool deplacementValide = false;
    int nouveauX, nouveauY, direction;

    while (!deplacementValide)
    {
        // Choisir une direction aléatoire : 0 = haut, 1 = bas, 2 = gauche, 3 = droite
        direction = generateur.Next(0, 4);

        // Variables pour déterminer les nouvelles positions
        nouveauX = position[0];
        nouveauY = position[1];

        switch (direction)
        {
            case 0: // Haut
                nouveauX--;
                break;
            case 1: // Bas
                nouveauX++;
                break;
            case 2: // Gauche
                nouveauY--;
                break;
            case 3: // Droite
                nouveauY++;
                break;
        }

        // Vérifier si le déplacement est valide (dans les limites du plateau)
        if (EstDeplacementValide([nouveauX, nouveauY]))
        {
            if (plateau[nouveauX, nouveauY] == VIDE)
            {
                // Effectuer le déplacement
                plateau[position[0], position[1]] = VIDE;
                position[0] = nouveauX;
                position[1] = nouveauY;
                plateau[position[0], position[1]] = personnage;
                deplacementValide = true;
            }
            //  Vérifier si I ne prend pas la place de O ou B
            else if (personnage == INDOMINUS)
            {
                if (plateau[nouveauX, nouveauY] == OWEN)
                {
                    partieTerminee = true;
                    raisonDeLaFin = "Indominus Rex a éliminé Owen.";

                    // Effectuer le déplacement
                    plateau[nouveauX, nouveauY] = VIDE;
                    position[0] = nouveauX;
                    position[1] = nouveauY;
                    plateau[position[0], position[1]] = personnage;
                    break;
                }
                if (plateau[position[0], position[1]] == MAISIE)
                {
                    partieTerminee = true;
                    raisonDeLaFin = "Indominus Rex a éliminé Maisie.";

                    // Effectuer le déplacement
                    plateau[position[0], position[1]] = VIDE;
                    position[0] = nouveauX;
                    position[1] = nouveauY;
                    plateau[position[0], position[1]] = personnage;
                    break;
                }
            }

        }

    }
}