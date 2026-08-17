// Assure-toi que ce modèle correspond à ton ArticleDto C#
export interface ArticleDto {
  id: number;
  nom: string;
  description?: string;
  prix: number;
}

// Voici ton modèle principal qui fait le lien
export interface ArticleItemInListe {
  listeDeNaissanceId: number;
  qtySouhaitee: number;
  article: ArticleDto;
}