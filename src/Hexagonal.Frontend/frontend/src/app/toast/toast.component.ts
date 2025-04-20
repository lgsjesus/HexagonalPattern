import { Component, OnInit, OnDestroy } from '@angular/core';
import { ToastService, ToastMessage } from '../services/toast.service';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  imports: [CommonModule],
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.css']
})
export class ToastComponent implements OnInit, OnDestroy {
  message: string = '';
  type: 'success' | 'error' | 'info' = 'info';
  isVisible: boolean = false;
  private subscription: Subscription = new Subscription;

  constructor(private readonly toastService: ToastService) { }

  ngOnInit(): void {
    this.subscription = this.toastService.toast$.subscribe(
      (toast: ToastMessage) => {
        this.message = toast.message;
        this.type = toast.type;
        this.isVisible = true;

        setTimeout(() => {
          this.isVisible = false;
        }, 3000); // Hide after 3 seconds (adjust as needed)
      }
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
