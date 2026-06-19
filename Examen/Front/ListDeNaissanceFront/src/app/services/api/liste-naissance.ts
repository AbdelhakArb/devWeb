import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ListeDeNaissance } from './models/liste-naissance';
import { Article } from './models/article';

@Injectable({
  providedIn: 'root'
})
export class ListeNaissanceService {
  private apiUrl = 'https://localhost:5141/api/listenaissance';

  constructor(private http: HttpClient) {}

  // 1. Créer une liste
  creerListe(liste: ListeDeNaissance): Observable<any> {
    return this.http.post<any>(this.apiUrl, liste);
  }

  // 2. Obtenir les listes d'un parent
  obtenirListesParParent(parentId: number): Observable<ListeDeNaissance[]> {
    return this.http.get<ListeDeNaissance[]>(`${this.apiUrl}/parent/${parentId}`);
  }

  // 3. Charger une liste par son ID unique (Visiteur)
  chargerListeParId(id: number): Observable<ListeDeNaissance> {
    return this.http.get<ListeDeNaissance>(`${this.apiUrl}/${id}`);
  }

  // 4. Charger les articles d'une liste spécifique
  chargerArticlesDeLaListe(listeId: number): Observable<Article[]> {
    return this.http.get<Article[]>(`${this.apiUrl}/${listeId}/articles`);
  }

  // 5. Ajouter ou mettre à jour un article dans une liste (Parent)
  ajouterArticleDansListe(listeId: number, articleId: number, quantite: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${listeId}/articles`, { articleId, quantite });
  }

  // 6. Réserver un article (Visiteur)
  reserverArticle(listeId: number, articleId: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${listeId}/articles/${articleId}/reserver`, {});
  }
}