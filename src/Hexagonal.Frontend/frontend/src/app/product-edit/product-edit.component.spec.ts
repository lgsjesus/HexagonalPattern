import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductEditComponent } from './product-edit.component';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../services/product.service';
import { ToastService } from '../services/toast.service';
import { of, throwError } from 'rxjs';
import { ReactiveFormsModule } from '@angular/forms';
import { Product } from '../models/product.model';
describe('ProductEditComponent', () => {
    let component: ProductEditComponent;
    let fixture: ComponentFixture<ProductEditComponent>;
    let mockActivatedRoute: any;
    let mockProductService: any;
    let mockToastService: any;
    let mockRouter: any;
    beforeEach(async () => {
        mockActivatedRoute = {
            snapshot: {
                paramMap: {
                    get: (key: string) => '1' // Simulate an ID in the route
                }
            }
        };
        mockProductService = {
            getProductById: jasmine.createSpy('getProductById').and.returnValue(of({ id: '1', name: 'Test Product', description: 'Test Description', price: 10, status: 1 })),
            updateProduct: jasmine.createSpy('updateProduct').and.returnValue(of({}))
        };
        mockToastService = {
            show: jasmine.createSpy('show')
        };
        mockRouter = {
            navigate: jasmine.createSpy('navigate')
        };
        await TestBed.configureTestingModule({
            imports: [ReactiveFormsModule, ProductEditComponent],
            providers: [
                { provide: ActivatedRoute, useValue: mockActivatedRoute },
                { provide: ProductService, useValue: mockProductService },
                { provide: ToastService, useValue: mockToastService },
                { provide: Router, useValue: mockRouter }
            ]
        }).compileComponents();
        fixture = TestBed.createComponent(ProductEditComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });
    it('1. should create', () => {
        expect(component).toBeTruthy();
    });
    it('2. should fetch product on init if id is present', () => {
        expect(mockProductService.getProductById).toHaveBeenCalledWith('1');
        expect(component.productForm.value).toEqual({
            name: 'Test Product',
            description: 'Test Description',
            price: 10,
            status: 1
        });
    });
    it('3. should update product and navigate on success', () => {
        component.product = { id: '1', name: 'Original Product', description: 'Original Description', price: 5, status: 0 };
        component.productForm.setValue({ name: 'Updated Product', description: 'Updated Description', price: 15, status: 1 });
        component.updateProduct();
        expect(mockProductService.updateProduct).toHaveBeenCalledWith('1', {
            name: 'Updated Product',
            description: 'Updated Description',
            price: 15,
            status: 1
        });
        expect(mockToastService.show).toHaveBeenCalledWith('Product updated successfully', { classname: 'bg-success text-light', delay: 5000 });
        expect(mockRouter.navigate).toHaveBeenCalledWith(['/products']);
    });
    it('4. should show error toast on update failure', () => {
        mockProductService.updateProduct.and.returnValue(throwError(() => new Error('Update Failed')));
        component.product = { id: '1', name: 'Original Product', description: 'Original Description', price: 5, status: 0 };
        component.productForm.setValue({ name: 'Updated Product', description: 'Updated Description', price: 15, status: 1 });
        component.updateProduct();
        expect(mockProductService.updateProduct).toHaveBeenCalled();
        expect(mockToastService.show).toHaveBeenCalledWith('Error updating product', { classname: 'bg-danger text-light', delay: 5000 });
    });
    it('5. should navigate to product list on cancel', () => {
        component.cancel();
        expect(mockRouter.navigate).toHaveBeenCalledWith(['/products']);
    });
});
