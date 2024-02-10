# Labyrinth-Game: My final project for last semester.

# Description: 

A small labyrinth game made in C# featuring a "hero" that must reach the objective while fighting monsters and collecting objects such as potions, weapons, and pickaxes.

The game features a simple battle mechanic involving stats like AP (attack power) and BP (defense power), movement speed, and various effects that modify these stats when using collectibles.

There are two versions: a Console Application and a Windows Forms Application.


# Changelog  (French only):

- 08 novembre 2023:
  
  Création des classes Carte, Random, Epee, Bouclier, Pic et Personnage du DLL et de leur squelette
  
  Finalisation de la classe Hasard

          
- 09 novembre 2023:

  Ajout des proprietes dans les classes du DLL et de leur Getter/Setter


- 11 novembre 2023:

  Création des classes Chasseur, Monstres et Monstre et de leur squelette

          
- 12 novembre 2023:

  Création du squelette de la classe Jeu_Console. 
 
  Implémentation des boucles principale et secondaires.

  Implémentation de la validation du nom du joueur et des dimensions de la carte

  Création d'un mini 'Lore' pour mettre de l'ambiance.

  Création de la première carte (foret(simple).map)


- 13 novembre 2023:

  Modification du constructeur de la classe Carte. Implémentation du code permettant de boucler dans le dossier Debug afin de lister le nom des cartes (fichiers .map)

  Ajout d'un menu de selection de niveaux dans la classe Jeu_Console.
  
  Ajout d'une validation pour le choix de niveau.

  Finalisation de la fonction ChargerCarte() de la classe Carte. Créé correctement les objets Chasseur, Monstres, et rempli les listes de pics, boucliers, epees, et potions.
  
  Finalisation de la fonction DessinerCarte() de la classe Jeu_Console. Dessine correctement la carte choisie à partir du fichier .map.
  
									 
- 14 novembre 2023:

  Implémentation du mouvement du joueur dans la classe Jeu_Console. Le joueur se déplace sans quitter les limites de la carte et sans passer au travers des murs.
  
  Ajout de la fonctionnalité des pics. Le joueur peut détruire un mur s'il possède un pic dans son inventaire de pics et le détruit selon une probabilité (propritété de la classe Pic)
  
									 
- 15 novembre 2023:

  Ajout des combats à la classe Jeu_Console. Le joueur peut attaquer et se défendre contre les monstres lorsqu'il atteint leur positions.
  
  Implémentation du UI de la classe Jeu_Console. Affiche et actualise les statistiques du joueur ainsi que les actions éffectuées.
  
  Ajout d'une fonctionnalité de l'UI (message de fin). Se positionne en fonction des dimensions de la carte.
  
									 
- 16 novembre 2023:

  Implémentation de la fin de partie.
  
  Ajout d'une fonction pour passer au niveau suivant en cas de victoire.
  
  Ajout d'une fonction pour recommencer le niveau en cas de mort.
  
  Ajout d'une boucle pour retourner au menu de selection de niveau si le joueur ne veux ni recommencer, ni aller au niveau suivant
  
  Ajout de credits
  

- 17 novembre 2023:
  
  Creation des forms FrmIntro et FrmJeu de la classe Jeu_Graph
  
  Design des forms (ajout de textbox, boutons, picturebox, etc.)
  
  Finalisation de la form FrmIntro. Implémentation de la validation du nom du chasseur et des dimension de la carte + choix de niveau
  
									 
- 18 novembre 2023:

  Création de la classe SpritePic dans le DLL. J'ai choisi de dessiner les pics dans la version Windows Forms, car il utilise à la fois une ligne et une courbe.
  
  Création de la fonction DessinerCarte() de la classe Jeu_Graph.
  
  Création de la classe Pamarètres dans le DLL pour regrouper toutes les constantes au même endroit.
  
									 
- 19 novembre 2023:

  Implémentation des mouvements du joueur
  
  Implémentation de l'intéraction avec les objets
  
  Implémentation des combats du joueur avec les monstres
  
  Implémentation de la fin de partie (victoire + défaite)
  
  Implémentation de l'UI (forms). S'ajuste en fonction des dimension de la carte
  
  Ajout d'une fonctionnalité à l'UI. La fenêtre de jeu se redimensionne automatiquement en fonction des dimensions de la carte choisie
  
										
- 21 novembre 2023:  

  Refactoring pour faciliter la lecture et la ré-utilisation du code en évitant les fonctions appelant d'autres fonctions.
  
										
- 22 novembre 2023:

  Création de l'interface IEtat et des classes en héritant. Implémentation du code avec celui de la classe Potion. Le joueur change désormais d'état en fonction du type de potion ramassée (console et forms)
  
  Ajout de la fonction RedessinerMonstre(). Si le joueur invisible est par-dessus un monstres, le redessine lorsqu'il change de position (Semble ne plus fonctionner avec l'ajout des threads. Si le monstre bouge en meme temps que le joueur, il ne se redessine que lorsque qu'il bouge a nouveau. Il demeure donc 
  invisible un certain temps.)
  
										
- 23 novembre 2023:

  Créations des threads secondaires du joueur et des monstres dans la classe Jeu_Console. Le joueur se déplace dans les threads et est sujet au freezetime
  
  Création de la thread secondaire du joueur (forms)
  
  Création des fonctions pour faire bouger les monstres automatiquement (console et forms).
  
									 
- 25 novembre 2023:

  Modification des fonctions pour faire bouger les monstres automatiquement.
  
  Synchronisation des positions à la fois dans la carte et dans le niveau pour faciliter la gestion des mouvements
  
  Finalisation de la fonction pour faire bouger les monstres (console). Les monstres peuvent bouger et intéragir avec le joueur automatiquement
  
  Essais et erreur dans les fonctions pour faire bouger les monstres correctement (forms). Message d'erreur lors des mouvements. Rien ne semble fonctionner pour l'instant.
  

- 26 novembre 2023:

  Finalisation de la fonction pour faire bouger les monstres (forms). L'erreur étaient dans les paramétres des fonctions. Les positions dans le niveau et dans la carte n'étaient pas correctement synchronisées.
  
										
- 29 novembre 2023:

  Modification des fonctions pour faire bouger les monstres (console et forms). Les monstres n'attaquent plus le joueur s'il est invisible
  Finalisation de la fonction pour faire bouger les monstres entre les threads (forms). Les messages d'actions s'affichent maintenant quand les monstres attaquent le joueur et disparraissent s'ils meurt durant l'attaque
  Ajout de la classe GestionErreur dans le DLL. Ajout d'un try/catch pour chaque fonction de la solution tel que demandé dans l'énoncé
  Ajout de la classe Score dans le DLL. Lis le fichier score.txt et rempli une liste de string, qui est ensuite triée, puis réécrite dans le fichier après l'ajout d'un nouveau score
									       - Création du fichier README.txt. Centralisation de l'historique du projet dans un seul fichier plutôt que de les répartir parmis les différents fichiers .cs .
									       - Création de #region pour faciliter la lecture du code
