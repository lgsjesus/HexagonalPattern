import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

export interface Product {
  id: string;
  name: string;
  price: number;
  status: number;
}

export interface ApiResponse<T> {
  error: string; 
  success: boolean;
  data: T;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  //private readonly apiUrl = 'http://localhost:8081/api/Product'; // Replace with your actual backend URL
  private readonly apiUrl = 'https://localhost:44341/api/Product'; // Replace with your actual backend URL

  constructor(private readonly http: HttpClient) { }

  getAllProducts(): Observable<Product[]> {
    return this.http.get<ApiResponse<Product[]>>(`${this.apiUrl}/GetAll`).pipe(
      map(response => response.data) // Extract the 'data' property
    );
  }

  getProductById(id: string): Observable<Product> {
    return this.http.get<ApiResponse<Product>>(`${this.apiUrl}/Get/${id}`).pipe(
      map(response => response.data)
    );
  }

  createProduct(product: Product): Observable<Product> {
    return this.http.post<ApiResponse<Product>>(`${this.apiUrl}/Create`, product).pipe(
      map(response => response.data)
    );
  }

  updateProduct(product: Product): Observable<any> {
    return this.http.put<ApiResponse<any>>(`${this.apiUrl}/Update`, product).pipe(
      map(response => response.data)
    );
  }

  removeProductById(id: string): Observable<boolean> {
    return this.http.delete<ApiResponse<boolean>>(`${this.apiUrl}/Delete/${id}`).pipe(
      map(response => response.data)
    );
  }
}
