import { Routes } from '@angular/router';
import { CreerListe } from './pages/creer-liste/creer-liste';
import { GestionListe } from './pages/gestion-liste/gestion-liste';

export const routes: Routes = [
  { path: '', redirectTo: 'creer-liste', pathMatch: 'full' },
  { path: 'creer-liste', component: CreerListe },
  { path: 'gestion-liste', component: GestionListe },
  // Plus tard, on ajoutera une sécurité (Guard) ici pour bloquer si non connecté
];