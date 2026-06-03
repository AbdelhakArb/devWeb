import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../model/ProductInterface';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  
private apiUrl = 'https://dummyjson.com/products/add';
private http = inject(HttpClient);


addProduct(product: Product): Observable<Product> {
  const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

 return this.http.post<Product>(this.apiUrl, product, httpOptions);

}

}
