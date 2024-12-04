Console.WriteLine("Bienvenue dans le jeu");

//-----------------DECLARATION DE VARIABLES-------------------------------

// Nombre de ligne et de colonnes de notre plateau
int nbLignes, nbColonnes;
bool partieTerminee;
string raisonDeLaFin;

// Position de chacun des personnages stockée dans des tableaux [x,y]
int[] owenPos, indominusPos, bluePos, maisiePos;

// Plateau qui est un tableau de caractères à 2D
char[,] plateau;

//Caractères permettant d'identifier chaque personnage sur le plateau
const char VIDE = ' ';
const char OWEN = 'O';
const char BLUE = 'B';
const char MAISIE = 'M';
const char INDOMINUS = 'I';

Random rng = new();

//------------------FIN DECLARATION--------------------------



RenseignerDimensionsPlateau();

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

void deplacerBlue(int[] blue, int[] indominus)
{
    if (indominus[0] > blue[0] + 1)
    {
        blue[0] += 2;
    }


    else if (indominus[0] < blue[0] - 1)
    {
        blue[0] -= 2;
    }


    else if (indominus[1] > blue[1] + 1)
    {
        blue[1] += 2;
    }


    else if (indominus[1] < blue[1] - 1)
    {
        blue[1] -= 2;
    }


    else if (indominus[0] > blue[0])
    {
        blue[0] += 1;
    }


    else if (indominus[0] < blue[0])
    {
        blue[0] -= 1;
    }


    else if (indominus[1] > blue[1])
    {
        blue[1] += 1;
    }


    else if (indominus[1] < blue[1])
    {
        blue[1] -= 1;
    }
}

void DeplacerAleatoirement(char personnage, int[] position)
{
    bool deplacementValide = false;
    int nouveauX, nouveauY, direction;

    while (!deplacementValide)
    {
        // Choisir une direction aléatoire : 0 = haut, 1 = bas, 2 = gauche, 3 = droite
        direction = rng.Next(0, 4);

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