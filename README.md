# ParisMarket

## 1. C’est quoi ce projet ?

ParisMarket est une application web de prédictions.

Elle permet de créer une question sur un événement à venir.

Les utilisateurs peuvent ensuite voter Oui ou Non.

Ils peuvent aussi voir les prédictions ouvertes et consulter les résultats.

Le but est de montrer une application simple, facile à comprendre et à utiliser.

## 2. À quoi ça sert concrètement ?

ParisMarket peut servir à imaginer des paris ou des sondages simples.

Exemples :

- prédire le résultat d’un match
- prédire si un produit va sortir bientôt
- demander l’avis des autres sur un événement
- voir combien de personnes pensent Oui ou Non
- comparer les votes une fois la prédiction terminée

## 3. Comment ça fonctionne (version simple)

1. On crée une prédiction.
2. Les utilisateurs votent Oui ou Non.
3. Les votes sont enregistrés dans data.json
4. On peut clôturer la prédiction.
5. On voit le résultat final.

## 4. Comment lancer le projet (très important)

### Étapes :

1. Ouvrir un terminal.
2. Aller dans le dossier du projet.
3. Taper :

```bash
dotnet build
dotnet run
```

4. Ouvrir le navigateur à l’adresse affichée dans le terminal.

Si tout fonctionne, vous verrez l’application s’ouvrir dans votre navigateur.

## 5. Technologies utilisées (simple)

- C# → langage utilisé pour créer le projet
- Blazor → permet de créer l’interface web
- JSON → permet de sauvegarder les données

##  6. Organisation du projet

- Pages → les pages visibles de l’application
- backend → la logique du projet, comme les votes et les prédictions
- wwwroot → les fichiers visuels, comme le style, les images et le JavaScript
- docs → les documents d’explication du projet

## 7. Roadmap dans le temps de ce qu'on a pu faire

**Phase 1 — Conception**
- Définition du projet et des fonctionnalités = Tout le monde
- Répartition des rôles = Tout le monde
- Modélisation des données (Prediction, Vote, User) = Maxence

**Phase 2 — Mise en place**
- Création des pages Blazor (accueil, création, résultats) = Eddy
- Mise en place du stockage JSON = Maxence 
- Première logique de création et de vote = Karl

**Phase 3 — Fonctionnalités**
- Ajout des règles métier (double vote, clôture, validation) = Karl Mike
- Connexion entre frontend et backend = Maxence
- Affichage des résultats (pourcentages) = Eddy

**Phase 4 — Amélioration**
- Sécurisation des règles = Karl Mike
- Ajout de tests simples = Maxence
- Amélioration de l’interface et du design = Eddy

**Phase 5 — Finalisation**
- Tests globaux = Karl et Maxence
- Correction des bugs = Karl
- Préparation du projet pour la présentation = Eddy



## 8. Limites du projet

- Il n’y a pas de vraie base de données.
- Les données sont sauvegardées dans un fichier JSON.
- Il n’y a pas de gestion avancée des utilisateurs.
- Les utilisateurs sont des profils de démonstration.
- Le projet est simple et fait surtout pour apprendre.

## 9. Améliorations possibles

- ajouter une vraie base de données
- ajouter une connexion utilisateur
- améliorer le design
- ajouter plus de types de votes
- afficher plus de statistiques

## 👨 10. Auteurs

- Karl Mike
- Marchand Maxence
- Eddy
