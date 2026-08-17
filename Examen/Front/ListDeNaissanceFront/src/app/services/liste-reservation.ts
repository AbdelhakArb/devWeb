import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface SoumettreReservationPayload {
  listeDeNaissanceId: number;
  articleId: number;
  qtySouhaitee: number;
  visiteurId: number;
  messageText: string;
  signatureMessage: string;
}

@Injectable({
  providedIn: 'root'
})
export class ListeReservationService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = 'http://localhost:5141/api/listenaissance';

  readonly articlesDeLaListe = signal<any[]>([]);

  chargerArticlesDeLaListe(listeId: number): void {
    this.http.get<any[]>(`${this.baseUrl}/${listeId}/articles`).subscribe({
      next: (articles) => this.articlesDeLaListe.set(articles),
      error: (err) => console.error('Erreur chargement articles :', err)
    });
  }

  enregistrerConsultation(listeId: number, visiteurId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/${listeId}/consultation`, { visiteurId });
  }

  soumettreReservations(panier: SoumettreReservationPayload[]): Observable<boolean> {
    return this.http.post<boolean>(`${this.baseUrl}/reserver`, panier);
  }

  cloturerListe(listeId: number, statusListe: string): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${listeId}/cloturer`, { statusListe });
  }
}