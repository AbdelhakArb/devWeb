import { Component, signal } from '@angular/core';
import { AddProductComponent } from './composants/add-product-component/add-product-component';

@Component({
  selector: 'app-root',
  imports: [AddProductComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('FormulaireReactAvecValidation');
}
