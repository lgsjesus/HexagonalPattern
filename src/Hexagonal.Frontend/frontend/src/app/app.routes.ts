import { Routes } from '@angular/router';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductEditComponent } from './product-edit/product-edit.component';

export const routes: Routes = [{ path: 'products', component: ProductListComponent },
    { path: 'products/:id', component: ProductEditComponent },
    { path: '', redirectTo: '/products', pathMatch: 'full' },];
