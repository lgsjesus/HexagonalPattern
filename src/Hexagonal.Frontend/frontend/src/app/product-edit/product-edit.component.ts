import { Component, OnInit } from '@angular/core';
import { ActivatedRoute ,Router} from '@angular/router';
import { Product, ProductService } from '../services/product.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastService } from '../services/toast.service'; // Import

@Component({
  imports: [FormsModule,CommonModule],
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.css']
})
export class ProductEditComponent implements OnInit {
  product: Product | undefined;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly productService: ProductService,
    private readonly toastService: ToastService, // Inject the ToastService
    private readonly router: Router
  ) { }

  ngOnInit(): void {
    this.getProduct();
  }

  getProduct(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.productService.getProductById(id).subscribe(
        product => this.product = product
      );
    }
  }

  updateProduct(): void {
    if (this.product) {
      this.productService.updateProduct(this.product).subscribe(
        () => {
          // Handle success (e.g., navigate back to product list)
          this.toastService.showToast('Product updated successfully!','success'); // Show success toast
          this.router.navigate(['/products']); // Navigate to the product list after successful update
        },
        response => {
          // Handle error (e.g., display an error message)
          console.error('Error updating product:', response.error.Error);
          this.toastService.showToast(response.error.Error, 'error'); // Show error toast
        }
      );
    }
  }
  cancel(): void {
    this.router.navigate(['/products']); // Navigate back to the product list without saving changes  
  }
}
