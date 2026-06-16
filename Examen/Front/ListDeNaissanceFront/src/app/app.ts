import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CreerListe } from './pages/creer-liste/creer-liste'; // On importe ta page

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CreerListe], // On ajoute CreerListe ici
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent {
  title = 'mon-site-naissance';
}