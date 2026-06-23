import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'http://localhost:5141/api/comptes';
  private utilisateurSignal = signal<any | null>(null);

  public utilisateurConnecte = computed(() => this.utilisateurSignal());
  public estAuthentifie = computed(() => this.utilisateurSignal() !== null);

  constructor(private http: HttpClient) {
    const userStocke = localStorage.getItem('babyList_user');
    if (userStocke) {
      try { this.utilisateurSignal.set(JSON.parse(userStocke)); }
      catch (e) { localStorage.removeItem('babyList_user'); }
    }
  }

  connexion(email: string, motDePasse: string, type: 'Parent' | 'Visiteur'): Observable<any> {
    const endpoint = type === 'Parent' ? 'connexion-parent' : 'connexion-visiteur';
    return this.http.post(`${this.baseUrl}/${endpoint}`, { email, motDePasse }).pipe(
      tap((reponse: any) => {
        if (reponse && reponse.utilisateur) {
          this.utilisateurSignal.set(reponse.utilisateur);
          localStorage.setItem('babyList_user', JSON.stringify(reponse.utilisateur));
        }
      })
    );
  }

  inscription(donnees: any, type: 'parent' | 'visiteur'): Observable<any> {
    const endpoint = type === 'parent' ? 'inscription-parent' : 'inscription-visiteur';
    return this.http.post(`${this.baseUrl}/${endpoint}`, donnees);
  }

  deconnexion(): void {
    this.utilisateurSignal.set(null);
    localStorage.removeItem('babyList_user');
  }
}