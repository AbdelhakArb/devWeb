import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Product, ProductResponse } from '../models/Product';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
private apiUrl = 'https://dummyjson.com/products';
private http = inject(HttpClient);

getProducts():Observable<ProductResponse> {

  return this.http.get<ProductResponse>(this.apiUrl);
}
}
