Console.WriteLine("Bienvenue dans le jeu");

//-----------------DECLARATION DE VARIABLES-------------------------------

// Nombre de ligne et de colonnes de notre plateau
int nbLignes, nbColonnes;
bool partieTerminee;
string raisonDeLaFin;

// Position de chacun des personnages stockée dans des tableaux [numLigne,numColonne]
int[] owenPos, indominusPos, bluePos, maisiePos;

//Le nombre de grenade que Owen peut lancer
int grenades;

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


//------------------Fonctions---------------------------------------

// Fonction permettant d'avoir le nombre de lignes et de colonnes de notre plateau
// Et d'initialiser les variables qui en dependent
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

    //Affecter le ombre de grenades à la plus grande dimension
    grenades = Math.Max(nbLignes, nbColonnes);

// Création du plateau
    plateau = new char[nbLignes, nbColonnes];

    // Affectation de valeurs de depart à M et IR
    maisiePos = [nbLignes - 1, nbColonnes - 1];
    indominusPos = [nbLignes / 2, nbColonnes / 2];
}

int[] RenseignerCible()
{
    Console.WriteLine("Choix de la cible par Owen avec portée de 3 cases :");

    int x, y;
    bool estValide = false;

    // Saisie et validation du numéro de ligne de la cible
    do
    {
        Console.WriteLine("Donner l'abscisse de l'emplacement du jet de grenade (doit être un nombre positif) :");
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
        Console.WriteLine("Donner l'ordonnée de l'emplacement du jet de grenade (doit être un nombre positif) :");
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

void InitialiserPlateau()
{
    for (int i = 0; i < nbLignes; i++)
    {
        for (int j = 0; j < nbColonnes; j++)
        {
            plateau[i, j] = vide;
        }
    }
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

void DeplacerOwen()
{
    Console.WriteLine("Utilisez les flèches pour diriger Owen.");

    bool deplacementValide = false;
    int nouveauX, nouveauY;
    ConsoleKey direction;

    while (!deplacementValide)
    {
        // Variables pour déterminer les nouvelles positions
        nouveauX = owenPos[0];
        nouveauY = owenPos[1];
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
                    Console.WriteLine("Veuillez utiliser les fleches pour deplacer le Owen...");
                    break;
            }
        } while (nouveauX == owenPos[0] && nouveauY == owenPos[1]);


        // Vérifier si le déplacement est valide (dans les limites du plateau)
        if (nouveauX >= 0 && nouveauX < nbLignes && nouveauY >= 0 && nouveauY < nbColonnes)
        {
            if (plateau[nouveauX, nouveauY] == vide)
            {
                // Effectuer le déplacement
                plateau[owenPos[0], owenPos[1]] = vide;
                owenPos[0] = nouveauX;
                owenPos[1] = nouveauY;
                plateau[owenPos[0], owenPos[1]] = owen;
                deplacementValide = true;
            }
            else
            {
                Console.WriteLine("Déplacement invalide !\n Veuillez choisir une autre direction...");
            }
        }
    }
}

void DeplacerBlue()
{
    Console.WriteLine("Utilisez les flèches pour diriger Blue.");

    bool deplacementValide = false;
    int nouveauX, nouveauY;
    ConsoleKey direction;

    while (!deplacementValide)
    {
        // Variables pour déterminer les nouvelles positions
        nouveauX = bluePos[0];
        nouveauY = bluePos[1];
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
                    Console.WriteLine("Veuillez utiliser les fleches pour deplacer le Blue...");
                    break;
            }
        } while (nouveauX == bluePos[0] && nouveauY == bluePos[1]);


        // Vérifier si le déplacement est valide (dans les limites du plateau)
        if (nouveauX >= 0 && nouveauX < nbLignes && nouveauY >= 0 && nouveauY < nbColonnes)
        {
            if (plateau[nouveauX, nouveauY] == vide)
            {
                // Effectuer le déplacement
                plateau[bluePos[0], bluePos[1]] = vide;
                bluePos[0] = nouveauX;
                bluePos[1] = nouveauY;
                plateau[bluePos[0], bluePos[1]] = blue;
                deplacementValide = true;
            }
            else if (plateau[nouveauX, nouveauY] == indominus)
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

void JetGrenades()
{
    Console.WriteLine($"Nombre de grenades restant: {grenades}");
    Console.WriteLine("Owen veut il lancer une grenade? \nRépondre par N pour Non et autre chose pour Oui...");
    var choix = Console.ReadLine();
    if (choix == "N")
        return;
    int[] cible1 = RenseignerCible();
    int[] cible2 = [0, 0];

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
        if (plateau[cible2[0], cible2[1]] == maisie || plateau[cible2[0], cible2[1]] == owen ||
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

    grenades--;
    if (VerifierIndominus())
    {
        partieTerminee = true;
        raisonDeLaFin = "===== VICtoiRE ===== \nOwen a piégé Indominus";
    }

    if (grenades <= 0)
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

(int, int) DeplacerAleatoirement(int direction, int[] position)
{
    int nouveauX = position[0];
    int nouveauY = position[1];

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

    return (nouveauX, nouveauY);
}

bool EffectuerDeplacement(int nouveauX, int nouveauY, bool deplacementValide)
{
    // Effectuer le déplacement si la case est libre
    if (plateau[nouveauX, nouveauY] == vide)
    {
        plateau[indominusPos[0], indominusPos[1]] = vide;
        indominusPos[0] = nouveauX;
        indominusPos[1] = nouveauY;
        plateau[indominusPos[0], indominusPos[1]] = indominus;
        deplacementValide = true;
    }
    //  Vérifier si I ne prend pas la place de O ou B
    else if (plateau[nouveauX, nouveauY] == owen)
    {
        partieTerminee = true;
        raisonDeLaFin = "===== DEFAITE ===== \nIndominus Rex a éliminé Owen.";

        // Effectuer le déplacement
        plateau[indominusPos[0], indominusPos[1]] = vide;
        indominusPos[0] = nouveauX;
        indominusPos[1] = nouveauY;
        plateau[indominusPos[0], indominusPos[1]] = indominus;
        deplacementValide = true;
    }

    else if (plateau[nouveauX, nouveauY] == maisie)
    {
        partieTerminee = true;
        raisonDeLaFin = "===== DEFAITE ===== \nIndominus Rex a éliminé Maisie.";

        // Effectuer le déplacement
        plateau[indominusPos[0], indominusPos[1]] = vide;
        indominusPos[0] = nouveauX;
        indominusPos[1] = nouveauY;
        plateau[indominusPos[0], indominusPos[1]] = indominus;
        deplacementValide = true;
    }

    return deplacementValide;
}

void DeplacerIndominus()
{
    bool deplacementValide = false;
    int nouveauX, nouveauY, direction;

    do
    {
        // Choisir une direction aléatoire : 0 = haut, 1 = bas, 2 = gauche, 3 = droite
        direction = generateur.Next(0, 4);


        (nouveauX, nouveauY) = DeplacerAleatoirement(direction, indominusPos);

        // Refais le choix si le déplacement est invalide
        if (!EstDeplacementValide([nouveauX, nouveauY])) continue;

        deplacementValide = EffectuerDeplacement(nouveauX, nouveauY, deplacementValide);
    } while (!deplacementValide);

    if (!partieTerminee && aToucheIndominus)
    {
        deplacementValide = false;
        int direction2;
        do
        {
            // Choisir une direction aléatoire qui est differente de celle choisie précèdemment
            do
            {
                direction2 = generateur.Next(0, 4);
            } while (direction2 != direction);

            (nouveauX, nouveauY) = DeplacerAleatoirement(direction2, indominusPos);

            // Refais le choix si le déplacement est invalide
            if (!EstDeplacementValide([nouveauX, nouveauY])) continue;

            deplacementValide = EffectuerDeplacement(nouveauX, nouveauY, deplacementValide);
        } while (!deplacementValide);
    }
}

void DeplacerMaisie()
{
    bool deplacementValide = false;
    int nouveauX, nouveauY, direction;

    while (!deplacementValide)
    {
        // Choisir une direction aléatoire : 0 = haut, 1 = bas, 2 = gauche, 3 = droite
        direction = generateur.Next(0, 4);

        (nouveauX, nouveauY) = DeplacerAleatoirement(direction, maisiePos);

        // Refais le choix si le déplacement est invalide
        if (!EstDeplacementValide([nouveauX, nouveauY])) continue;

        // Effectuer le déplacement si la case est libre
        if (plateau[nouveauX, nouveauY] == vide)
        {
            plateau[maisiePos[0], maisiePos[1]] = vide;
            maisiePos[0] = nouveauX;
            maisiePos[1] = nouveauY;
            plateau[maisiePos[0], maisiePos[1]] = maisie;
            deplacementValide = true;
        }
        else if (plateau[nouveauX, nouveauY] == indominus)
        {
            // Repousser Indominus Rex de 3 cases  ou sur le trou le plus proche
            int finalX = nouveauX;
            int finalY = nouveauY;

            //Verifier s'ils sont sur la même ligne
            if (finalX == maisiePos[0])
            {
                //Essayer de deplacer I de trois cases en restant sur la même ligne
                for (int i = 0; i < 3; i++)
                {
                    if (maisiePos[1] < finalX)
                        finalY++;
                    else
                    {
                        finalY--;
                    }

                    // Verifier si le déplacement est valide
                    if (EstDeplacementValide([finalX, finalY]))
                    {
                        plateau[indominusPos[0], indominusPos[1]] = vide;
                        indominusPos = [finalX, finalY];
                        plateau[indominusPos[0], indominusPos[1]] = indominus;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            //Impossible de repousser Indominus --> Fin de partie I a perdu
                            partieTerminee = true;
                            raisonDeLaFin = "===== VICTOIRE ===== \nIndominus Rex a été éliminé par Blue.";

                            plateau[maisiePos[0], maisiePos[1]] = vide;
                            maisiePos = [nouveauX, nouveauY];
                            plateau[maisiePos[0], maisiePos[1]] = maisie;
                            deplacementValide = true;
                        }

                        break;
                    }

                    plateau[maisiePos[0], maisiePos[1]] = vide;
                    maisiePos = [nouveauX, nouveauY];
                    plateau[maisiePos[0], maisiePos[1]] = maisie;
                    deplacementValide = true;
                }
            }
            //Sinon ils sont sur la même colonne
            else
            {
                //Essayer de deplacer I de trois cases en restant sur la même colonne
                for (int i = 3; i > 0; i--)
                {
                    finalX += i;

                    // Verifier si le déplacement est valide
                    if (EstDeplacementValide([finalX, finalY]))
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

// fonction principale du jeu
void JouerLeJeu()
{
    // Affectation des positions de départ de O et B
    owenPos = [0, 0];
    bluePos = [0, 1];
    
    //Permet de savoir quand IR est énérvé
    aToucheIndominus = false;
    
    partieTerminee = false;
    raisonDeLaFin = "";
    
    RenseignerDimensionsPlateau();
    InitialiserPlateau();


    while (true)
    {
        PlacerPersonnages();
        AfficherPlateau();


        Console.WriteLine("Tour de Owen :");
        DeplacerOwen();
        AfficherPlateau();


        JetGrenades();
        AfficherPlateau();


        Console.WriteLine("Tour de Blue :");
        DeplacerBlue();
        AfficherPlateau();


        Console.WriteLine("Tour de l'Indominus :");
        DeplacerIndominus();
        AfficherPlateau();

        Console.WriteLine("Pause. Appuyer pour continuer...");
        Console.ReadLine();
        Console.Clear();
        
        Console.WriteLine("Tour de maisie:");
        DeplacerMaisie();
        AfficherPlateau();

        
        if (partieTerminee)
        {
            Console.WriteLine($"{raisonDeLaFin}");
            break;
        }


        Console.WriteLine("Fin du tour. Appuyez sur Entrée pour au tour suivant...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Le programme a repris !");
    }
}

//------------------Fin Fonctions---------------------------------------

//Appel de la fonction principale
JouerLeJeu();