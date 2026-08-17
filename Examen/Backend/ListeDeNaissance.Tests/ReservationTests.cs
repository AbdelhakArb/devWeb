using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Xunit;
using Infra.Repositories;
using ListeDeNaissance.Core.Models;

public class RepositoryReservationTest
{
    [Fact]
    public async Task SoumettreReservationsRepoAsync_DoitInsererReservation_QuandPanierValide()
    {
        // ARRANGE : Configuration de la vraie chaîne de connexion vers ta base de test/développement
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"ConnectionStrings:DefaultConnection", "Server=localhost;Database=gestiondeslistesdenaissances;Uid=root;Pwd=coucou;"}
            })
            .Build();

        var repository = new ListeDeNaissanceRepository(configuration);

        // Assure-toi que ces IDs existent réellement dans ta base de données pour que le test passe
        var panier = new List<ReservationRequestItem>
        {
            new ReservationRequestItem
            {
                ListeDeNaissanceId = 1,
                ArticleId = 1,
                VisiteurId = 5,
                QtySouhaitee = 1,
                MessageText = "Félicitations pour le bébé !",
                SignatureMessage = "Test unitaire"
            }
        };

        // ACT : Exécution de la méthode seule
        var resultat = await repository.SoumettreReservationsRepoAsync(panier);

        // ASSERT : Vérification du retour
        Assert.True(resultat);
    }
}