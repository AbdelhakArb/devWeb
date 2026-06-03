import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductService } from '../../services/product-service';
import { toSignal } from '@angular/core/rxjs-interop';
import { Product } from '../../model/ProductInterface';

@Component({
  selector: 'app-add-product-component',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './add-product-component.html',
  styleUrl: './add-product-component.css',
})
export class AddProductComponent {
  private readonly fb = inject(FormBuilder);
  registerForm = this.fb.group({
    title: ['',[Validators.required, Validators.minLength(3)]],
    price: [0,[Validators.required, Validators.min(1)]],
    description: ['',[Validators.required, Validators.minLength(10)]] ,
  });
  newProduct = signal<Product | null>(null);

  private readonly ProductService = inject(ProductService);

 onSubmit(): void {
  const product: Product = {
    title: this.registerForm.get('title')?.value || '',
    price: this.registerForm.get('price')?.value || 0,
    description: this.registerForm.get('description')?.value || '',
  };
  this.ProductService.addProduct(product).subscribe({
    next: (response) => {
      console.log('Product added successfully:', response);
      this.newProduct.set(response);
    },
    error: (error) => {
      console.error('Error adding product:', error);
    }});
    
  }

}
