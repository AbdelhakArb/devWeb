export interface PanierItem {
  listeDeNaissanceId?: number;
  articleId: number;
  articleNom: string;
  articlePrix: number;
  articleQty: number;
  soustotalArticles: number;
  totalAPayer: number;
}