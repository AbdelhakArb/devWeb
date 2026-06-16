import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ListeDeNaissance } from './models/liste-naissance';

@Injectable({
  providedIn: 'root'
})
export class ListeNaissanceService {
  // Ajuste bien le port si ton backend C# utilise un autre port
  private apiUrl = 'http://localhost:5141/api/listedenaissance'; 

  constructor(private http: HttpClient) { }

  creerListe(liste: ListeDeNaissance): Observable<any> {
    return this.http.post<any>(this.apiUrl, liste);
  }
}