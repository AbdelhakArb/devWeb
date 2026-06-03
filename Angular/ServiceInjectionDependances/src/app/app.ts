import { Component, signal } from '@angular/core';
import { Produits } from './composants/produits/produits';


@Component({
  selector: 'app-root',
  imports: [Produits],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('ServiceInjectionDependances');
}
  