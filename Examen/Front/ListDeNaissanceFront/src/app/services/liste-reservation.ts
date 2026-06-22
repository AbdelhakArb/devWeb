import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PanierReservationDto } from './api/models/panier-reservation';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ListeReservationService {
  private apiUrl = 'https://localhost:5141/api/reserver'; // Vérifie ton URL exacte

  constructor(private http: HttpClient) {}

  reserver(panier: PanierReservationDto): Observable<any> {
    return this.http.post(this.apiUrl, panier);
  }
}