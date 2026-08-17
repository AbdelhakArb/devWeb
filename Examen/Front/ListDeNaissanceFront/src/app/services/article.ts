import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Article } from './api/models/article';

@Injectable({ providedIn: 'root' })
export class ArticleService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5141/api/articles';
  
  private readonly _articles = signal<Article[]>([]);
  readonly articles = this._articles.asReadonly();

  loadArticles() {
    this.http.get<Article[]>(this.apiUrl).subscribe(data => {
      this._articles.set(data);
    });
  }

  diminuerStock(articleId: number) {
    this._articles.update(items => 
      items.map(i => (i.articleId === articleId || i.id === articleId) ? { ...i, articleQty: (i.articleQty ?? i.quantite ?? 1) - 1, quantite: (i.articleQty ?? i.quantite ?? 1) - 1 } : i)
    );
  }

  augmenterStock(articleId: number) {
    this._articles.update(items => 
      items.map(i => (i.articleId === articleId || i.id === articleId) ? { ...i, articleQty: (i.articleQty ?? i.quantite ?? 0) + 1, quantite: (i.articleQty ?? i.quantite ?? 0) + 1 } : i)
    );
  }
}