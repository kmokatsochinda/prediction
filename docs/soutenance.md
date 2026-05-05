# Notes de soutenance

## Sujet

ParisMarket est une application web de prédictions. Un utilisateur peut consulter les prédictions ouvertes, voter Oui ou Non, créer une nouvelle prédiction et consulter les résultats.

## Fonctionnalités à montrer

1. Page d'accueil avec les prédictions ouvertes.
2. Sélection d'un utilisateur de démonstration.
3. Vote Oui/Non avec blocage du double vote.
4. Création d'une prédiction avec une date de clôture.
5. Page de résultats avec clôture d'une prédiction.

## Points techniques à expliquer

- `Pages/` contient les écrans Blazor.
- `backend/PredictionService.cs` contient les règles métier principales.
- `backend/PredictionDataStore.cs` lit et écrit les données dans `data.json`.
- `SimpleTests.cs` vérifie les cas importants sans modifier les données réelles.

## Règles métier importantes

- Une question ne peut pas être vide.
- La date de clôture doit être future.
- Un utilisateur ne peut voter qu'une seule fois par prédiction.
- Une prédiction clôturée n'accepte plus de vote.
- Le résultat final est enregistré au moment de la clôture.

## Limites assumées

- Les utilisateurs sont fictifs.
- Le stockage JSON est choisi pour la simplicité, pas pour une vraie production.
- Il n'y a pas encore d'authentification.
- Les tests sont simples et pourraient être remplacés par xUnit plus tard.

## Commandes utiles

```bash
dotnet build SiteDePrediction.csproj --configuration Release
dotnet run --project SiteDePrediction.csproj --configuration Release -- --test
dotnet run --project SiteDePrediction.csproj
```
