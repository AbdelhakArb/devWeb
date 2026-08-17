import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ListeModelsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = 'http://localhost:5141/api/modeleslistes';

  obtenirTousLesModels(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl);
  }

  obtenirArticlesDuModel(modelId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/${modelId}/articles`);
  }
}