import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../services/product-service';

@Component({
  selector: 'app-produits',
  imports: [FormsModule],
  templateUrl: './produits.html',
  styleUrl: './produits.css',
})
export class Produits {
productservice = inject(ProductService);
products = toSignal(this.productservice.getProducts(), {initialValue: {products: []}});

constructor() {
  console.log(this.products());
}
}
