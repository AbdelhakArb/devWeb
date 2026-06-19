import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class HomeComponent {
  // Pas de logique lourde ici, ce sont les boutons du template (routerLink) 
  // qui vont rediriger les utilisateurs vers les bons espaces.
}