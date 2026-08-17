import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Article } from '../../services/api/models/article';

@Injectable({
  providedIn: 'root'
})
export class ListeNaissanceService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = 'http://localhost:5141/api/listenaissance';

  creerListe(data: any): Observable<any> {
    return this.http.post(this.baseUrl, data);
  }

  chargerListeParId(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${id}`);
  }

  obtenirArticlesDeLaListe(listeId: number): Observable<Article[]> {
    return this.http.get<Article[]>(`${this.baseUrl}/${listeId}/articles`);
  }

  ajouterArticle(listeId: number, articleId: number, quantite: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/${listeId}/articles`, { articleId, quantite });
  }

  ajouterArticleDansListe(listeId: number, articleId: number, quantite: number): Observable<any> {
    return this.ajouterArticle(listeId, articleId, quantite);
  }

  incrementerQuantite(listeId: number, articleId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/${listeId}/articles/incrementer`, { articleId });
  }

  decrementerQuantite(listeId: number, articleId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/${listeId}/articles/decrementer`, { articleId });
  }

  mettreAJourQuantiteArticle(listeId: number, articleId: number, delta: number): Observable<any> {
    if (delta > 0) {
      return this.incrementerQuantite(listeId, articleId);
    } else {
      return this.decrementerQuantite(listeId, articleId);
    }
  }
}