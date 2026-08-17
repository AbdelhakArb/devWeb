namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface IObtenirTousLesModelsUseCase
    {
        Task<IEnumerable<Models.ModelDeListeDeNaissance>> ExecuterAsync();
    }

    public interface IObtenirArticlesDuModelUseCase
    {
        Task<IEnumerable<Models.Article>> ExecuterAsync(int modelId);
    }
}