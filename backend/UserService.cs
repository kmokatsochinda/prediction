namespace SiteDePrediction;

// Permet de récupérer les utilisateurs de test.
public class UserService
{
    private readonly PredictionDataStore dataStore;

    public UserService(PredictionDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    // Retourne tous les utilisateurs de test.
    public IReadOnlyList<User> GetAll()
    {
        return dataStore.Load().Users;
    }

    // Cherche un utilisateur avec son identifiant.
    public User? GetById(int id)
    {
        return dataStore.Load().Users.FirstOrDefault(user => user.Id == id);
    }
}
