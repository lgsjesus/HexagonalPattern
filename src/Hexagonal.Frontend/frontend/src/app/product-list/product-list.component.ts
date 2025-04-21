import { Component, OnInit } from '@angular/core';
import { Product, ProductService } from '../services/product.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ToastService } from '../services/toast.service'; // Import

@Component({
  imports: [CommonModule,RouterModule],
  selector: 'app-product-list',
  templateUrl: 'product-list.component.html',
  styleUrls: ['product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];

  constructor(private readonly productService: ProductService, private readonly toastService: ToastService) { }

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts(): void {
    this.productService.getAllProducts().subscribe({
      next: products => this.products = products,
      error: error => {
        // Handle errors appropriately, e.g., display an error message
        this.toastService.showToast(error.Error, 'error');
      }
    });
  }
  deleteProduct(id: string): void {
    this.productService.removeProductById(id).subscribe({
      next: () => {
        this.products = this.products.filter(product => product.id !== id);
        this.toastService.showToast('Product deleted successfully!', 'success'); // Show success toast
      },
      error: error => {
        // Handle errors appropriately, e.g., display an error message
        this.toastService.showToast(error.Error, 'error'); // Show error toast
      }
    });
  }
}
