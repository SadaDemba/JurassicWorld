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

bool aToucheIndominus;

// Plateau qui est un tableau de caractères à 2D
char[,] plateau;

//Caractères permettant d'identifier le contnu de chaque case le plateau
const char vide = ' ';
const char owen = 'O';
const char blue = 'B';
const char maisie = 'M';
const char indominus = 'I';
const char trou = '*';

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
        plateau[i, j] = vide;
    }
}


// Affectation des positions de départ des utilisateurs
owenPos = [0, 0];
bluePos = [0, 1];
maisiePos = [nbLignes - 1, nbColonnes - 1];
indominusPos = [nbLignes / 2, nbColonnes / 2];

aToucheIndominus = false;
partieTerminee = false;
raisonDeLaFin = "";

while (true) // Ajouter une boucle pour continuer à jouer
{
    PlacerPersonnages();
    AfficherPlateau();

    //Tour de Owen
    DeplacerManuellement(owen, owenPos);
    AfficherPlateau();
    
    LancerGrenade();
    AfficherPlateau();

    //Tour de Blue
    DeplacerManuellement(blue, bluePos);
    AfficherPlateau();

    //Tour de Indominus Rex
    DeplacerAleatoirement(indominus, indominusPos);
    AfficherPlateau();
    
    //Tour de Maisie
    DeplacerAleatoirement(maisie, maisiePos);
    AfficherPlateau();

    if (partieTerminee)
    {
        Console.WriteLine($"{raisonDeLaFin}");
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

int[] RenseignerCible()
{
    Console.WriteLine("Choix de la cible par Owen avec portée de 3 cases :");
    
    int x, y;
    bool estValide = false;

    // Saisie et validation du numéro de ligne de la cible
    do
    {
        Console.WriteLine("Entrez le numéro de ligne de la cible (doit être un nombre positif) :");
        var input = Console.ReadLine();
        
        estValide = int.TryParse(input, out x);
        
        // Valider que l'entrée est un nombre, qu'elle est positif,
        // et qu'elle est à la portée de Owen
        if (!estValide)
        {
            Console.WriteLine("Veuillez entrer un nombre !");
        }
        else if (x < 0 || x >= nbLignes)
        {
            Console.WriteLine($"Veuillez entrer un nombre positif et inférieur à {nbLignes}.");
        }
        else if (Math.Abs(x - owenPos[0]) > 3)
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


        estValide = int.TryParse(input, out y);
        // Valider que l'entrée est un nombre, qu'elle est positif,
        // et qu'elle est à la portée de Owen
        if (!estValide)
        {
            Console.WriteLine("Veuillez entrer un nombre !");
        }
        else if (y < 0 || y >= nbColonnes)
        {
            Console.WriteLine($"Veuillez entrer un nombre positif et inférieur à {nbColonnes}.");
        }
        else if (Math.Abs(y - owenPos[1]) > 3)
        {
            Console.WriteLine("Cible hors de portée!");
        }
        else
        {
            break;
        }
    } while (true);
    return [x, y];
}

// Fonction pour placer les personnages dans le plateau
void PlacerPersonnages()
{
    plateau[owenPos[0], owenPos[1]] = owen;
    plateau[bluePos[0], bluePos[1]] = blue;
    plateau[maisiePos[0], maisiePos[1]] = maisie;
    plateau[indominusPos[0], indominusPos[1]] = indominus;
}

void AfficherPlateau()
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
    string nomPersonnage = personnage == owen ? "Owen" : "Blue";
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
            if (plateau[nouveauX, nouveauY] == vide)
            {
                // Effectuer le déplacement
                plateau[position[0], position[1]] = vide;
                position[0] = nouveauX;
                position[1] = nouveauY;
                plateau[position[0], position[1]] = personnage;
                deplacementValide = true;
            }
            else if (personnage == blue && plateau[nouveauX, nouveauY] == indominus)
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

void LancerGrenade()
{
    Console.WriteLine("Owen veut il lancer une grenade? \nRépondre par N pour Non et autre chose pour Oui...");
    var choix = Console.ReadLine();
    if (choix == "N")
        return;
    int[] cible1 = RenseignerCible();
    int[] cible2 = [0,0];

    // Choisir la direction en comparant la position de la cible avec celle d'Owen
    if (cible1[0] < owenPos[0]) // Vers le haut
        cible2[0] = cible1[0] - 1;
    else if (cible1[0] > owenPos[0]) // Vers le bas
    {
        cible2[0] = cible1[0] + 1;
    }
    else
    {
        cible2[0] = cible1[0];
    }

    if (cible1[1] < owenPos[1]) // Vers la gauche
        cible2[1] = cible1[0] - 1;
    else if (cible1[1] > owenPos[1]) // Vers la droite
    {
        cible2[1] = cible1[1] + 1;
    }
    else
    {
        cible2[1] = cible1[1];
    }

    // Vérifier si la cible1 touche un personnage
    if (plateau[cible1[0], cible1[1]] == maisie || plateau[cible1[0], cible1[1]] == owen ||
        plateau[cible1[0], cible1[1]] == blue)
    {
        partieTerminee = true;
        raisonDeLaFin = "===== DEFAITE ===== \nUne grenade a touché un personnage par erreur.";
        plateau[cible1[0], cible1[1]] = trou;
        return;
    }
    
    if (plateau[cible1[0], cible1[1]] == indominus)
    {
        aToucheIndominus = true;
    }
    else
    {
        plateau[cible1[0], cible1[1]] = trou;
    }

    // Vérifier si la cible2 touche un personnage
    if (EstDeplacementValide(cible2))
    {
        
        if(plateau[cible2[0], cible2[1]] == maisie || plateau[cible2[0], cible2[1]] == owen ||
           plateau[cible2[0], cible2[1]] == blue)
        {
            partieTerminee = true;
            raisonDeLaFin = "===== DEFAITE ===== \nUne grenade a touché un personnage par erreur.";
            plateau[cible1[0], cible1[1]] = trou;
            return;
        }

        if (plateau[cible2[0], cible2[1]] == indominus)
        {
            aToucheIndominus = true;
        }
        else
        {
            plateau[cible2[0], cible2[1]] = trou;
        }
    }



    aToucheIndominus = plateau[cible1[0], cible1[1]] == indominus || aToucheIndominus;

    nbGrenades--;
    if (VerifierIndominus())
    {
        partieTerminee = true;
        raisonDeLaFin = "===== VICtoiRE ===== \nOwen a piégé Indominus";
    }
    
    if (nbGrenades <= 0)
    {
        Console.WriteLine("Plus de grenades disponibles !");
        partieTerminee = true;
        raisonDeLaFin = "===== DEFAITE ===== \nOwen a épuisé toutes ses grenades sans enfermer l'Indominus Rex.";
    }
}

bool EstDeplacementValide(int[] position)
{
    return position[0] >= 0 && position[0] < nbLignes &&
           position[1] >= 0 && position[1] < nbColonnes &&
           plateau[position[0], position[1]] != trou;
}

void DeplacerAleatoirement(char personnage, int[] position)
{
    bool deplacementValide = false;
    int nouveauX, nouveauY, direction;
    int nbCases = 1;
    
    //Fonctionnalité Indominus énérvé
    if (aToucheIndominus && personnage == indominus)
    {
        nbCases = 2;
    }

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
                nouveauX -= nbCases;
                break;
            case 1: // Bas
                nouveauX += nbCases;
                break;
            case 2: // Gauche
                nouveauY -= nbCases;
                break;
            case 3: // Droite
                nouveauY += nbCases;
                break;
        }

        // Refais le choix si le déplacement est invalide
        if (!EstDeplacementValide([nouveauX, nouveauY])) continue;

        // Effectuer le déplacement si la case est libre
        if (plateau[nouveauX, nouveauY] == vide)
        {
            plateau[position[0], position[1]] = vide;
            position[0] = nouveauX;
            position[1] = nouveauY;
            plateau[position[0], position[1]] = personnage;
            deplacementValide = true;
        }
        //  Vérifier si I ne prend pas la place de O ou B
        else if (personnage == indominus)
        {
            if (plateau[nouveauX, nouveauY] == owen)
            {
                partieTerminee = true;
                raisonDeLaFin = "===== DEFAITE ===== \nIndominus Rex a éliminé Owen.";

                // Effectuer le déplacement
                plateau[position[0], position[1]] = vide;
                position[0] = nouveauX;
                position[1] = nouveauY;
                plateau[position[0], position[1]] = personnage;
                break;
            }

            if (plateau[position[0], position[1]] == maisie)
            {
                partieTerminee = true;
                raisonDeLaFin = "===== DEFAITE ===== \nIndominus Rex a éliminé Maisie.";

                // Effectuer le déplacement
                plateau[position[0], position[1]] = vide;
                position[0] = nouveauX;
                position[1] = nouveauY;
                plateau[position[0], position[1]] = personnage;
                break;
            }
        }
        else if (personnage == blue && plateau[nouveauX, nouveauY] == indominus)
        {
            // Repousser Indominus Rex de 3 cases  ou sur le trou le plus proche
            int finalX = nouveauX ;
            int finalY = nouveauY ;

            if (finalX == position[0])
            {
                for (int i = 3; i >0 ; i--)
                {
                    finalY+= i;
        
                    // Verifier si le déplacement est valide
                    if (EstDeplacementValide([finalX, finalY] ))
                    {
                        plateau[indominusPos[0], indominusPos[1]] = vide;
                        indominusPos = [finalX, finalY];
                        plateau[indominusPos[0], indominusPos[1]] = indominus;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 3; i >0 ; i--)
                {
                    finalX+= i;
        
                    // Verifier si le déplacement est valide
                    if (EstDeplacementValide([finalX, finalY] ))
                    {
                        plateau[indominusPos[0], indominusPos[1]] = vide;
                        indominusPos = [finalX, finalY];
                        plateau[indominusPos[0], indominusPos[1]] = indominus;
                        break;
                    }
                }
            }
        }
    }
}

//Permet de verifiier si Indominus est bloqué
bool VerifierIndominus()
{
    int[] pos = indominusPos;
    bool encercleeHaut = pos[0] == 0 || (pos[0] > 0 && plateau[pos[0] - 1, pos[1]] == trou);
    bool encercleeGauche = pos[1] == 0 || (pos[1] > 0 && plateau[pos[0], pos[1] - 1] == trou);
    bool encercleeBas = pos[0] == nbLignes - 1 || (pos[0] < nbLignes - 1 && plateau[pos[0] + 1, pos[1]] == trou);
    bool encercleeDroite = pos[1] == nbColonnes - 1 || (pos[1] < nbColonnes - 1 && plateau[pos[0] + 1, pos[1]] == trou);

    return encercleeHaut && encercleeGauche && encercleeBas && encercleeDroite;
}