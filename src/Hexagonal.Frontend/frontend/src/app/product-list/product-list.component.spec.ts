import { ComponentFixture, TestBed } from '@angular/core/testing'; 
import { of, throwError } from 'rxjs';
import { ProductListComponent } from './product-list.component';
import { ProductService } from '../services/product.service';
import { ToastService } from '../services/toast.service';
import { Product } from '../models/product.model';

describe('ProductListComponent', () => {
  let component: ProductListComponent;
  let fixture: ComponentFixture<ProductListComponent>;
  let productService: jasmine.SpyObj<ProductService>;
  let toastService: jasmine.SpyObj<ToastService>;

  beforeEach(async () => {
    const productServiceSpy = jasmine.createSpyObj('ProductService', ['getAllProducts', 'removeProductById']);
    const toastServiceSpy = jasmine.createSpyObj('ToastService', ['show']);

    await TestBed.configureTestingModule({ 
      imports: [ProductListComponent],
      providers: [
        { provide: ProductService, useValue: productServiceSpy },
        { provide: ToastService, useValue: toastServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ProductListComponent);
    component = fixture.componentInstance;
    productService = TestBed.inject(ProductService) as jasmine.SpyObj<ProductService>;
    toastService = TestBed.inject(ToastService) as jasmine.SpyObj<ToastService>;
  });

  it('1. should create', () => {
    expect(component).toBeTruthy();
  });

  it('2. should fetch products on ngOnInit', () => {
    const mockProducts: Product[] = [
      { id: '1', name: 'Product 1', description: 'Description 1', price: 10, status: 1 },
      { id: '2', name: 'Product 2', description: 'Description 2', price: 20, status: 0 }
    ];
    productService.getAllProducts.and.returnValue(of(mockProducts));

    fixture.detectChanges(); // Trigger ngOnInit

    expect(productService.getAllProducts).toHaveBeenCalled();
    expect(component.products).toEqual(mockProducts);
  });

  it('3. should delete a product and update the list', () => {
    const mockProducts: Product[] = [
      { id: '1', name: 'Product 1', description: 'Description 1', price: 10, status: 1 },
      { id: '2', name: 'Product 2', description: 'Description 2', price: 20, status: 0 }
    ];
    component.products = mockProducts;
    productService.removeProductById.and.returnValue(of(true));

    component.deleteProduct('1');

    expect(productService.removeProductById).toHaveBeenCalledWith('1');
    expect(component.products.length).toBe(1);
    expect(component.products[0].id).toBe('2');
    expect(toastService.show).toHaveBeenCalledWith('Product deleted successfully', { classname: 'bg-success text-light', delay: 5000 });
  });

  it('4. should handle error when deleting a product', () => {
    const mockProducts: Product[] = [
      { id: '1', name: 'Product 1', description: 'Description 1', price: 10, status: 1 },
      { id: '2', name: 'Product 2', description: 'Description 2', price: 20, status: 0 }
    ];
    component.products = mockProducts;
    productService.removeProductById.and.returnValue(throwError(() => new Error('Failed to delete')));

    component.deleteProduct('1');

    expect(productService.removeProductById).toHaveBeenCalledWith('1');
    expect(component.products.length).toBe(2); // Products should remain unchanged
    expect(toastService.show).toHaveBeenCalledWith('Error deleting product', { classname: 'bg-danger text-light', delay: 5000 });
  });
});
